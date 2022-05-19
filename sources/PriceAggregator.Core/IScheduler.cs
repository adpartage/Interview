using System;
using System.Timers;

namespace PriceAggregator.Core
{
    public interface IScheduler
    {
        void Schedule(Action toDo);
        void UnSchedule(Action toUnSchedule);
    }

    public class Scheduler : IScheduler
    {
        Timer _timer;

        public Scheduler()
        {
            _timer = new Timer();
        }

        public void Schedule(Action toDo)
        {
            _timer.Elapsed += (e, a) => toDo?.Invoke();
        }

        public void UnSchedule(Action toUnSchedule)
        {
            throw new NotImplementedException();
        }
    }
}