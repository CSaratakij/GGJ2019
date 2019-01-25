using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        Transform target;

        [SerializeField]
        float damp;

        [SerializeField]
        Vector3 offset;

        Vector3 targetPosition;
        Vector3 previousOffset;


        void Awake()
        {
            Initialize();
        }

        void LateUpdate()
        {
            FollowTarget();
        }

        void Initialize()
        {
            if (target == null) {
                Debug.Log("Can't find a follow target.");
                return;
            }
        }

        void FollowTarget()
        {
            targetPosition = Vector3.Lerp(transform.position, target.position + offset, damp);
            targetPosition.z = -10.0f;
            transform.position = targetPosition;
        }

        public void ResetOffset(bool value)
        {
            if (value) {
                previousOffset = offset;
                offset = Vector3.zero;
            }
            else {
                offset = previousOffset;
            }
        }
    }
}
