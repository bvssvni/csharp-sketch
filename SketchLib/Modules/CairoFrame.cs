using System;
using Sketch;

namespace Sketch
{
	public class CairoFrame
	{
		public static void Draw(Cairo.Context context, Frame frame) {
			context.LineWidth = 3;
			context.LineCap = Cairo.LineCap.Round;

			var n = frame.Strokes.Count;
			for (var i = 0; i < n; i++) {
				var stroke = frame.Strokes[i];
				stroke.Draw(context);
			}
		}
	}
}

