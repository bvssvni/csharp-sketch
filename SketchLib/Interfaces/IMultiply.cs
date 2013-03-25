using System;

namespace Sketch
{
	public interface IMultiply<Argument, Return>
	{
		Return Multiply(Argument b);
	}
}

