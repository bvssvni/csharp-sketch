using System;

namespace Sketch
{
	/// <summary>
	/// This interface is for refreshing a part the user interface.
	/// </summary>
	public interface IUI<T>
	{
		void RefreshUI(T ui);
	}
}

