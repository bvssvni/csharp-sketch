using System;

namespace SketchToVideo
{
	public class VideoFormats
	{
		public static void RefreshComboBox(Gtk.ComboBox formatComboBox) {
			formatComboBox.AppendText("GIF");
		}

		public static bool DoesFFMpegExists() {
			// Only OSX is supported for now.
			var ffmpeg = FFMPEG.Path();
			return System.IO.File.Exists(ffmpeg);
		}

		public static void ConvertPNG(string folder, string filename, int framesPerSecond) {
			var ffmpeg = FFMPEG.Path();
			var input = folder + System.IO.Path.DirectorySeparatorChar + "%06d.png";
			var args = "-f image2 -r " + framesPerSecond + 
					" -i '" + input +
					"' -c:v libx264 -r 30 '" +  filename + "'";
			var info = new System.Diagnostics.ProcessStartInfo();
			// info.UseShellExecute = true;
			info.FileName = ffmpeg;
			info.Arguments = args;
			var process = System.Diagnostics.Process.Start(info);
			process.WaitForExit();
		}
	}
}

