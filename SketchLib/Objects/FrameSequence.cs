using System;
using System.Collections.Generic;

namespace Sketch
{
	public class FrameSequence
	{
		public List<Frame> Frames;

		public FrameSequence ()
		{
			Frames = new List<Frame>();
		}

		public void Save(Obf.OpenBinaryFormat w) {
			var frameSequence = w.StartBlock("FrameSequence");
			var n = Frames.Count;
			w.WriteInt("FramesLength", n);
			for (var i = 0; i < n; i++) {
				Frames[i].Save(w);
			}

			w.EndBlock(frameSequence);
		}

		public void Read(Obf.OpenBinaryFormat r) {
			var frameSequence = r.StartBlock("FrameSequence");
			var n = r.Read<int>("FramesLength", 0, frameSequence);
			for (var i = 0; i < n; i++) {
				var frame = new Frame();
				frame.Read(r);
				Frames.Add(frame);
			}

			r.EndBlock(frameSequence);
		}
	}
}

