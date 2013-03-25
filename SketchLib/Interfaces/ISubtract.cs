using System;

namespace Sketch
{
	public interface ISubtract<Argument, Return>
	{
		Return Subtract(Argument b);
	}
}

