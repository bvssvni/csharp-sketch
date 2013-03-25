using System;

namespace GenericInterface.Algebra
{
	public interface IMultiply<Argument, Return>
	{
		Return Multiply(Argument b);
	}
}

