using NSBUnityError.Commands;
using NServiceBus.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSBUnityError.Host
{
	public class TestSaga : Saga<TestSagaData>,
		IAmStartedByMessages<TestCommand>
	{
		public void Handle(TestCommand message)
		{
			Console.WriteLine("Received TestCommand");
		}

		protected override void ConfigureHowToFindSaga(SagaPropertyMapper<TestSagaData> mapper)
		{
			mapper.ConfigureMapping<TestCommand>(m => m.SomeKey).ToSaga(s => s.SomeKey);
		}
	}
}
