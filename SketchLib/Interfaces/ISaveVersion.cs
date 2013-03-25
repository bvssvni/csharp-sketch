using System;

namespace Sketch
{
	public interface ISaveVersion<T, V>
	{
		void Save(T w, V version);
	}
}

