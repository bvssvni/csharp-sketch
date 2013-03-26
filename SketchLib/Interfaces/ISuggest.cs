using System;

namespace Sketch
{
	public delegate void AppActionDelegate<App>(App app);

	public interface ISuggest<App, Action>
	{
		AppActionDelegate<App> Suggest(Action action);
	}
}

