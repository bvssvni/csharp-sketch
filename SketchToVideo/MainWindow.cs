using System;
using Gtk;
using SketchToVideo;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow(): base (Gtk.WindowType.Toplevel)
	{
		Build();

		VideoFormats.RefreshComboBox(formatComboBox);
	}
	
	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}

	protected void OnExportButtonClicked(object sender, EventArgs e)
	{
		if (!VideoFormats.DoesFFMpegExists()) return;

		var file = filechooserwidget1.Filename;
		var directory = System.IO.Path.GetDirectoryName(file);
		var filename = System.IO.Path.GetFileNameWithoutExtension(file);
		var tmpDirectory = directory + System.IO.Path.DirectorySeparatorChar + filename + "-tmp";
		// Create the directory if it doesn't exists.
		if (!System.IO.Directory.Exists(tmpDirectory)) {
			System.IO.Directory.CreateDirectory(tmpDirectory);
		}

		var frameData = new Sketch.FrameData();
		var f = Obf.OpenBinaryFormat.FromFile(file);
		var sketchDocument = f.StartBlock("SketchDocument");
		frameData.Read(f);
		f.EndBlock(sketchDocument);
		f.Close();
		frameData.ExportToPngs(tmpDirectory);

		var outFile = file + ".mp4";
		if (System.IO.File.Exists(outFile)) {
			System.IO.File.Delete(outFile);
		}

		VideoFormats.ConvertPNG(tmpDirectory, outFile, 10);

		System.IO.Directory.Delete(tmpDirectory, true);
	}
}
