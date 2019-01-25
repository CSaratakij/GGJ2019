using UnityEngine;

namespace GGJ
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        bool isScroll;

        [SerializeField]
        ScrollDirection scrollDirection;

        [SerializeField]
        float scrollSpeed;

        [SerializeField]
        Transform target;

        [SerializeField]
        float damp;

        [SerializeField]
        Vector3 offset;

        Vector3 targetPosition;
        Vector3 previousOffset;

        enum ScrollDirection
        {
            Up,
            Down
        }

        void Awake()
        {
            Initialize();
        }

        void LateUpdate()
        {
            MovementHandler();
        }

        void Initialize()
        {
            if (target == null) {
                Debug.Log("Can't find a follow target.");
                return;
            }
        }

        void MovementHandler()
        {
            if (!GameController.IsGameStart)
                return;

            if (isScroll)
                ScrollHandler();
            else
                FollowTarget();
        }

        void FollowTarget()
        {
            targetPosition = Vector3.Lerp(transform.position, target.position + offset, damp);
            targetPosition.x = 0.0f;
            targetPosition.z = -10.0f;
            transform.position = targetPosition;
        }

        void ScrollHandler()
        {
            switch (scrollDirection)
            {
                case ScrollDirection.Up:
                    transform.position += Vector3.up * scrollSpeed * Time.deltaTime;
                    break;

                case ScrollDirection.Down:
                    transform.position += Vector3.down * scrollSpeed * Time.deltaTime;
                    break;
            }
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
