using System;

namespace Sketch
{
	public class DocumentAdvisor :
		Utils.UserInterfaceAdvisor<DocumentAdvisor.Event, DocumentAdvisor.Action, App.UI>
	{
		private App m_app;

		public enum Event
		{
			Closing,
			Open,
			New,
			Save,
			SaveAs
		}

		public enum Action
		{
			Close,
			CancelCloseEvent,
			Open,
			New,
			Save,
			SaveAs
		}

		public DocumentAdvisor(App app)
		{
			m_app = app;
		}

		public override void DoAction(Event e, Action action)
		{
			if (m_app.IsBusy()) return;

			switch (action) {
				case Action.CancelCloseEvent: if (ShouldCancelCloseEvent(e)) DocumentAction.CancelCloseEvent(m_app); return;
				case Action.Close: if (ShouldClose(e)) DocumentAction.Close(m_app); return;
				case Action.New: if (ShouldNew(e)) DocumentAction.New(m_app); return;
				case Action.Open: if (ShouldOpen(e)) DocumentAction.Open(m_app); return;
				case Action.Save: if (ShouldSave(e)) DocumentAction.Save(m_app); return;
				case Action.SaveAs: if (ShouldSaveAs(e)) DocumentAction.SaveAs(m_app); return;
			}
		}

		public override void Refresh(Event e, App.UI ui) {
			if (m_app.IsBusy()) return;

			if (ui == App.UI.Graphics) {
				switch (e) {
					case Event.New:
					case Event.Open:
						m_app.Refresh(ui);
						return;
				}
			} else if (ui == App.UI.Title) {
				switch (e) {
					case Event.New:
					case Event.Open:
					case Event.Save:
					case Event.SaveAs:
						m_app.Refresh(ui);
						return;

				}
			}
		}

		public bool ShouldClose(Event e) {
			if (m_app.IsBusy()) return false;
			
			return e == Event.Closing;
		}

		public bool ShouldCancelCloseEvent(Event e) {
			if (!m_app.IsBusy()) return false;
			if (m_app.DeleteEventArgs == null) return false;

			return e == Event.Closing;
		}
		
		public bool ShouldOpen(Event e) {
			if (m_app.IsBusy()) return false;
			if (m_app.Window == null) return false;
			
			return e == Event.Open;
		}

		public bool ShouldNew(Event e) {
			if (m_app.IsBusy()) return false;
			
			return e == Event.New;
		}

		public bool ShouldSave(Event e) {
			if (m_app.IsBusy()) return false;
			if (m_app.Window == null) return false;
			
			return e == Event.Save;
		}
		
		public bool ShouldSaveAs(Event e) {
			if (m_app.IsBusy()) return false;
			if (m_app.Window == null) return false;
			
			return e == Event.SaveAs;
		}
	}
}

