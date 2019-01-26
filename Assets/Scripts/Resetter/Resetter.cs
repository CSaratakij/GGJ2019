using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GGJ
{
    public class Resetter : MonoBehaviour
    {
        public static Resetter instance = null;

        [SerializeField]
        Transform player;

        [SerializeField]
        Transform[] checkPoints;

        //Hacks
        [SerializeField]
        FadeController fadeController;

        //Hacks
        [SerializeField]
        Timer timer;


        public delegate void _Func();
        public event _Func OnReset;

        PlayerController playerController;
        GameObject[] resetables;

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Handles.color = Color.green;

            foreach (Transform trans in checkPoints)
            {
                if (trans == null)
                    continue;

                Handles.DrawDottedLine(trans.position, transform.position, 0.2f);
            }
        }
#endif

        void Awake()
        {
            Initialize();
            MakeSingleton();

            timer.OnTimerStop += OnTimerStop;
        }

        void OnDestroy()
        {
            timer.OnTimerStop -= OnTimerStop;
        }

        void Initialize()
        {
            playerController = player.GetComponent<PlayerController>();
            resetables = GameObject.FindGameObjectsWithTag("Resetable");
        }

        void MakeSingleton()
        {
            if (instance == null) {
                instance = this;
            }
            else {
                Destroy(this.gameObject);
            }
        }

        public void Reset()
        {
            Camera.main.GetComponent<CameraFollow>().MakeScroll(false);
            fadeController.FadeIn();

            foreach (GameObject obj in resetables)
            {
                obj.SetActive(true);
            }

            OnReset?.Invoke();
            timer.Countdown();
        }

        void OnTimerStop()
        {
            playerController.Reset();
            player.transform.position = checkPoints[PlayerController.CheckpointID].position;

            Camera.main.transform.position = new Vector3(0.0f, player.transform.position.y, -10.0f);

            fadeController.FadeOut();
        }
    }
}
