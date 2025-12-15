using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace ConcurrentCollections.SellShirts
{
	public static class TShirtProvider
	{
		public static ImmutableArray<TShirt> AllShirts { get; } = ImmutableArray.Create(
			new TShirt("igeek", "IGeek", 500),
			new TShirt("bigdata", "Big Data", 600),
			new TShirt("ilovenode", "I Love Node", 750),
			new TShirt("kcdc", "kcdc", 400),
			new TShirt("docker", "Docker", 350),
			new TShirt("qcon", "QCon", 300),
			new TShirt("ps", "Pluralsight", 60000),
			new TShirt("pslive", "Pluralsight Live", 60000)
		);
	}
}
