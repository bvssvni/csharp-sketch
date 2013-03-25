using System;

namespace Sketch
{
	public class SketchAdvisor
	{
		private App m_app;

		public bool DeleteSelectedFrame;
		public bool GotoLastFrame;
		public bool GotoFirstFrame;
		public bool GotoPreviousFrame;
		public bool GotoNextFrame;
		public bool AddNewFrame;
		public bool Closing;

		public SketchAdvisor(App app)
		{
			m_app = app;
		}

		public bool ShouldDeleteSelectedFrame() {
			if (m_app == null) return false;
			if (m_app.Data.Frames.Count <= 1) return false;
			if (m_app.IsBusy()) return false;

			return DeleteSelectedFrame;
		}

		public bool ShouldGotoLastFrame() {
			if (m_app.IsBusy()) return false;
			if (m_app.Data.Frames.Count == 0) return false;
			if (m_app.SelectedFrame == m_app.Data.Frames.Count-1) return false;

			return GotoLastFrame;
		}

		public bool ShouldGotoFirstFrame() {
			if (m_app.IsBusy()) return false;
			if (m_app.SelectedFrame == 0) return false;

			return GotoFirstFrame;
		}

		public bool ShouldGotoPreviousFrame() {
			if (m_app.IsBusy()) return false;
			if (m_app.SelectedFrame == 0) return false;

			return GotoPreviousFrame;
		}

		public bool ShouldGotoNextFrame() {
			if (m_app.IsBusy()) return false;
			if (m_app.SelectedFrame == m_app.Data.Frames.Count - 1) return false;

			return GotoNextFrame;
		}

		public bool ShouldAddNewFrame() {
			if (m_app.IsBusy()) return false;

			return AddNewFrame;
		}

		public bool ShouldClose() {
			if (m_app.IsBusy()) return false;

			return Closing;
		}

		public bool ShouldRefreshTitle() {
			if (m_app.IsBusy()) return false;

			return true;
		}

		public bool ShouldRefreshGraphics() {
			if (m_app.IsBusy()) return false;

			return true;
		}
	}
}

