using System;

namespace Sketch
{
	public class Point
	{
		public double X;
		public double Y;

		public Point (double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		public Point Copy() {
			return new Point(this.X, this.Y);
		}

		public void Save(Obf.OpenBinaryFormat w) {
			var point = w.StartBlock("Point");
			w.WriteDouble("X", this.X);
			w.WriteDouble("Y", this.Y);
			w.EndBlock(point);
		}

		public void Read(Obf.OpenBinaryFormat r) {
			var point = r.StartBlock("Point");
			this.X = r.Read<double>("X", 0, point);
			this.Y = r.Read<double>("Y", 0, point);
			r.EndBlock(point);
		}
	}
}

