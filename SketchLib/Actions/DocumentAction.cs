using System;
using Utils.Document.Gtk;

namespace Sketch
{
	public class DocumentAction
	{
		public static void Close(App app) {
			Gtk.Application.Quit();
		}

		public static void Open(App app) {
			DocumentModule.Open(app.Window, app.FileExtensions, app);
		}

		public static void CancelCloseEvent(App app) {
			app.DeleteEventArgs.RetVal = true;
		}

		public static void New(App app) {
			// If the program is started by passing file to command line, this might not work.
			System.Diagnostics.Process.Start(Environment.CommandLine);
		}

		public static void Save(App app) {
			DocumentModule.Save(app.Window, app.FileExtensions, app.FileName, app);
		}

		public static void SaveAs(App app) {
			DocumentModule.Save(app.Window, app.FileExtensions, app.FileName, app);
		}
	}
}

