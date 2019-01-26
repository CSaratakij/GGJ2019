using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ
{
    [RequireComponent(typeof(Stat))]
    public class StatManipulator : MonoBehaviour
    {
        [SerializeField]
        ManipulateState manipulalteState;

        [SerializeField]
        float decreaseRate;

        [SerializeField]
        float decreaseValue;

        [SerializeField]
        float increaseRate;

        [SerializeField]
        float increaseValue;

        public enum ManipulateState
        {
            None,
            DecreaseOverTime,
            IncreaseOverTime
        }

        IEnumerator manipulateCallback;

        WaitForSeconds decreaseWait;
        WaitForSeconds increaseWait;

        bool isStart;
        Stat stat;

        void Awake()
        {
            Initialize();
        }

        void Initialize()
        {
            stat = GetComponent<Stat>();

            if (stat == null)
                Debug.Log("Can't find a stat");

            manipulateCallback = Manipulate_Callback();

            decreaseWait = new WaitForSeconds(decreaseRate);
            increaseWait = new WaitForSeconds(increaseRate);
        }

        void ManipulateStateHandler()
        {
            switch (manipulalteState)
            {
                case ManipulateState.None:
                    break;

                case ManipulateState.DecreaseOverTime:
                    stat.Remove(decreaseValue);
                    break;

                case ManipulateState.IncreaseOverTime:
                    stat.Restore(increaseValue);
                    break;

                default:
                    break;
            }
        }

        IEnumerator Manipulate_Callback()
        {
            while (true)
            {
                ManipulateStateHandler();
                switch (manipulalteState)
                {
                    case ManipulateState.DecreaseOverTime:
                        yield return decreaseWait;
                        break;

                    case ManipulateState.IncreaseOverTime:
                        yield return increaseWait;
                        break;

                    default:
                        yield return null;
                        break;
                }
            }
        }

        public void StartManipulate()
        {
            if (isStart)
                return;

            isStart = true;
            StartCoroutine(manipulateCallback);
        }

        public void Stop()
        {
            if (!isStart)
                return;

            isStart = false;
            StopCoroutine(manipulateCallback);
        }

        public void SetManipulate(ManipulateState state)
        {
            if (state != manipulalteState)
                manipulalteState = state;
        }
    }
}
