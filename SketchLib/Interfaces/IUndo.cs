using System;

namespace Sketch
{
	public interface IUndo<T>
	{
		void Undo(T app);
	}
}

