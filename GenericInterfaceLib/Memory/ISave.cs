using System;

namespace GenericInterface.Memory
{
	public interface ISave<T>
	{
		void Save(T w);
	}
}

