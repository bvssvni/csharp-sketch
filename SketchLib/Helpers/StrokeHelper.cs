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
			app.History.AddStroke(app);
		}
	}
}

