using System;
using Utils.Document;

namespace Sketch
{
	/// <summary>
	/// This object stores application data and delegates refreshes to the interface.
	/// Usually there is one App object per window.
	/// </summary>
	public class App :
		IDraw<Cairo.Context>,
		IRead<string>,
		IWrite<string>
	{
		public enum UI
		{
			Title,
			Graphics,
			Buttons
		}
		
		public delegate bool IsBusyDelegate();
		public delegate void RefreshDelegate(UI ui);
		
		public IsBusyDelegate IsBusy;
		public RefreshDelegate Refresh;

		public History<App> History;

		public StrokeHelper StrokeHelper;

		public FrameData Data;
		public int SelectedFrame;
		public string FileName;
		public string[] FileExtensions = new string[]{".obf.gz", ".obf"};

		public App ()
		{
			try {
				// Add history.
				History = new History<App>();

				// Add default frame.
				Data = new FrameData();
				Data.Frames.Add(new Frame());
				SelectedFrame = 0;
			} catch (NullReferenceException ex) {
				Console.Error.WriteLine(ex);
			}
		}
		
		public void Draw(Cairo.Context context) {
			try {
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
			} catch (NullReferenceException ex) {
				Console.Error.WriteLine(ex);
			}
		}
		
		public void Save(string filename) {
			try {
				Obf.OpenBinaryFormat w = Obf.OpenBinaryFormat.ToFile(filename);
				var sketchDocument = w.StartBlock("SketchDocument");
				Data.Save(w);
				w.EndBlock(sketchDocument);
				w.Close();
				this.History = new History<App>();
				this.FileName = filename;
			} catch (NullReferenceException ex) {
				Console.Error.WriteLine(ex);
			}
		}
		
		public void Read(string filename) {
			try {
				Obf.OpenBinaryFormat r = Obf.OpenBinaryFormat.FromFile(filename);
				var sketchDocument = r.SeekBlock("SketchDocument");
				var frameData = new FrameData();
				frameData.Read(r);
				r.EndBlock(sketchDocument);
				r.Close();
				this.Data = frameData;
				this.SelectedFrame = 0;
				this.History = new History<App>();
				this.FileName = filename;
			} catch (NullReferenceException ex) {
				Console.Error.WriteLine(ex);
			}
		}
	}
}

