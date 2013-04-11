using System;
using System.Collections.Generic;
using Utils.Document;

namespace Sketch
{
	public class Frame :
		IDraw<Cairo.Context>,
		IDraw<Cairo.ImageSurface>,
		ICopyTo<Frame>,
		IRead<Obf.OpenBinaryFormat>,
		IWriteVersion<Obf.OpenBinaryFormat, int>
	{
		public List<Stroke> Strokes;

		public Frame ()
		{
			Strokes = new List<Stroke>();
		}

		public void Draw(Cairo.Context context) {
			CairoFrameModule.Draw(context, this);
		}

		public void Draw(Cairo.ImageSurface surface) {
			using (var context = new Cairo.Context(surface)) {
				Utils.CairoFillModule.Fill(context, surface.Width, surface.Height, new Cairo.Color(1, 1, 1));
				context.Antialias = Cairo.Antialias.Subpixel;
				context.Color = new Cairo.Color(0, 0, 0);
				CairoFrameModule.Draw(context, this);
			}
		}

		public Frame CopyTo(Frame obj) {
			var n = Strokes.Count;
			for (var i = 0; i < n; i++) {
				obj.Strokes.Add(Strokes[i].CopyTo(new Stroke()));
			}

			return obj;
		}

		public void Save(Obf.OpenBinaryFormat w, int version) {
			var frame = w.StartBlock("Frame");
			var n = Strokes.Count;
			w.WriteInt("StrokesLength", n);
			for (int i = 0; i < n; i++) {
				Strokes[i].Save(w, version);
			}

			w.EndBlock(frame);
		}

		public void Read(Obf.OpenBinaryFormat r) {
			var frame = r.StartBlock("Frame");
			var n = r.Seek<int>("StrokesLength", 0, frame);
			for (int i = 0; i < n; i++) {
				var stroke = new Stroke();
				stroke.Read(r);
				Strokes.Add(stroke);
			}

			r.EndBlock(frame);
		}

	}
}

