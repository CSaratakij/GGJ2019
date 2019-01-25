using UnityEngine;

namespace GGJ
{
    public class Timer : MonoBehaviour
    {
        [SerializeField]
        float current;

        [SerializeField]
        float maximum;

        public delegate void _Func();

        public event _Func OnTimerStart;
        public event _Func OnTimerStop;

        public bool IsStart { get; private set; }
        public bool IsPause { get; private set; }

        public float Current => current;
        public float Maximum => maximum;

        void OnDestroy()
        {
            CleanUp();
        }

        void Update()
        {
            TickHandler();
        }

        void TickHandler()
        {
            if (!IsStart)
                return;

            if (IsPause)
                return;

            current -= 1.0f * Time.deltaTime;

            if (current <= 0.0f)
                Stop();
        }

        void CleanUp()
        {
            OnTimerStart = null;
            OnTimerStop = null;
        }

        public void Countdown()
        {
            if (IsStart)
                return;

            IsStart = true;
            OnTimerStart?.Invoke();
        }

        public void Stop()
        {
            if (!IsStart)
                return;

            IsStart = false;
            OnTimerStop?.Invoke();
        }

        public void Pause(bool value)
        {
            IsPause = value;
        }

        public void Reset()
        {
            IsStart = false;
            current = maximum;
        }
    }
}
