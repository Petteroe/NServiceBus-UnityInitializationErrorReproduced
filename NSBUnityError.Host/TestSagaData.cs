using NServiceBus.Saga;

namespace NSBUnityError.Host
{
	public class TestSagaData : ContainSagaData
	{
		[Unique]
		public string SomeKey { get; set; }
	}
}
