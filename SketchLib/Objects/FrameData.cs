using System;
using System.Collections.Generic;
using Utils.Document;

namespace Sketch
{
	public class FrameData :
		IRead<Obf.OpenBinaryFormat>,
		IWrite<Obf.OpenBinaryFormat>
	{
		public List<Frame> Frames;
		public int CurrentVersion = 1;

		public FrameData ()
		{
			Frames = new List<Frame>();
		}

		public void Save(Obf.OpenBinaryFormat w) {
			var frameSequence = w.StartBlock("FrameSequence");
			var n = Frames.Count;
			w.WriteInt("FramesLength", n);
			for (var i = 0; i < n; i++) {
				Frames[i].Save(w, CurrentVersion);
			}

			w.EndBlock(frameSequence);
		}

		public void Read(Obf.OpenBinaryFormat r) {
			var frameSequence = r.StartBlock("FrameSequence");
			var n = r.Seek<int>("FramesLength", 0, frameSequence);
			for (var i = 0; i < n; i++) {
				var frame = new Frame();
				frame.Read(r);
				Frames.Add(frame);
			}

			r.EndBlock(frameSequence);
		}

		public void ExportToPngs(string directory) {
			var surface = new Cairo.ImageSurface(Cairo.Format.ARGB32, 400, 300);
			var n = this.Frames.Count;
			for (int i = 0; i < n; i++) {
				var frame = this.Frames[i];
				string filename = directory + System.IO.Path.DirectorySeparatorChar + i.ToString().PadLeft(6, '0') + ".png";
				frame.Draw(surface);
				surface.WriteToPng(filename);
			}
		}
	}
}

