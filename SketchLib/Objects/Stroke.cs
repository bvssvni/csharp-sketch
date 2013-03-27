using System;
using System.Collections.Generic;
using Cairo;

namespace Sketch
{
	public class Stroke : 
		Utils.ICopyTo<Stroke>,
		Utils.IDraw<Cairo.Context>,
		Utils.IRead<Obf.OpenBinaryFormat>,
		Utils.IWriteVersion<Obf.OpenBinaryFormat, int>
	{
		public readonly List<double> X = new List<double>();
		public readonly List<double> Y = new List<double>();

		public int Count
		{
			get {return X.Count;}
		}

		public Stroke ()
		{
		}

		public Point this[int index] {
			get {
				return new Point(X[index], Y[index]);
			}
			set {
				X[index] = value.X;
				Y[index] = value.Y;
			}
		}

		public void Add(Point p) {
			X.Add(p.X);
			Y.Add(p.Y);
		}

		public void Draw(Cairo.Context context) {
			var n = this.Count;
			context.NewPath();
			context.MoveTo(this.X[0], this.Y[0]);
			for (var i = 1; i < n; i++) {
				context.LineTo(this.X[i], this.Y[i]);
			}

			context.Stroke();
		}

		public Stroke CopyTo(Stroke obj) {
			var n = this.Count;
			for (int i = 0; i < n; i++) {
				obj.Add(this[i]);
			}

			return obj;
		}

		public void Save(Obf.OpenBinaryFormat w, int version) {
			var stroke = w.StartBlock("Stroke");
			var n = this.Count;

			if (version == 0) {
				// Save each point with x and y component.
				w.WriteInt("Length", n);
				for (int i = 0; i < n; i++) {
					var point = w.StartBlock("Point");
					w.WriteDouble("X", X[i]);
					w.WriteDouble("Y", Y[i]);
					w.EndBlock(point);
				}
			} else if (version == 1) {
				var points = w.StartBlock("Points");
				var bw = w.Writer;
				bw.Write((int)X.Count);
				for (int i = 0; i < n; i++) {
					bw.Write((double)X[i]);
					bw.Write((double)Y[i]);
				}
				w.EndBlock(points);
			}

			w.EndBlock(stroke);
		}

		public void Read(Obf.OpenBinaryFormat r) {
			var stroke = r.StartBlock("Stroke");
			var next = r.NextId();

			if (next == "Points") {
				// Points means they are saved compressed.
				var points = r.StartBlock("Points");
				var br = r.Reader;
				var n = br.ReadInt32();
				for (int i = 0; i < n; i++) {
					this.Add(new Point(br.ReadDouble(), br.ReadDouble()));
				}
				r.EndBlock(points);
			} else {
				// Read each point one by one.
				var n = r.Seek<int>("Length", 0, stroke);
				for (int i = 0; i < n; i++) {
					var point = r.StartBlock("Point");
					var x = r.Seek<double>("X", 0, point);
					var y = r.Seek<double>("Y", 0, point);
					this.Add(new Point(x, y));
					r.EndBlock(point);
				}
			}

			r.EndBlock(stroke);
		}
	}
}

