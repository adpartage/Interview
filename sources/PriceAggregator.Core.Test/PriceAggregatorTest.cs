using NSubstitute;
using NUnit.Framework;
using PriceAggregator.Core;
using System.Collections.Generic;

namespace PriceAggregator.Tests
{
    [TestFixture]
    public class PriceAggregatorTest
    {
        private IEnumerable<IPriceFeederFacade> _feeders;

        private IPriceAggregator GetTarget()
        {
            return new Core.PriceAggregator(_feeders);
        }

        [Test]
        public void Aggregate_WhenNoFeeder_THenShouldReturNull()
        {
            _feeders = new List<IPriceFeederFacade>();
            IPriceAggregator aggregator = GetTarget();

            var aggregatedValue = aggregator.Aggregate();

            Assert.That(aggregatedValue, Is.Null);
        }

        [Test]
        public void Aggregate_WhenTwoFeeders_THenShouldSumLastPrices()
        {
            var feeder1 = Substitute.For<IPriceFeederFacade>();
            feeder1.GetLastPrice().Returns(1);
            var feeder2 = Substitute.For<IPriceFeederFacade>();
            feeder2.GetLastPrice().Returns(2);

            _feeders = new[] { feeder1, feeder2 };
            IPriceAggregator aggregator = GetTarget();

            var aggregatedValue = aggregator.Aggregate();

            Assert.That(aggregatedValue, Is.EqualTo(3));
        }

    }
}
