using System;
using Utils.Document;

namespace Sketch
{
	public class RemoveFrameUndoAction : IUndoAction<App> {
		public int OldFrameIndex;
		public int NewFrameIndex;
		public Frame Frame;
		
		public RemoveFrameUndoAction(int oldFrameIndex, int newFrameIndex, Frame frame) {
			this.OldFrameIndex = oldFrameIndex;
			this.NewFrameIndex = newFrameIndex;
			this.Frame = frame;
		}
		
		public void Undo(App app) {
			var frameCopy = Frame.CopyTo(new Frame());
			app.Data.Frames.Insert(OldFrameIndex, frameCopy);
			app.SelectedFrame = OldFrameIndex;
		}
		
		public void Redo(App app) {
			app.Data.Frames.RemoveAt(OldFrameIndex);
			app.SelectedFrame = NewFrameIndex;
		}
		
		public void Dispose() {
			
		}
	}
}

