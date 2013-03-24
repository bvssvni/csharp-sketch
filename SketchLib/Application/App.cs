using System;

namespace Sketch
{
	public class App
	{
		public FrameSequence FrameData;
		public int SelectedFrame;
		public string FileName;
		public string FileExtension = "obf";

		public History History;

		public delegate void RefreshDelegate();
		public delegate bool IsBusyDelegate();

		public RefreshDelegate RefreshGraphics;
		public RefreshDelegate RefreshTitle;
		public IsBusyDelegate IsBusy;

		public bool HasSelectedFrame {
			get {
				if (FrameData.Frames.Count == 0) return false;
				if (SelectedFrame < 0) return false;
				if (SelectedFrame >= FrameData.Frames.Count) return false;

				return true;
			}
		}

		public bool HasNextFrame {
			get {
				if (SelectedFrame == FrameData.Frames.Count-1) return false;

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
				if (FrameData.Frames.Count < 2) return false;

				return true;
			}
		}

		public void NavigateToLastFrame() {
			this.SelectedFrame = this.FrameData.Frames.Count-1;
		}

		public void NavigateToFirstFrame() {
			this.SelectedFrame = 0;
		}

		public void RemoveSelectedFrame() {
			if (this.FrameData.Frames.Count <= 1) return;

			var frame = this.FrameData.Frames[this.SelectedFrame];
			var oldFrameIndex = this.SelectedFrame;
			this.FrameData.Frames.RemoveAt(this.SelectedFrame);
			if (this.SelectedFrame >= this.FrameData.Frames.Count) {
				this.SelectedFrame = this.FrameData.Frames.Count-1;
			}

			var newFrameIndex = this.SelectedFrame;
			History.RemoveFrame(oldFrameIndex, newFrameIndex, frame);
		}

		public void BeginStroke(Stroke stroke) {
			FrameData.Frames[SelectedFrame].Strokes.Add(stroke);
		}

		public void EndStroke() {
			var frame = FrameData.Frames[SelectedFrame];
			if (frame.Strokes.Count == 0) return;

			History.AddStroke(this);
		}

		public void AddNewFrame() {
			int oldFrameIndex = SelectedFrame;
			FrameData.Frames.Insert(this.SelectedFrame + 1, new Frame());
			SelectedFrame++;
			int newFrameIndex = SelectedFrame;

			History.AddFrame(oldFrameIndex, newFrameIndex);
		}

		public Stroke LastStroke() {
			if (SelectedFrame < 0 || SelectedFrame >= FrameData.Frames.Count) return null;

			var strokes = FrameData.Frames[SelectedFrame].Strokes;
			if (strokes.Count == 0) return null;

			return strokes[strokes.Count-1];
		}

		public void DrawSelectedFrame(Cairo.Context context) {
			var prevFrame = this.SelectedFrame-1;
			var prevOpacity = 0.2;
			if (prevFrame < 0) {
				prevFrame = this.FrameData.Frames.Count-1;
				prevOpacity = 0.1;
			}

			var nextFrame = this.SelectedFrame+1;
			var nextOpacity = 0.2;
			if (nextFrame >= this.FrameData.Frames.Count) {
				nextFrame = 0;
				nextOpacity = 0.1;
			}

			if (prevFrame != SelectedFrame) {
				context.Color = new Cairo.Color(1, 0, 0, prevOpacity);
				FrameData.Frames[prevFrame].Draw(context);
			}
			if (nextFrame != SelectedFrame) {
				context.Color = new Cairo.Color(0, 0, 1, nextOpacity);
				FrameData.Frames[nextFrame].Draw(context);
			}

			context.Color = new Cairo.Color(0, 0, 0);
			FrameData.Frames[SelectedFrame].Draw(context);

		}

		public void Save(string filename) {
			Obf.OpenBinaryFormat w = Obf.OpenBinaryFormat.ToFile(filename);
			var sketchDocument = w.StartBlock("SketchDocument");
			FrameData.Save(w);
			w.EndBlock(sketchDocument);
			w.Close();
			this.History = new History();
			this.FileName = filename;
		}

		public void Read(string filename) {
			Obf.OpenBinaryFormat r = Obf.OpenBinaryFormat.FromFile(filename);
			var sketchDocument = r.StartBlock("SketchDocument");
			var frameData = new FrameSequence();
			frameData.Read(r);
			r.EndBlock(sketchDocument);
			r.Close();
			this.FrameData = frameData;
			this.SelectedFrame = 0;
			this.History = new History();
			this.FileName = filename;
		}

		public App ()
		{
			// Add history.
			History = new History();

			// Add default frame.
			FrameData = new FrameSequence();
			FrameData.Frames.Add(new Frame());
			SelectedFrame = 0;
		}
	}
}

