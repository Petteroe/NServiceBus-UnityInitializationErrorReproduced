using Microsoft.Practices.Unity;
using NServiceBus;

namespace NSBUnityError.Host
{
	using System;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Worker
	{
		public void Customize(BusConfiguration configuration)
		{
			configuration.UseContainer<UnityBuilder>(c => c.UseExistingContainer(getUnityContainer()));

			configuration.UseSerialization<JsonSerializer>();

			configuration.ScaleOut().UseSingleBrokerQueue();

			configuration.UseTransport<AzureServiceBusTransport>().ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString"));
			configuration.UsePersistence<AzureStoragePersistence>();

		}

		private IUnityContainer getUnityContainer()
		{
			var container = new UnityContainer();

			// Register our own components here

//			container.RegisterType<ISagaPersister, AzureSagaPersister>(new InjectionConstructor(CloudStorageAccount.DevelopmentStorageAccount, true));

			return container;
		}

	}
}
