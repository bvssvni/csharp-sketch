using System;

namespace GenericInterface.Memory
{
	public interface IUndo<T>
	{
		void Undo(T app);
	}
}

