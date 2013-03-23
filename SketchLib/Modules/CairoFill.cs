using System;

namespace Sketch
{
	public class CairoFill
	{
		public static void Fill(Cairo.Context context, Gtk.Widget widget, Cairo.Color color) {
			context.NewPath();
			context.Color = color;
			context.Rectangle(0, 0, widget.Allocation.Width, widget.Allocation.Height);
			context.Fill();
		}
	}
}
