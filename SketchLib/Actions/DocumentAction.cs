using System;

namespace Sketch
{
	public class DocumentAction
	{
		public static void Close(App app) {
			Gtk.Application.Quit();
		}

		public static void Open(App app, Gtk.Window window) {
			DocumentModule.Save(window, app.FileExtensions, app.FileName, app);
		}

		public static void CancelCloseEvent(App app, Gtk.DeleteEventArgs args) {
			args.RetVal = true;
		}

		public static void New(App app) {
			// If the program is started by passing file to command line, this might not work.
			System.Diagnostics.Process.Start(Environment.CommandLine);
		}

		public static void Save(App app, Gtk.Window window) {
			DocumentModule.Save(window, app.FileExtensions, app.FileName, app);
		}
	}
}

