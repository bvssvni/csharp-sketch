using System;

namespace Sketch
{
	public class Point
	{
		public readonly double X;
		public readonly double Y;

		public Point (double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		public void Save(Obf.OpenBinaryFormat w) {
			var point = w.StartBlock("Point");
			w.WriteDouble("X", this.X);
			w.WriteDouble("Y", this.Y);
			w.EndBlock(point);
		}
	}
}

