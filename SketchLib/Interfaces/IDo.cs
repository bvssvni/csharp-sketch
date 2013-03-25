using System;

namespace Sketch
{
	public interface IDo<T>
	{
		void Do(T action);
	}
}

