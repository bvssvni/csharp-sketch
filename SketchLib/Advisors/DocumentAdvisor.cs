using System;

namespace Sketch
{
	public class DocumentAdvisor
	{
		private App m_app;

		public Gtk.Window Window;
		public Gtk.DeleteEventArgs DeleteEvent;

		public bool Closing;
		public bool Open;
		public bool New;
		public bool Save;
		public bool SaveAs;

		public DocumentAdvisor(App app)
		{
			m_app = app;
		}

		public bool ShouldClose() {
			if (m_app.IsBusy()) return false;
			
			return Closing;
		}

		public bool ShouldCancelCloseEvent() {
			if (!m_app.IsBusy()) return false;
			if (DeleteEvent == null) return false;

			return Closing;
		}
		
		public bool ShouldOpen() {
			if (m_app.IsBusy()) return false;
			if (Window == null) return false;
			
			return Open;
		}

		public bool ShouldNew() {
			if (m_app.IsBusy()) return false;
			
			return New;
		}

		public bool ShouldSave() {
			if (m_app.IsBusy()) return false;
			if (Window == null) return false;
			
			return Save;
		}
		
		public bool ShouldSaveAs() {
			if (m_app.IsBusy()) return false;
			if (Window == null) return false;
			
			return SaveAs;
		}
	}
}

