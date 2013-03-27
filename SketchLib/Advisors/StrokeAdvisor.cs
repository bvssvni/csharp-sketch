using System;

namespace Sketch
{
	public class StrokeAdvisor : 
		Utils.UserInterfaceAdvisor<StrokeAdvisor.Event, StrokeAdvisor.Action, App.UI>
	{
		private App m_app;

		public enum Event
		{
			StartStroke,
			Stroking,
			EndStroke
		}

		// There are no actions since everything happens through a helper.
		public enum Action
		{
		}

		public StrokeAdvisor(App app)
		{
			m_app = app;
		}

		public override void DoAction(Event e, Action action)
		{
		}

		public override void Refresh(Event e, App.UI ui)
		{
			if (m_app.IsBusy()) return;

			if (ui == App.UI.Graphics) {
				m_app.Refresh(ui);
			} else if (ui == App.UI.Title) {
				m_app.Refresh(ui);
			}
		}
	}
}

