using UnityEngine;

namespace GGJ
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ScrollEnabler : MonoBehaviour
    {
        [SerializeField]
        EnableState enableState;

        enum EnableState
        {
            Enable,
            Disable
        }

        CameraFollow cameraFollow;

        void Awake()
        {
            Initialize();
        }

        void Initialize()
        {
            cameraFollow = Camera.main.GetComponent<CameraFollow>();
            GetComponent<BoxCollider2D>().isTrigger = true;
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player")) {
                cameraFollow.MakeScroll(enableState == EnableState.Enable ? true : false);
            }
        }
    }
}
