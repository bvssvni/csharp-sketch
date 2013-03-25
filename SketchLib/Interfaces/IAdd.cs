using System;

namespace Sketch
{
	public interface IAdd<Argument, Return>
	{
		Return Add(Argument b);
	}
}

