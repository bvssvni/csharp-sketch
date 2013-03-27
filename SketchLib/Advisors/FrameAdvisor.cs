using System;

namespace Sketch
{
	public class FrameAdvisor :
		Utils.UserInterfaceAdvisor<FrameAdvisor.Event, FrameAdvisor.Action, App.UI>
	{
		private App m_app;

		public enum Event
		{
			DeleteSelectedFrame,
			GotoLastFrame,
			GotoFirstFrame,
			GotoPreviousFrame,
			GotoNextFrame,
			AddNewFrame,
			Loading,
			StartPreview,
			StopPreview,
		}

		public enum Action
		{
			DeleteSelectedFrame,
			GotoLastFrame,
			GotoFirstFrame,
			GotoPreviousFrame,
			GotoNextFrame,
			AddFrame,
			StartPreview,
			StopPreview
		}

		public override void DoAction(Event e, Action action)
		{
			switch (action) {
				case Action.DeleteSelectedFrame: 
					if (ShouldDeleteSelectedFrame(e)) {
						SelectedFrameAction.Delete(m_app);
					}
					break;
				case Action.GotoLastFrame:
					if (ShouldGotoLastFrame(e)) {
						SelectedFrameAction.GotoLast(m_app);
					}
					break;
				case Action.GotoFirstFrame:
					if (ShouldGotoFirstFrame(e)) {
						SelectedFrameAction.GotoFirst(m_app);
					}
					break;
				case Action.GotoPreviousFrame:
					if (ShouldGotoPreviousFrame(e)) {
						SelectedFrameAction.GotoPrevious(m_app);
					}
					break;
				case Action.GotoNextFrame:
					if (ShouldGotoNextFrame(e)) {
						SelectedFrameAction.GotoNext(m_app);
					}
					break;
				case Action.AddFrame:
					if (ShouldAddNewFrame(e)) {
						SelectedFrameAction.AddNewFrame(m_app);
					}
					break;
			}
		}

		public override void Refresh(Event e, App.UI ui)
		{
			switch (ui) {
				case App.UI.Graphics:
					if (ShouldRefreshGraphics(e)) {
						m_app.Refresh(ui);
					}
					return;
				case App.UI.Title:
					if (ShouldRefreshTitle(e)) {
						m_app.Refresh(ui);
					}
					return;
				case App.UI.StartPreview:
					if (ShouldStartPreview(e)) {
						m_app.Refresh(ui);
					}
					return;
				case App.UI.StopPreview:
					if (ShouldStopPreview(e)) {
						m_app.Refresh(ui);
					}
					return;
			}
		}

		public FrameAdvisor(App app)
		{
			m_app = app;
		}

		private bool ShouldDeleteSelectedFrame(Event e) {
			if (m_app == null) return false;
			if (m_app.Data.Frames.Count <= 1) return false;
			if (m_app.IsBusy()) return false;

			return e == Event.DeleteSelectedFrame;
		}

		private bool ShouldGotoLastFrame(Event e) {
			if (m_app.IsBusy()) return false;
			if (m_app.Data.Frames.Count == 0) return false;
			if (m_app.SelectedFrame == m_app.Data.Frames.Count-1) return false;

			return e == Event.GotoLastFrame;
		}

		private bool ShouldGotoFirstFrame(Event e) {
			if (m_app.IsBusy()) return false;
			if (m_app.SelectedFrame == 0) return false;

			return e == Event.GotoFirstFrame;
		}

		private bool ShouldGotoPreviousFrame(Event e) {
			if (m_app.IsBusy()) return false;
			if (m_app.SelectedFrame == 0) return false;

			return e == Event.GotoPreviousFrame;
		}

		private bool ShouldGotoNextFrame(Event e) {
			if (m_app.IsBusy()) return false;
			if (m_app.SelectedFrame == m_app.Data.Frames.Count - 1) return false;

			return e == Event.GotoNextFrame;
		}

		private bool ShouldAddNewFrame(Event e) {
			if (m_app.IsBusy()) return false;

			return e == Event.AddNewFrame;
		}

		private bool ShouldStartPreview(Event e) {
			if (m_app.IsBusy()) return false;

			return e == Event.StartPreview;
		}

		private bool ShouldStopPreview(Event e) {
			return e == Event.StopPreview;
		}

		private bool ShouldRefreshTitle(Event e) {
			if (m_app.IsBusy()) return false;

			return true;
		}

		private bool ShouldRefreshGraphics(Event e) {
			if (m_app.IsBusy()) return false;

			return e != Event.Loading;
		}
	}
}

