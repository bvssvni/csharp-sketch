using System;

namespace GenericInterface.Algebra
{
	public interface IAdd<Argument, Return>
	{
		Return Add(Argument b);
	}
}

