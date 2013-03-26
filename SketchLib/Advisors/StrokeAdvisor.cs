using System;

namespace Sketch
{
	public class StrokeAdvisor
	{
		private App m_app;
		
		public bool StartStroke;
		public bool Stroking;
		public bool EndStroke;

		public StrokeAdvisor(App app)
		{
			m_app = app;
		}

		public bool ShouldStartStroke() {
			if (m_app.IsBusy()) return false;
			if (m_app.StrokeHelper != null) return false;
			
			return StartStroke;
		}
		
		public bool ShouldStroke() {
			if (m_app.IsBusy()) return false;
			if (m_app.StrokeHelper == null) return false;
			
			return Stroking;
		}
		
		public bool ShouldEndStroke() {
			if (m_app.IsBusy()) return false;
			if (m_app.StrokeHelper == null) return false;
			
			return EndStroke;
		}

		public bool ShouldRefreshGraphics() {
			if (m_app.IsBusy()) return false;

			return true;
		}

		public bool ShouldRefreshTitle() {
			if (m_app.IsBusy()) return false;

			return true;
		}
	}
}

