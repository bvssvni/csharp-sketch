using System;

namespace GenericInterface.Graphics
{
	public interface IDraw<T>
	{
		void Draw(T context);
	}
}

