using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSBUnityError.Host
{
	public class MessageConventions : INeedInitialization
	{
		public void Customize(BusConfiguration config)
		{
			config.Conventions()
				.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("NSBUnityError.Commands"));
		}
	}
}
