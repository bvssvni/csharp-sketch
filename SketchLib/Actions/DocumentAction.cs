using System;
using Utils.Document.Gtk;

namespace Sketch
{
	public class DocumentAction
	{
		public static void Close(App app) {
			try {
				Gtk.Application.Quit();
			} catch (NullReferenceException ex) {
				Console.Error.WriteLine(ex);
			}
		}

		public static void Open(App app, Gtk.Window window) {
			try {
				DocumentModule.Open(window, app.FileExtensions, app);
			} catch (NullReferenceException ex) {
				Console.Error.WriteLine(ex);
			}
		}

		public static void CancelCloseEvent(App app, Gtk.DeleteEventArgs args) {
			try {
				args.RetVal = true;
			} catch (NullReferenceException ex) {
				Console.Error.WriteLine(ex);
			}
		}

		public static void New(App app) {
			try {
				// If the program is started by passing file to command line, this might not work.
				System.Diagnostics.Process.Start(Environment.CommandLine);
			} catch (NullReferenceException ex) {
				Console.Error.WriteLine(ex);
			}
		}

		public static void Save(App app, Gtk.Window window) {
			try {
				DocumentModule.Save(window, app.FileExtensions, app.FileName, app);
			} catch (NullReferenceException ex) {
				Console.Error.WriteLine(ex);
			}
		}

		public static void SaveAs(App app, Gtk.Window window) {
			try {
				DocumentModule.Save(window, app.FileExtensions, app.FileName, app);
			} catch (NullReferenceException ex) {
				Console.Error.WriteLine(ex);
			}
		}
	}
}

