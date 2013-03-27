using System;
using NUnit.Framework;

namespace Sketch
{
	[TestFixture()]
	public class TestApp
	{
		[Test()]
		public void TestAddFrame()
		{
			App app = new App();
			Assert.True(app.Data.Frames.Count == 1);
			SelectedFrameAction.AddNewFrame(app);
			Assert.True(app.Data.Frames.Count == 2);
			Assert.True(app.Data.Frames[app.SelectedFrame] != null);
		}
	}
}

