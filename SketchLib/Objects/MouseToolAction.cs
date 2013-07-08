using System;

namespace Utils
{
	public class MouseToolAction
	{
		public enum ModifierKey {
			None = 0,
			Shift = 1,
			Alt = 2,
			Control = 4
		}

		public enum MouseButton {
			None = 0,
			Left = 1,
			Right = 2,
			Middle = 3
		}

		public delegate void PositionDelegate (double x, double y);

		public MouseButton Button;
		public ModifierKey Modifier;
		public PositionDelegate MouseDown;
		public PositionDelegate MouseMove;
		public PositionDelegate MouseUp;

		public MouseToolAction()
		{
		}
	}
}

