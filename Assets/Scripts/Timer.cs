using System;
using UniRx;

namespace Assets.Scripts
{
    public class Timer
    {
        public void Countdown(float duration, Action onCompleted)
        {
            var observer = Observer.Create<long>((time) => { }, onCompleted);

            var observable = Observable.Timer(
                TimeSpan.FromSeconds(duration),
                Scheduler.MainThreadEndOfFrame);

            observable.Subscribe(observer);
        }
    }
}
