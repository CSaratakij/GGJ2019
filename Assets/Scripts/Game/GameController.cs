using UnityEngine;

namespace GGJ
{
    public class GameController : MonoBehaviour
    {
        public static bool IsGameStart { get; private set; }
        public delegate void _Func();

        public static event _Func OnGameStart;
        public static event _Func OnGameStop;


        public void GameStart()
        {
            if (IsGameStart)
                return;

            IsGameStart = true;

            if (OnGameStart != null)
                OnGameStart();
        }

        public void GameStop()
        {
            if (!IsGameStart)
                return;

            IsGameStart = false;

            if (OnGameStop != null)
                OnGameStop();
        }
    }
}
