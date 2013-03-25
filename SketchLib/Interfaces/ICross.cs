using System;

namespace Sketch
{
	public interface ICross<Argument, Return>
	{
		Return Cross(Argument b);
	}
}

