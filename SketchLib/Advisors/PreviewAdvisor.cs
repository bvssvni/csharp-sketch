using System;

namespace Sketch
{
	public class PreviewAdvisor : 
		Utils.UserInterfaceAdvisor<PreviewAdvisor.Event, PreviewAdvisor.Action, App.UI>
	{
		private App m_app;
		private SketchControl m_sketchControl;

		public enum Event
		{
			StartPreview,
			StopPreview
		};

		public enum Action
		{
			StartPreview,
			StopPreview
		};

		public PreviewAdvisor(App app, SketchControl sketchControl)
		{
			m_app = app;
			m_sketchControl = sketchControl;
		}

		#region implemented abstract members of UserInterfaceAdvisor


		public override void DoAction(Event e, Action action)
		{
			switch (action) {
				case Action.StartPreview: 
					if (ShouldStartPreview(e)) {
						PreviewAction.Start(m_app, m_sketchControl); 
					}

					break;
				case Action.StopPreview:
					if (ShouldStopPreview(e)) {
						PreviewAction.Stop(m_app, m_sketchControl);
					}

					break;
			}
		}

		private bool ShouldStartPreview(Event e) {
			if (m_app.IsBusy()) return false;

			return e == Event.StartPreview;
		}

		private bool ShouldStopPreview(Event e) {
			return e == Event.StopPreview;
		}

		public override void Refresh(Event e, App.UI ui)
		{
			m_app.Refresh(ui);
		}

		#endregion

	}
}

