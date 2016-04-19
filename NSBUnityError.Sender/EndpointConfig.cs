using Microsoft.Practices.Unity;
using NSBUnityError.Commands;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Features;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NSBUnityError.Sender
{
	public class EndpointConfig
	{
		private static BusConfiguration _config;

		public static BusConfiguration GetConfiguration(string someConnectionString, string someOtherConnectionString, string serviceBusConnectionString)
		{
			if (_config == null)
			{
				var config = new BusConfiguration();

				config.UseContainer<UnityBuilder>(c => c.UseExistingContainer(getContainer(config, someConnectionString, someOtherConnectionString)));
				config.CustomConfigurationSource(new MyConfigSource(serviceBusConnectionString));

				config.AssembliesToScan(getAssembliesToScan());

				config.EndpointName("NSBUnityError.Sender");

				config.DisableFeature<TimeoutManager>();

				config.UseSerialization<JsonSerializer>();
				config.UseTransport<AzureServiceBusTransport>();
				config.UsePersistence<AzureStoragePersistence>();

				config.ScaleOut().UseSingleBrokerQueue();

				config.Conventions()
					.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("NSBUnityError.Commands"));

				if (Debugger.IsAttached)
					config.EnableInstallers();

				_config = config;
			}

			return _config;
		}

		private static IUnityContainer getContainer(BusConfiguration config, string someConnectionString, string someOtherConnectionString)
		{
			var container = new UnityContainer();

			// Register some of our own components here

			return container;
		}

		public static IBus GetBus(BusConfiguration config)
		{
			var startableBus = Bus.Create(config);
			var startedBus = startableBus.Start();
			return startedBus;
		}

		private static IReadOnlyList<Assembly> getAssembliesToScan()
		{
			var assemblies = new List<Assembly>
			{
				typeof(TestCommand).Assembly
			};

			assemblies.Add(Assembly.Load("NServiceBus.Azure.Transports.WindowsAzureServiceBus").GetTypes().Last().Assembly);

			return assemblies;
		}

		private class MyConfigSource : IConfigurationSource
		{
			private string _serviceBusConnectionString;

			public MyConfigSource(string serviceBusConnectionString)
			{
				_serviceBusConnectionString = serviceBusConnectionString;
			}

			public T GetConfiguration<T>() where T : class, new()
			{
				if (typeof(T) == typeof(UnicastBusConfig))
				{
					Debug.WriteLine("Getting UnicastBusConfig");
					var config = new UnicastBusConfig();
					config.MessageEndpointMappings.Add(new MessageEndpointMapping
					{
						AssemblyName = "NSBUnityError.Commands",
						Namespace = "NSBUnityError.Commands",
						Endpoint = "NSBUnityError.Host"
					});
					return config as T;
				}

				if (typeof(T) == typeof(TransportConfig))
				{
					Debug.WriteLine("Getting TransportConfig");
					var config = new TransportConfig
					{
						MaximumConcurrencyLevel = 1,
						MaxRetries = 3
					};
					return config as T;
				}

				if (typeof(T) == typeof(MessageForwardingInCaseOfFaultConfig))
				{
					Debug.WriteLine("Getting MessageForwardingInCaseOfFaultConfig");
					var config = new MessageForwardingInCaseOfFaultConfig
					{
						ErrorQueue = "error"
					};
					return config as T;
				}

				if (typeof(T) == typeof(SecondLevelRetriesConfig))
				{
					Debug.WriteLine("Getting SecondLevelRetriesConfig");
					var config = new SecondLevelRetriesConfig
					{
						Enabled = true,
						NumberOfRetries = 3,
						TimeIncrease = TimeSpan.FromSeconds(10)
					};
					return config as T;
				}

				if (typeof(T) == typeof(AuditConfig))
				{
					Debug.WriteLine("Getting AuditConfig");
					var config = new AuditConfig
					{
						QueueName = "audit"
					};
					return config as T;
				}

				if (typeof(T) == typeof(AzureServiceBusQueueConfig))
				{
					Debug.WriteLine("Getting AzureServiceBusQueueConfig");
					var config = new AzureServiceBusQueueConfig
					{
						ConnectionString = _serviceBusConnectionString
					};
					return config as T;
				}

				return null;
			}
		}

	}
}
