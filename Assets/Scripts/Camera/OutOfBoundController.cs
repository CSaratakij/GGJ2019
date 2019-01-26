using UnityEngine;

namespace GGJ
{
    public class OutOfBoundController : MonoBehaviour
    {
        [SerializeField]
        Transform target;

        [SerializeField]
        Vector3 offset;

        [SerializeField]
        Stat stat;

        [SerializeField]
        Vector3 viewportBound;

        Vector3 screenBottomWorldPos;
        Vector3 targetLimitPoint;

        void LateUpdate()
        {
            OutOfBoundHandler();
        }

        void OutOfBoundHandler()
        {
            screenBottomWorldPos = Camera.main.ViewportToWorldPoint(viewportBound);
            targetLimitPoint = target.position + offset;

            if (screenBottomWorldPos.y > targetLimitPoint.y)
                stat.Clear();
        }
    }
}
