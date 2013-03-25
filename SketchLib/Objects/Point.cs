using System;

namespace Sketch
{
	public class Point : 
		GenericInterface.Memory.ICopyTo<Point>,
		GenericInterface.Memory.ISave<Obf.OpenBinaryFormat>,
		GenericInterface.Memory.IRead<Obf.OpenBinaryFormat>,
		GenericInterface.Algebra.IAdd<Point, Point>,
		GenericInterface.Algebra.ISubtract<Point, Point>,
		GenericInterface.Algebra.IMultiply<double, Point>,
		GenericInterface.Algebra.IDot<Point, double>,
		GenericInterface.Algebra.ICross<Point, double>
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

		public Point Add(Point b)
		{
			return new Point(this.X + b.X, this.Y + b.Y);
		}

		public Point Subtract(Point b)
		{
			return new Point(this.X - b.X, this.Y - b.Y);
		}

		public Point Multiply(double b) 
		{
			return new Point(this.X * b, this.Y * b);
		}

		public double Dot(Point b) {
			return this.X * b.X + this.Y * b.Y;
		}

		public double Cross(Point b) {
			return this.X * b.Y - this.Y * b.X;
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

