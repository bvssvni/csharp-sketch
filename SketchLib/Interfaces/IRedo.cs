using System;

namespace Sketch
{
	public interface IRedo<T>
	{
		void Redo(T app);
	}
}

