using System;

namespace Sketch
{
	public interface IShould<Event, Action>
	{
		bool Should(Event e, Action action);
	}
}

