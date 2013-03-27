using System;
using Utils.Document;

namespace Sketch
{
	public class AddStrokeUndoAction : IUndoAction<App>
	{
		public int FrameIndex;
		public int StrokeIndex;
		public Stroke Stroke;
		
		public AddStrokeUndoAction(int frame, int index, Stroke stroke) {
			this.FrameIndex = frame;
			this.StrokeIndex = index;
			this.Stroke = stroke;
		}
		
		public void Undo(App app) {
			app.Data.Frames[FrameIndex].Strokes.RemoveAt(StrokeIndex);
			app.SelectedFrame = FrameIndex;
		}
		
		public void Redo(App app) {
			var strokeCopy = Stroke.CopyTo(new Stroke());
			app.Data.Frames[FrameIndex].Strokes.Insert(StrokeIndex, strokeCopy);
			app.SelectedFrame = FrameIndex;
		}
		
		public void Dispose() {
			
		}
	}
}

