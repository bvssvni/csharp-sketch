using System;

namespace Sketch
{
	public class PreviewAction
	{
		public static void Start(App app, SketchControl sketchControl) {
			sketchControl.Preview = true;
		}

		public static void Stop(App app, SketchControl sketchControl) {
			sketchControl.Preview = false;
		}
	}
}

