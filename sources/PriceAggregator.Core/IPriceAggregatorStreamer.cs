namespace PriceAggregator.Core
{
    public interface IPriceAggregatorStreamer
    {
        void Subscribe(PriceHandler callback);
        void Start();
    }
}