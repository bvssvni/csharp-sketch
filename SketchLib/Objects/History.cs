using System;
using System.Collections.Generic;

namespace Sketch
{
	public interface IUndoAction :
		IUndo<App>,
		IRedo<App>,
		IDisposable
	{
	}

	public class History : List<IUndoAction>
	{
		private int m_cursor = 0;

		public int Cursor
		{
			get {return m_cursor;}
		}

		private class AddFrameUndoAction : IUndoAction
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

				app.RefreshUI(UI.Graphics);
				app.RefreshUI(UI.Title);
			}

			public void Redo(App app) {
				app.Data.Frames.Insert(NewFrameIndex, new Frame());
				app.SelectedFrame = NewFrameIndex;

				app.RefreshUI(UI.Graphics);
				app.RefreshUI(UI.Title);
			}

			public void Dispose() {

			}
		}

		private class RemoveFrameUndoAction : IUndoAction {
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

				app.RefreshUI(UI.Graphics);
				app.RefreshUI(UI.Title);
			}

			public void Redo(App app) {
				app.Data.Frames.RemoveAt(OldFrameIndex);
				app.SelectedFrame = NewFrameIndex;

				app.RefreshUI(UI.Graphics);
				app.RefreshUI(UI.Title);
			}

			public void Dispose() {

			}
		}

		private class AddStrokeUndoAction : IUndoAction
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

				app.RefreshUI(UI.Title);
				app.RefreshUI(UI.Graphics);
			}

			public void Redo(App app) {
				var strokeCopy = Stroke.CopyTo(new Stroke());
				app.Data.Frames[FrameIndex].Strokes.Insert(StrokeIndex, strokeCopy);
				app.SelectedFrame = FrameIndex;

				app.RefreshUI(UI.Title);
				app.RefreshUI(UI.Graphics);
			}

			public void Dispose() {

			}
		}

		public History ()
		{
		}

		public void Undo(App app) {
			if (m_cursor <= 0) return;
			
			m_cursor--;
			this[m_cursor].Undo(app);
		}

		public void Redo(App app) {
			if (m_cursor >= this.Count) return;

			this[m_cursor].Redo(app);
			m_cursor++;
		}

		public void AddStroke(App app) {
			int frameIndex = app.SelectedFrame;
			var frame = app.Data.Frames[frameIndex];
			int strokeIndex = frame.Strokes.Count-1;
			Stroke stroke = frame.Strokes[strokeIndex];
			var strokeCopy = stroke.CopyTo(new Stroke());
			AddUndoAction(new AddStrokeUndoAction(frameIndex, strokeIndex, strokeCopy));
		}

		public void AddFrame(int oldFrameIndex, int newFrameIndex) {
			AddUndoAction(new AddFrameUndoAction(oldFrameIndex, newFrameIndex));
		}

		public void RemoveFrame(int oldFrameIndex, int newFrameIndex, Frame frame) {
			var frameCopy = frame.CopyTo(new Frame());
			AddUndoAction(new RemoveFrameUndoAction(oldFrameIndex, newFrameIndex, frameCopy));
		}

		public void AddUndoAction(IUndoAction action) {
			// Remove the history up to the current point.
			this.RemoveRange(m_cursor, this.Count - m_cursor);
			// Add action to history.
			this.Add(action);
			m_cursor++;
		}
	}
}

