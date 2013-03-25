using System;

namespace Sketch
{
	public interface IDot<Argument, Return>
	{
		Return Dot(Argument b);
	}
}

