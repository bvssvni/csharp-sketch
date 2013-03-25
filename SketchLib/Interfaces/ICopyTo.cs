using System;

namespace Sketch
{
	/// <summary>
	/// When implementing this interface,
	/// remember to return the object copied to.
	/// </summary>
	public interface ICopyTo<T>
	{
		T CopyTo(T obj);
	}
}

