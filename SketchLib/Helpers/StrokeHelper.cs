using System;

namespace Sketch
{
	public class StrokeHelper
	{
		private Frame m_frame;
		private Stroke m_stroke;

		public StrokeHelper()
		{
		}

		public void Step1_SetFrame(Frame frame) {
			m_frame = frame;
		}

		public void Step2_CreateStroke() {
			m_stroke = new Stroke();
			m_frame.Strokes.Add(m_stroke);
		}

		public void Step3_AddPoint(Point p) {
			m_stroke.Add(p);
		}

		public void Step4_EndStroke(App app) {
			int frameIndex = app.SelectedFrame;
			var frame = app.Data.Frames[frameIndex];
			int strokeIndex = frame.Strokes.Count-1;
			Stroke stroke = frame.Strokes[strokeIndex];
			var strokeCopy = stroke.CopyTo(new Stroke());
			app.History.AddUndoAction(new AddStrokeUndoAction(frameIndex, strokeIndex, strokeCopy));
		}
	}
}

