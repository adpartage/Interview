using NSubstitute;
using NUnit.Framework;
using PriceAggregator.Core;
using System;

namespace PriceAggregator.Tests
{
    [TestFixture]
    public class PriceAggregatorStreamerTest
    {
        private IPriceAggregator _priceAggregator;
        private IScheduler _pulseService;

        [SetUp]
        public void Setup()
        {
            _priceAggregator = Substitute.For<IPriceAggregator>();
            _pulseService = Substitute.For<IScheduler>();
        }

        private IPriceAggregatorStreamer GetTarget()
        {
            return new PriceAggregatorStreamer(_priceAggregator, _pulseService);
        }

        [Test]
        public void Start_WhenInvoked_ThenShouldSubscribeToPulseService()
        {
            var streamer = GetTarget();

            streamer.Start();

            _pulseService.Received(1).Schedule(Arg.Any<Action>());
        }

        [Test]
        public void OnPulse_WhenInvoked_ThenCallPulseService()
        {
            _priceAggregator.Aggregate().Returns(2);
            _pulseService.When(i => i.Schedule(Arg.Any<Action>())).Do(a => a.Arg<Action>()?.Invoke());
            decimal? callbackInvokedWith = null;

            var streamer = GetTarget();
            PriceHandler handler = v => callbackInvokedWith = v;
            streamer.Subscribe(handler);
            streamer.Start();

            Assert.That(callbackInvokedWith, Is.EqualTo(2));
        }
    }
}
