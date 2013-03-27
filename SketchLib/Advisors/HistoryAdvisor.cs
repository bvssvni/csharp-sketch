using System;

namespace Sketch
{
	public class HistoryAdvisor :
		Utils.UserInterfaceAdvisor<HistoryAdvisor.Event, HistoryAdvisor.Action, App.UI>
	{
		private App m_app;

		public enum Action
		{
			Undo,
			Redo
		}

		public enum Event
		{
			UndoClicked,
			RedoClicked
		}

		public HistoryAdvisor(App app)
		{
			m_app = app;
		}

		public override void DoAction(Event e, Action action) {
			if (m_app.IsBusy()) return;

			switch (action) {
				case Action.Undo: if (ShouldUndo(e)) m_app.History.Undo(m_app); return;
				case Action.Redo: if (ShouldRedo(e)) m_app.History.Redo(m_app); return;
			}
		}

		public override void Refresh(Event e, App.UI ui) {
			if (m_app.IsBusy()) return;

			switch (ui) {
				case App.UI.Graphics: if (ShouldRefreshGraphics(e)) m_app.Refresh(ui); return;
				case App.UI.Title: if (ShouldRefreshTitle(e)) m_app.Refresh(ui); return;
			}
		}

		private bool ShouldUndo(Event e) {
			if (m_app.History.Cursor == 0) return false;
			
			return e == Event.UndoClicked;
		}
		
		private bool ShouldRedo(Event e) {
			if (m_app.History.Cursor == m_app.History.Count) return false;
			
			return e == Event.RedoClicked;
		}

		private bool ShouldRefreshGraphics(Event e) {
			return e == Event.RedoClicked ||
				e == Event.UndoClicked;
		}

		private bool ShouldRefreshTitle(Event e) {
			return e == Event.RedoClicked ||
				e == Event.UndoClicked;
		}
	}
}

