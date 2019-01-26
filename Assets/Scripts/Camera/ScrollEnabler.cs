using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GGJ
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ScrollEnabler : MonoBehaviour
    {
        [SerializeField]
        bool isDisableOnContact;

        [SerializeField]
        EnableState enableState;

        [SerializeField]
        Transform focusTarget;

        enum EnableState
        {
            Enable,
            Disable
        }

        bool isEnter;
        CameraFollow cameraFollow;

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (enableState == EnableState.Disable) {
                if (focusTarget == null) {
                    Handles.Label(transform.position, "No focus target..");
                }
                else {
                    Handles.color = Color.red;
                    Handles.DrawDottedLine(transform.position, focusTarget.position, 0.2f);
                }
            }
        }
#endif

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

        void LateUpdate()
        {
            if (enableState == EnableState.Disable && focusTarget != null && isEnter) {
                Vector3 targetPos = Vector3.Lerp(Camera.main.transform.position, focusTarget.position, 0.04f);
                targetPos.x = 0.0f;
                targetPos.z = -10.0f;
                Camera.main.transform.position = targetPos;
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player")) {
                cameraFollow.MakeScroll(enableState == EnableState.Enable ? true : false);
                isEnter = true;
                gameObject.SetActive(!isDisableOnContact);
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player")) {
                isEnter = false;
            }
        }
    }
}
