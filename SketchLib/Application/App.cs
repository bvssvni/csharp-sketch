using System;

namespace Sketch
{
	public class App :
		IDraw<Cairo.Context>,
		IRead<string>,
		ISave<string>,
		IUI<UI>,
		IDo<SketchAdvisor>
	{
		public FrameData Data;
		private int m_selectedFrame;
		private string m_fileName;
		private string[] m_fileExtensions = new string[]{".obf.gz", ".obf"};

		private History m_history;

		public delegate void RefreshDelegate(UI ui);
		public delegate bool IsBusyDelegate();

		public RefreshDelegate Refresh;
		public RefreshDelegate RefreshTitle;
		public IsBusyDelegate IsBusy;

		public void RefreshUI(UI ui) {
			Refresh(ui);
		}

		public void Do(SketchAdvisor advisor) {
			if (advisor.ShouldDeleteSelectedFrame()) SelectedFrameAction.Delete(this);
			if (advisor.ShouldGotoLastFrame()) SelectedFrameAction.GotoLast(this);
			if (advisor.ShouldGotoFirstFrame()) SelectedFrameAction.GotoFirst(this);
			if (advisor.ShouldGotoPreviousFrame()) SelectedFrameAction.GotoPrevious(this);
			if (advisor.ShouldGotoNextFrame()) SelectedFrameAction.GotoNext(this);
			if (advisor.ShouldAddNewFrame()) SelectedFrameAction.AddNewFrame(this);
			if (advisor.ShouldClose()) ClosingAction.Close(this);
			if (advisor.ShouldRefreshTitle()) Refresh(UI.Title);
			if (advisor.ShouldRefreshGraphics()) Refresh(UI.Graphics);
		}

		public History History {
			get {
				return m_history;
			}
			set {
				m_history = value;
			}
		}

		public string[] FileExtensions {
			get {
				return m_fileExtensions;
			}
			set {
				m_fileExtensions = value;
			}
		}

		public string FileName {
			get {
				return m_fileName;
			}
			set {
				m_fileName = value;
			}
		}

		public int SelectedFrame {
			get {
				return m_selectedFrame;
			}
			set {
				m_selectedFrame = value;
			}
		}

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
		
		public void BeginStroke(Stroke stroke) {
			Data.Frames[SelectedFrame].Strokes.Add(stroke);
		}
		
		public void EndStroke() {
			var frame = Data.Frames[SelectedFrame];
			if (frame.Strokes.Count == 0) return;
			
			History.AddStroke(this);
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
			var sketchDocument = r.SeekBlock("SketchDocument");
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

