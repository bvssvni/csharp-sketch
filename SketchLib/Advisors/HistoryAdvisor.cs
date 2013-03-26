using System;

namespace Sketch
{
	public class HistoryAdvisor :
		IShould<HistoryAdvisor.Event, HistoryAdvisor.Action>,
		ISuggest<App, HistoryAdvisor.Action>
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

		public AppActionDelegate<App> Suggest(Action action) {
			switch (action) {
				case Action.Undo: return m_app.History.Undo;
				case Action.Redo: return m_app.History.Redo;
			}

			return null;
		}

		public bool Should(Event e, Action action) {
			if (m_app.IsBusy()) return false;

			switch (action) {
				case Action.Undo: return ShouldUndo(e);
				case Action.Redo: return ShouldRedo(e);
			}

			return false;
		}

		private bool ShouldUndo(Event e) {
			if (m_app.History.Cursor == 0) return false;
			
			return e == Event.UndoClicked;
		}
		
		private bool ShouldRedo(Event e) {
			if (m_app.History.Cursor == m_app.History.Count) return false;
			
			return e == Event.RedoClicked;
		}
	}
}

