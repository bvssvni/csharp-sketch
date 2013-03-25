using System;
using System.Collections;

namespace Sketch
{
	public class FrameNavigationModule
	{
		public static int Last(int frames) {
			return frames-1;
		}

		public static int First() {
			return 0;
		}

		public static int Next(int frames, int selectedFrame) {
			if (selectedFrame >= frames - 1) return frames - 1;

			return selectedFrame + 1;
		}

		public static int Previous(int selectedFrame) {
			if (selectedFrame <= 0) return 0;

			return selectedFrame - 1;
		}
	}
}

