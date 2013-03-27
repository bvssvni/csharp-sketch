using System;

namespace Sketch
{
	/// <summary>
	/// An Advisor is an object that performs actions and refreshes interface in correct order.
	/// The order is controlled in the declaration of two enums:
	/// 
	/// 1. Action - declared within the advisor.
	/// 2. UserInteface - declared in App object.
	/// 
	/// For example, a DocumentAdvisor can control logic to open and save documents.
	/// For example, a FrameAdvisor can control logic to navigate an animation.
	/// For example, a HistoryAdvisor can control logic to undo or redo.
	/// </summary>
	public abstract class Advisor<Event, Action, UserInterface>
	{
		public abstract void DoAction(Event e, Action action);
		public abstract void Refresh(Event e, UserInterface ui);

		public void Do(Event e) {
			var actions = (Action[])Enum.GetValues(typeof(Action));
			for (int i = 0; i < actions.Length; i++) {
				var action = actions[i];
				DoAction(e, action);
			}

			var uis = (UserInterface[])Enum.GetValues(typeof(UserInterface));
			for (int i = 0; i < uis.Length; i++) {
				var ui = uis[i];
				Refresh(e, ui);
			}
		}
	}
}

