using System;

namespace Sketch
{
	public abstract class Advisor<Event, Action, UI>
	{
		public abstract void DoAction(Event e, Action action);
		public abstract void Refresh(Event e, UI ui);

		public void Do(Event e) {
			var actions = (Action[])Enum.GetValues(typeof(Action));
			for (int i = 0; i < actions.Length; i++) {
				var action = actions[i];
				DoAction(e, action);
			}

			var uis = (UI[])Enum.GetValues(typeof(UI));
			for (int i = 0; i < uis.Length; i++) {
				var ui = uis[i];
				Refresh(e, ui);
			}
		}
	}
}

