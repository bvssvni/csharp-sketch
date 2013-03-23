using System;
using System.Collections.Generic;

namespace Sketch
{
	public interface IUndoAction
	{
		void Undo(App app);
		void Redo(App app);
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
				app.FrameData.Frames.RemoveAt(NewFrameIndex);
				app.SelectedFrame = OldFrameIndex;

				app.RefreshGraphics();
				app.RefreshTitle();
			}

			public void Redo(App app) {
				app.FrameData.Frames.Insert(NewFrameIndex, new Frame());
				app.SelectedFrame = NewFrameIndex;

				app.RefreshGraphics();
				app.RefreshTitle();
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
				app.FrameData.Frames.Insert(OldFrameIndex, Frame.Copy());
				app.SelectedFrame = OldFrameIndex;

				app.RefreshGraphics();
				app.RefreshTitle();
			}

			public void Redo(App app) {
				app.FrameData.Frames.RemoveAt(OldFrameIndex);
				app.SelectedFrame = NewFrameIndex;

				app.RefreshGraphics();
				app.RefreshTitle();
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
				app.FrameData.Frames[FrameIndex].Strokes.RemoveAt(StrokeIndex);
				app.SelectedFrame = FrameIndex;

				app.RefreshTitle();
				app.RefreshGraphics();
			}

			public void Redo(App app) {
				app.FrameData.Frames[FrameIndex].Strokes.Insert(StrokeIndex, Stroke.Copy());
				app.SelectedFrame = FrameIndex;

				app.RefreshTitle();
				app.RefreshGraphics();
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
			var frame = app.FrameData.Frames[frameIndex];
			int strokeIndex = frame.Strokes.Count-1;
			Stroke stroke = frame.Strokes[strokeIndex];
			AddUndoAction(new AddStrokeUndoAction(frameIndex, strokeIndex, stroke.Copy()));
		}

		public void AddFrame(int oldFrameIndex, int newFrameIndex) {
			AddUndoAction(new AddFrameUndoAction(oldFrameIndex, newFrameIndex));
		}

		public void RemoveFrame(int oldFrameIndex, int newFrameIndex, Frame frame) {
			AddUndoAction(new RemoveFrameUndoAction(oldFrameIndex, newFrameIndex, frame.Copy()));
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

