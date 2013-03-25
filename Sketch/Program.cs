using System;
using Gtk;

namespace Sketch
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			SketchWindow win = new SketchWindow ();
			win.Show ();
			Application.Run ();
		}
	}
}
