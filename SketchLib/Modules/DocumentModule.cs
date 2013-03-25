using System;
using GenericInterface.Memory;

namespace Sketch
{
	public class DocumentModule
	{
		public static void Save(Gtk.Window window, string fileExtension, string filename, ISave<string> data) {
			if (filename == null) filename = Utils.SaveFileModule.WithFilters(window, "*." + fileExtension);
			if (filename == null) return;
			if (!filename.EndsWith("." + fileExtension)) filename += "." + fileExtension;
			
			data.Save(filename);
		}

		public static void SaveAs(Gtk.Window window, string fileExtension, ISave<string> data) {
			var filename = Utils.SaveFileModule.WithFilters(window, "*." + fileExtension);
			if (filename == null) return;
			if (!filename.EndsWith("." + fileExtension)) filename += "." + fileExtension;
			
			data.Save(filename);
		}

		public static void Open(Gtk.Window window, string fileExtension, IRead<string> data) {
			var filename = Utils.OpenFileModule.WithFilters(window, "*." + fileExtension);
			if (filename == null) return;
			
			data.Read(filename);
		}
	}
}

