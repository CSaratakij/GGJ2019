using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GGJ
{
    public class OffsetReseter : MonoBehaviour
    {
        CameraFollow cameraFollow;


#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Handles.Label(transform.position, "Camera Offset Resetter");
        }
#endif
        void Awake()
        {
            cameraFollow = Camera.main.GetComponent<CameraFollow>();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (cameraFollow == null)
                return;

            if (other.gameObject.CompareTag("Player")) {
                cameraFollow.ResetOffset(true);
            }
        }
    }
}
