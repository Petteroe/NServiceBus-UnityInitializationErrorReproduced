using NSBUnityError.Commands;
using NServiceBus;
using System;

namespace NSBUnityError.Sender
{
	class Program
	{
		static IBus _bus;

		static void Main(string[] args)
		{
			_bus = EndpointConfig.GetBus(EndpointConfig.GetConfiguration("someConnectionString", "someOtherConnectionString", Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString")));

			Console.WriteLine("Bus started.");

			run();
		}

		static void run()
		{
			Console.WriteLine("Press any key to send command.");
			Console.ReadKey();

			_bus.Send(new TestCommand());

			Console.WriteLine("Command sent.");
			Console.WriteLine();

			run();
		}
	}
}
