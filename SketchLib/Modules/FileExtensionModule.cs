using System;

namespace Sketch
{
	public class FileExtensionModule
	{
		public static string[] AddStar(string[] extensions) {
			var result = new string[extensions.Length];
			for (var i = 0; i < result.Length; i++) {
				result[i] = "*" + extensions[i];
			}
			return result;
		}

		public static bool EndsWithAny(string file, string[] extensions) {
			for (var i = 0; i < extensions.Length; i++) {
				if (file.EndsWith(extensions[i])) return true;
			}

			return false;
		}
	}
}

