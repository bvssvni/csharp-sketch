using System;

namespace Sketch
{
	public class ClosingAction
	{
		public static void Close(App app) {
			Gtk.Application.Quit();
		}
	}
}

