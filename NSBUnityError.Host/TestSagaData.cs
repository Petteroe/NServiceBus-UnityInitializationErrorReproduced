using NServiceBus.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSBUnityError.Host
{
	public class TestSagaData : ContainSagaData
	{
		[Unique]
		public string SomeKey { get; set; }
	}
}
