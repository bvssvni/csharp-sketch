using System;
using NUnit.Framework;

namespace Sketch
{
	[TestFixture()]
	public class TestOpenSave
	{
		[Test()]
		public void TestOpenSavePoint()
		{
			var point = new Point(1, 2);
			var mem = new System.IO.MemoryStream();
			var f = Obf.OpenBinaryFormat.ToMemory(mem);
			point.Save(f);
			var bytes = mem.ToArray();
			f.Close();

			f = Obf.OpenBinaryFormat.FromBytes(bytes);
			var newPoint = new Point(0, 0);
			newPoint.Read(f);
			f.Close();

			Assert.True(newPoint.X == 1);
			Assert.True(newPoint.Y == 2);
		}

		[Test()]
		public void TestOpenSaveStroke() {
			var stroke = new Stroke();
			stroke.Add(new Point(1, 2));
			stroke.Add(new Point(3, 4));
			var mem = new System.IO.MemoryStream();
			var f = Obf.OpenBinaryFormat.ToMemory(mem);
			stroke.Save(f);
			var bytes = mem.ToArray();
			f.Close();

			f = Obf.OpenBinaryFormat.FromBytes(bytes);
			var newStroke = new Stroke();
			newStroke.Read(f);
			Assert.True(newStroke.Count == 2);
			Assert.True(newStroke[0].X == 1);
			Assert.True(newStroke[0].Y == 2);
			Assert.True(newStroke[1].X == 3);
			Assert.True(newStroke[1].Y == 4);
		}

		[Test()]
		public void TestOpenSaveFrameData()
		{
			var frameData = new FrameData();
			var frame = new Frame();
			frameData.Frames.Add(frame);
			var stroke = new Stroke();
			stroke.Add(new Point(0, 0));
			stroke.Add(new Point(100, 100));
			frame.Strokes.Add(stroke);

			System.IO.MemoryStream mem = new System.IO.MemoryStream();
			Obf.OpenBinaryFormat f = Obf.OpenBinaryFormat.ToMemory(mem);
			frameData.Save(f);

			var bytes = mem.ToArray();
			f.Close();

			f = Obf.OpenBinaryFormat.FromBytes(bytes);

			var newFrameData = new FrameData();
			newFrameData.Read(f);

			f.Close();

			Assert.True(newFrameData.Frames.Count == 1);
			frame = newFrameData.Frames[0];
			Assert.True(frame.Strokes.Count == 1);
			stroke = frame.Strokes[0];
			Assert.True(stroke.Count == 2);
			var p1 = stroke[0];
			Assert.True(p1.X == 0);
			Assert.True(p1.Y == 0);
			var p2 = stroke[1];
			Assert.True(p2.X == 100);
			Assert.True(p2.Y == 100);
		}
	}
}

