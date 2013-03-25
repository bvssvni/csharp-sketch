using System;

namespace Sketch
{
	public class Point : 
		ICopyTo<Point>,
		ISave<Obf.OpenBinaryFormat>,
		IRead<Obf.OpenBinaryFormat>
	{
		public double X;
		public double Y;

		public Point (double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		public Point CopyTo(Point obj) {
			obj.X = this.X;
			obj.Y = this.Y;
			return obj;
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

