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
        GameObject[] resetableItemGood;
        GameObject[] resetableItemBad;

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
            resetableItemGood = GameObject.FindGameObjectsWithTag("GoodItem");
            resetableItemBad = GameObject.FindGameObjectsWithTag("BadItem");
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
            timer.Countdown();
        }

        void OnTimerStop()
        {
            foreach (GameObject obj in resetables)
            {
                obj.SetActive(true);
            }

            foreach (GameObject obj in resetableItemBad)
            {
                obj.SetActive(true);
            }

            foreach (GameObject obj in resetableItemGood)
            {
                obj.SetActive(true);
            }

            OnReset?.Invoke();

            playerController.Reset();
            player.transform.position = checkPoints[PlayerController.CheckpointID].position;

            Camera.main.transform.position = new Vector3(0.0f, player.transform.position.y, -10.0f);
            fadeController.FadeOut();
        }
    }
}
