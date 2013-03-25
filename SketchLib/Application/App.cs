using System;

namespace Sketch
{
	public class App :
		IDraw<Cairo.Context>,
		IRead<string>,
		ISave<string>
	{
		public FrameData Data;
		public int SelectedFrame;
		public string FileName;
		public string FileExtension = "obf";

		public History History;

		public delegate void RefreshDelegate();
		public delegate bool IsBusyDelegate();

		public RefreshDelegate RefreshGraphics;
		public RefreshDelegate RefreshTitle;
		public IsBusyDelegate IsBusy;

		public App ()
		{
			// Add history.
			History = new History();

			// Add default frame.
			Data = new FrameData();
			Data.Frames.Add(new Frame());
			SelectedFrame = 0;
		}

		
		public bool HasSelectedFrame {
			get {
				if (Data.Frames.Count == 0) return false;
				if (SelectedFrame < 0) return false;
				if (SelectedFrame >= Data.Frames.Count) return false;
				
				return true;
			}
		}
		
		public bool HasNextFrame {
			get {
				if (SelectedFrame == Data.Frames.Count-1) return false;
				
				return true;
			}
		}
		
		public bool HasPreviousFrame {
			get {
				if (SelectedFrame == 0) return false;
				
				return true;
			}
		}
		
		public bool HasMoreThanOneFrame {
			get {
				if (Data.Frames.Count < 2) return false;
				
				return true;
			}
		}
		
		public void NavigateToLastFrame() {
			this.SelectedFrame = this.Data.Frames.Count-1;
		}
		
		public void NavigateToFirstFrame() {
			this.SelectedFrame = 0;
		}
		
		public void RemoveSelectedFrame() {
			if (this.Data.Frames.Count <= 1) return;
			
			var frame = this.Data.Frames[this.SelectedFrame];
			var oldFrameIndex = this.SelectedFrame;
			this.Data.Frames.RemoveAt(this.SelectedFrame);
			if (this.SelectedFrame >= this.Data.Frames.Count) {
				this.SelectedFrame = this.Data.Frames.Count-1;
			}
			
			var newFrameIndex = this.SelectedFrame;
			History.RemoveFrame(oldFrameIndex, newFrameIndex, frame);
		}
		
		public void BeginStroke(Stroke stroke) {
			Data.Frames[SelectedFrame].Strokes.Add(stroke);
		}
		
		public void EndStroke() {
			var frame = Data.Frames[SelectedFrame];
			if (frame.Strokes.Count == 0) return;
			
			History.AddStroke(this);
		}
		
		public void AddNewFrame() {
			int oldFrameIndex = SelectedFrame;
			Data.Frames.Insert(this.SelectedFrame + 1, new Frame());
			SelectedFrame++;
			int newFrameIndex = SelectedFrame;
			
			History.AddFrame(oldFrameIndex, newFrameIndex);
		}
		
		public Stroke LastStroke() {
			if (SelectedFrame < 0 || SelectedFrame >= Data.Frames.Count) return null;
			
			var strokes = Data.Frames[SelectedFrame].Strokes;
			if (strokes.Count == 0) return null;
			
			return strokes[strokes.Count-1];
		}
		
		public void Draw(Cairo.Context context) {
			var prevFrame = this.SelectedFrame-1;
			var prevOpacity = 0.2;
			if (prevFrame < 0) {
				prevFrame = this.Data.Frames.Count-1;
				prevOpacity = 0.1;
			}
			
			var nextFrame = this.SelectedFrame+1;
			var nextOpacity = 0.2;
			if (nextFrame >= this.Data.Frames.Count) {
				nextFrame = 0;
				nextOpacity = 0.1;
			}
			
			if (prevFrame != SelectedFrame) {
				context.Color = new Cairo.Color(1, 0, 0, prevOpacity);
				Data.Frames[prevFrame].Draw(context);
			}
			if (nextFrame != SelectedFrame) {
				context.Color = new Cairo.Color(0, 0, 1, nextOpacity);
				Data.Frames[nextFrame].Draw(context);
			}
			
			context.Color = new Cairo.Color(0, 0, 0);
			Data.Frames[SelectedFrame].Draw(context);
			
		}
		
		public void Save(string filename) {
			Obf.OpenBinaryFormat w = Obf.OpenBinaryFormat.ToFile(filename);
			var sketchDocument = w.StartBlock("SketchDocument");
			Data.Save(w);
			w.EndBlock(sketchDocument);
			w.Close();
			this.History = new History();
			this.FileName = filename;
		}
		
		public void Read(string filename) {
			Obf.OpenBinaryFormat r = Obf.OpenBinaryFormat.FromFile(filename);
			var sketchDocument = r.StartBlock("SketchDocument");
			var frameData = new FrameData();
			frameData.Read(r);
			r.EndBlock(sketchDocument);
			r.Close();
			this.Data = frameData;
			this.SelectedFrame = 0;
			this.History = new History();
			this.FileName = filename;
		}
	}
}

