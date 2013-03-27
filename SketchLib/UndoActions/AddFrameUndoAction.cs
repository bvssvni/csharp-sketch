using System;

namespace Sketch
{
	public class AddFrameUndoAction : Utils.IUndoAction<App>
	{
		public int OldFrameIndex;
		public int NewFrameIndex;
		
		public AddFrameUndoAction(int oldFrameIndex, int newFrameIndex) {
			this.OldFrameIndex = oldFrameIndex;
			this.NewFrameIndex = newFrameIndex;
		}
		
		public void Undo(App app) {
			app.Data.Frames.RemoveAt(NewFrameIndex);
			app.SelectedFrame = OldFrameIndex;
		}
		
		public void Redo(App app) {
			app.Data.Frames.Insert(NewFrameIndex, new Frame());
			app.SelectedFrame = NewFrameIndex;
		}
		
		public void Dispose() {
			
		}
	}
}

