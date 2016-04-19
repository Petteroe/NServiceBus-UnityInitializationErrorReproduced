using NSBUnityError.Commands;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSBUnityError.Sender
{
	class Program
	{
		static IBus _bus;

		static void Main(string[] args)
		{
			_bus = EndpointConfig.GetBus(EndpointConfig.GetConfiguration("someConnectionString", "someOtherConnectionString", ConfigurationManager.ConnectionStrings["ServiceBusConnectionString"].ConnectionString));

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
