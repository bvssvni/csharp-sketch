using System;
using System.Collections.Generic;

namespace Sketch
{
	public class Frame
	{
		public List<Stroke> Strokes;

		public Frame ()
		{
			Strokes = new List<Stroke>();
		}

		public void Draw(Cairo.Context context) {
			context.LineWidth = 3;
			context.LineCap = Cairo.LineCap.Round;

			var n = Strokes.Count;
			for (var i = 0; i < n; i++) {
				var stroke = Strokes[i];
				stroke.Draw(context);
			}
		}

		public Frame Copy() {
			var frame = new Frame();
			var n = Strokes.Count;
			for (var i = 0; i < n; i++) {
				frame.Strokes.Add(Strokes[i].Copy());
			}

			return frame;
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

