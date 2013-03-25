using System;

namespace Sketch
{
	public class SelectedFrameAction
	{
		public static void Delete(App app) {
			var frame = app.Data.Frames[app.SelectedFrame];
			var oldFrameIndex = app.SelectedFrame;
			app.Data.Frames.RemoveAt(app.SelectedFrame);
			app.SelectedFrame = app.Data.Frames.Count - 1;
			var newFrameIndex = app.SelectedFrame;
			app.History.RemoveFrame(oldFrameIndex, newFrameIndex, frame);
		}

		public static void GotoLast(App app) {
			app.SelectedFrame = app.Data.Frames.Count - 1;
		}

		public static void GotoFirst(App app) {
			app.SelectedFrame = 0;
		}

		public static void GotoPrevious(App app) {
			app.SelectedFrame--;
		}

		public static void GotoNext(App app) {
			app.SelectedFrame++;
		}

		public static void AddNewFrame(App app) {
			int oldFrameIndex = app.SelectedFrame;
			app.Data.Frames.Insert(app.SelectedFrame + 1, new Frame());
			app.SelectedFrame++;
			int newFrameIndex = app.SelectedFrame;
			app.History.AddFrame(oldFrameIndex, newFrameIndex);
		}
	}
}

