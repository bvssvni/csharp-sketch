using System;

namespace Sketch
{
	public interface IDraw<T>
	{
		void Draw(T context);
	}
}

