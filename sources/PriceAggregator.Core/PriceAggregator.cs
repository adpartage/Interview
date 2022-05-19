using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace PriceAggregator.Core
{

    public class PriceAggregator : IPriceAggregator
    {
        IEnumerable<IPriceFeederFacade> _priceFeeders;

        public PriceAggregator(IEnumerable<IPriceFeederFacade> feeders)
        {
            _priceFeeders = feeders;
        }

        public decimal? Aggregate()
        {
            return _priceFeeders != null && _priceFeeders.Any()
                ? _priceFeeders.Sum(f => f.GetLastPrice())
                : (decimal?)null;
        }
    }

    
    public delegate void PriceHandler(decimal? price);
    public class PriceAggregatorStreamer : IPriceAggregatorStreamer
    {
        private event PriceHandler _priceHadler;
        private readonly IPriceAggregator _priceAggregator;
        private readonly IScheduler _pulseService;
        private volatile bool _started;

        public PriceAggregatorStreamer(IPriceAggregator priceAggregator, IScheduler pulseService)
        {
            _priceAggregator = priceAggregator;
            _pulseService = pulseService;
        }

        public void Start()
        {
            if (_started) return;
            _started = true;

            _pulseService?.Schedule(OnPulse);
        }

        private void OnPulse()
        {
            var aggregatedValue = _priceAggregator.Aggregate();
            _priceHadler?.Invoke(aggregatedValue);
        }

        public void Subscribe(PriceHandler callback)
        {
            _priceHadler += callback;
        }

    }

}
