using System;

namespace SketchToVideo
{
	public class FFMPEG
	{
		public static string Path() {
			var appDir = Environment.CurrentDirectory;
			var osxFFmpeg = "ffmpeg-osx";
			return appDir + System.IO.Path.DirectorySeparatorChar + osxFFmpeg;
		}
	}
}

