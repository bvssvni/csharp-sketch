using System;

namespace Sketch
{
	public class SaveDocument
	{
		public static void SaveAs(Gtk.Window window, string fileExtension, ISave<string> data) {
			var filename = Utils.SaveFile.WithFilters(window, "*." + fileExtension);
			if (filename == null) return;
			if (!filename.EndsWith("." + fileExtension)) filename += "." + fileExtension;
			
			data.Save(filename);
		}
	}
}

