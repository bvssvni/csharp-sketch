using System;

namespace Sketch
{
	public class DocumentModule
	{
		public static void Save(Gtk.Window window, string[] fileExtensions, string filename, ISave<string> data) {
			var filter = FileExtensionModule.AddStar(fileExtensions);
			if (filename == null) filename = Utils.SaveFileModule.WithFilters(window, filter);
			if (filename == null) return;
			if (!FileExtensionModule.EndsWithAny(filename, fileExtensions)) filename += fileExtensions[0];
			
			data.Save(filename);
		}

		public static void SaveAs(Gtk.Window window, string[] fileExtensions, ISave<string> data) {
			var filter = FileExtensionModule.AddStar(fileExtensions);
			var filename = Utils.SaveFileModule.WithFilters(window, filter);
			if (filename == null) return;
			if (!FileExtensionModule.EndsWithAny(filename, fileExtensions)) filename += fileExtensions[0];

			data.Save(filename);
		}

		public static void Open(Gtk.Window window, string[] fileExtensions, IRead<string> data) {
			var filter = FileExtensionModule.AddStar(fileExtensions);
			var filename = Utils.OpenFileModule.WithFilters(window, filter);
			if (filename == null) return;
			
			data.Read(filename);
		}
	}
}

