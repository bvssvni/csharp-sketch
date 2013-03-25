using System;

namespace GenericInterface.Algebra
{
	public interface IDot<Argument, Return>
	{
		Return Dot(Argument b);
	}
}

