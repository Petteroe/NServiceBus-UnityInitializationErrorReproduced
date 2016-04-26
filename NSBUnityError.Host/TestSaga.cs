using NSBUnityError.Commands;
using NServiceBus.Saga;
using System;

namespace NSBUnityError.Host
{
	public class TestSaga : Saga<TestSagaData>,
		IAmStartedByMessages<TestCommand>
	{
		public void Handle(TestCommand message)
		{
			Console.WriteLine("Received TestCommand");
			MarkAsComplete();
		}

		protected override void ConfigureHowToFindSaga(SagaPropertyMapper<TestSagaData> mapper)
		{
			mapper.ConfigureMapping<TestCommand>(m => m.SomeKey).ToSaga(s => s.SomeKey);
		}
	}
}
