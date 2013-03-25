using System;

namespace GenericInterface.Memory
{
	public interface IRedo<T>
	{
		void Redo(T app);
	}
}

