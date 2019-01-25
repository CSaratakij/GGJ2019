using UnityEngine;

namespace GGJ
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        bool isScrollUp;

        [SerializeField]
        float scrollUpSpeed;

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
            if (isScrollUp) {
                ScrollUp();
            }
            else {
                FollowTarget();
            }
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
            targetPosition.x = 0.0f;
            targetPosition.z = -10.0f;
            transform.position = targetPosition;
        }

        void ScrollUp()
        {
            transform.position += Vector3.up * scrollUpSpeed * Time.deltaTime;
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
