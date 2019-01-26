using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GGJ
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class StatStateChanger : MonoBehaviour
    {
        [SerializeField]
        StatManipulator.ManipulateState state;

        [SerializeField]
        StatManipulator stateManipulator;

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            string text = "";

            switch (state)
            {
                case StatManipulator.ManipulateState.None:
                    text = "State : None";
                    break;

                case StatManipulator.ManipulateState.DecreaseOverTime:
                    text = "State : DecreaseOverTime";
                    break;

                case StatManipulator.ManipulateState.IncreaseOverTime:
                    text = "State : IncreaseOverTime";
                    break;

                default:
                    break;
            }

            Handles.Label(transform.position, text);
        }
#endif

        void Awake()
        {
            Initialize();
        }

        void Initialize()
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player")) {
                stateManipulator.SetManipulate(state);
                gameObject.SetActive(false);
            }
        }
    }
}
