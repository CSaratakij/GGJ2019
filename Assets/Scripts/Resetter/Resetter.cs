using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ
{
    public class Resetter : MonoBehaviour
    {
        public static Resetter instance = null;

        [SerializeField]
        Transform player;

        [SerializeField]
        Transform[] checkPoints;

        public delegate void _Func();
        public event _Func OnReset;

        PlayerController playerController;
        GameObject[] resetables;


        void Awake()
        {
            Initialize();
            MakeSingleton();
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
            //make ui bind this event, make ui produce fade.. 
            //then
            foreach (GameObject obj in resetables)
            {
                obj.SetActive(true);
            }

            playerController.Reset();
            player.transform.position = checkPoints[PlayerController.CheckpointID].position;

            Camera.main.transform.position = new Vector3(0.0f, player.transform.position.y, -10.0f);
            Camera.main.GetComponent<CameraFollow>().MakeScroll(false);

            OnReset?.Invoke();
        }
    }
}
