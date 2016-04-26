using NServiceBus;

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
