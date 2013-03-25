using System;

namespace GenericInterface.Algebra
{
	public interface ICross<Argument, Return>
	{
		Return Cross(Argument b);
	}
}

