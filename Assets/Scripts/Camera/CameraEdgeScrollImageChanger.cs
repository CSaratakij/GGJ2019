using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ
{
    public class CameraEdgeScrollImageChanger : MonoBehaviour
    {
        [SerializeField]
        int imageID;

        [SerializeField]
        CameraEdgeScroll edgeScroll;


        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player")) {
                edgeScroll.ChangeImage(imageID);
                gameObject.SetActive(false);
            }
        }
    }
}
