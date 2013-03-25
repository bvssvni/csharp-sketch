using System;

namespace GenericInterface.Algebra
{
	public interface ISubtract<Argument, Return>
	{
		Return Subtract(Argument b);
	}
}

