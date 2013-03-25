using System;
using System.Collections.Generic;

namespace Sketch
{
	public class Frame :
		IDraw<Cairo.Context>,
		IDraw<Cairo.ImageSurface>,
		ICopyTo<Frame>,
		IRead<Obf.OpenBinaryFormat>,
		ISave<Obf.OpenBinaryFormat>
	{
		public List<Stroke> Strokes;

		public Frame ()
		{
			Strokes = new List<Stroke>();
		}

		public void Draw(Cairo.Context context) {
			CairoFrame.Draw(context, this);
		}

		public void Draw(Cairo.ImageSurface surface) {
			using (var context = new Cairo.Context(surface)) {
				CairoFill.Fill(context, surface.Width, surface.Height, new Cairo.Color(1, 1, 1));
				context.Antialias = Cairo.Antialias.Subpixel;
				context.Color = new Cairo.Color(0, 0, 0);
				CairoFrame.Draw(context, this);
			}
		}

		public Frame CopyTo(Frame obj) {
			var n = Strokes.Count;
			for (var i = 0; i < n; i++) {
				obj.Strokes.Add(Strokes[i].CopyTo(new Stroke()));
			}

			return obj;
		}

		public void Save(Obf.OpenBinaryFormat w) {
			var frame = w.StartBlock("Frame");
			var n = Strokes.Count;
			w.WriteInt("StrokesLength", n);
			for (int i = 0; i < n; i++) {
				Strokes[i].Save(w);
			}

			w.EndBlock(frame);
		}

		public void Read(Obf.OpenBinaryFormat r) {
			var frame = r.StartBlock("Frame");
			var n = r.Read<int>("StrokesLength", 0, frame);
			for (int i = 0; i < n; i++) {
				var stroke = new Stroke();
				stroke.Read(r);
				Strokes.Add(stroke);
			}

			r.EndBlock(frame);
		}

	}
}

