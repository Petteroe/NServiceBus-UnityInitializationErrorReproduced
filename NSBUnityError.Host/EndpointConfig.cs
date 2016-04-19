using Microsoft.Practices.Unity;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSBUnityError.Host
{
	public class EndpointConfig : IConfigureThisEndpoint, AsA_Worker
	{
		public void Customize(BusConfiguration configuration)
		{
			configuration.UseContainer<UnityBuilder>(c => c.UseExistingContainer(getUnityContainer()));

			configuration.UseSerialization<JsonSerializer>();

			configuration.ScaleOut().UseSingleBrokerQueue();

			configuration.UseTransport<AzureServiceBusTransport>();
			configuration.UsePersistence<AzureStoragePersistence>();

		}

		private IUnityContainer getUnityContainer()
		{
			var container = new UnityContainer();

			// Register our own components here

			return container;
		}

	}
}
