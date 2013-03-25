using System;

namespace Sketch
{
	public class SketchReadSaveModule
	{
		public static void Save(System.IO.MemoryStream mem, App app) {
			Obf.OpenBinaryFormat w = Obf.OpenBinaryFormat.ToMemory(mem);
			var sketchDocument = w.StartBlock("SketchDocument");
			app.Data.Save(w);
			w.EndBlock(sketchDocument);
			w.Close();
		}

		public static void Read(System.IO.MemoryStream mem, App app) {

		}
	}
}

