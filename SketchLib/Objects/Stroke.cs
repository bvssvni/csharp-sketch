using System;
using System.Collections.Generic;
using Cairo;

namespace Sketch
{
	public class Stroke : 
		List<Point>,
		ICopyTo<Stroke>,
		IDraw<Cairo.Context>
	{
		public Stroke ()
		{
		}

		public void Draw(Cairo.Context context) {
			var n = this.Count;
			context.NewPath();
			context.MoveTo(this[0].X, this[0].Y);
			for (var i = 1; i < n; i++) {
				context.LineTo(this[i].X, this[i].Y);
			}

			context.Stroke();
		}

		public Stroke CopyTo(Stroke obj) {
			var n = this.Count;
			for (int i = 0; i < n; i++) {
				obj.Add(this[i].CopyTo(new Point(0, 0)));
			}

			return obj;
		}

		public void Save(Obf.OpenBinaryFormat w) {
			var stroke = w.StartBlock("Stroke");
			var n = this.Count;
			w.WriteInt("Length", n);
			for (int i = 0; i < n; i++) {
				this[i].Save(w);
			}

			w.EndBlock(stroke);
		}

		public void Read(Obf.OpenBinaryFormat r) {
			var stroke = r.StartBlock("Stroke");
			var n = r.Read<int>("Length", 0, stroke);
			for (int i = 0; i < n; i++) {
				var point = new Point(0, 0);
				point.Read(r);
				this.Add(point);
			}

			r.EndBlock(stroke);
		}
	}
}

