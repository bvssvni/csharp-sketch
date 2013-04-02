using System;

namespace Sketch
{
	public class DocumentAdvisor :
		Utils.UserInterfaceAdvisor<DocumentAdvisor.Event, DocumentAdvisor.Action, App.UI>
	{
		private App m_app;
		private Gtk.Window m_window;
		private Gtk.DeleteEventArgs m_deleteEventArgs;

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

		public DocumentAdvisor(App app, Gtk.Window window, Gtk.DeleteEventArgs args)
		{
			m_app = app;
			m_window = window;
			m_deleteEventArgs = args;
		}

		public override void DoAction(Event e, Action action)
		{
			if (m_app.IsBusy()) return;

			switch (action) {
				case Action.CancelCloseEvent: 
					if (ShouldCancelCloseEvent(e)) DocumentAction.CancelCloseEvent(m_app, m_deleteEventArgs); return;
				case Action.Close: if (ShouldClose(e)) DocumentAction.Close(m_app); return;
				case Action.New: if (ShouldNew(e)) DocumentAction.New(m_app); return;
				case Action.Open: if (ShouldOpen(e)) DocumentAction.Open(m_app, m_window); return;
				case Action.Save: if (ShouldSave(e)) DocumentAction.Save(m_app, m_window); return;
				case Action.SaveAs: if (ShouldSaveAs(e)) DocumentAction.SaveAs(m_app, m_window); return;
			}
		}

		public override void Refresh(Event e, App.UI ui) {
			if (m_app.IsBusy()) return;

			m_app.Refresh(ui);
		}

		public bool ShouldClose(Event e) {
			if (m_app.IsBusy()) return false;
			
			return e == Event.Closing;
		}

		public bool ShouldCancelCloseEvent(Event e) {
			if (!m_app.IsBusy()) return false;
			if (m_deleteEventArgs == null) return false;

			return e == Event.Closing;
		}
		
		public bool ShouldOpen(Event e) {
			if (m_app.IsBusy()) return false;
			if (m_window == null) return false;
			
			return e == Event.Open;
		}

		public bool ShouldNew(Event e) {
			if (m_app.IsBusy()) return false;
			
			return e == Event.New;
		}

		public bool ShouldSave(Event e) {
			if (m_app.IsBusy()) return false;
			if (m_window == null) return false;
			
			return e == Event.Save;
		}
		
		public bool ShouldSaveAs(Event e) {
			if (m_app.IsBusy()) return false;
			if (m_window == null) return false;
			
			return e == Event.SaveAs;
		}
	}
}

