using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GGJ
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField]
        float moveSpeed;

        [SerializeField]
        MoveState moveState;

        [SerializeField]
        Transform firstPivot;

        [SerializeField]
        Transform secondPivot;

        enum MoveState
        {
            None,
            Horizontal,
            Vertical
        }

        bool isTowardFirst = true;

        Vector3 moveDirection;
        Vector3 velocity;

        Vector3 cacheFirstPivot;
        Vector3 cacheSecondPivot;

        MoveState cacheMoveState;

        Vector3 newPos;
        Timer changeStateDelay;

        Rigidbody2D rigid;

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (firstPivot == null || secondPivot == null)
                return;

            Handles.color = Color.red;

            Handles.DrawDottedLine(firstPivot.position, transform.position, 0.2f);
            Handles.DrawDottedLine(secondPivot.position, transform.position, 0.2f);

            Handles.Label(firstPivot.position + new Vector3(-0.5f, 0, 0), "First");
            Handles.Label(secondPivot.position + new Vector3(-0.5f, 0, 0), "Second");
        }
#endif

        void Awake()
        {
            Initialize();
            SubscribeEvent();
        }

        void Initialize()
        {
            rigid = GetComponent<Rigidbody2D>();
            changeStateDelay = GetComponent<Timer>();

            cacheFirstPivot = firstPivot.position;
            cacheSecondPivot = secondPivot.position;
            cacheMoveState = moveState;
        }

        void SubscribeEvent()
        {
            changeStateDelay.OnTimerStop += changeStateDelay_OnStop;
        }

        void UnsubsribeEvent()
        {
            changeStateDelay.OnTimerStop -= changeStateDelay_OnStop;
        }

        void OnDestroy()
        {
            UnsubsribeEvent();
        }

        void FixedUpdate()
        {
            MoveHandler();
        }

        void MoveHandler()
        {
            switch (moveState)
            {
                case MoveState.Horizontal:
                    MoveHorizontal();
                    break;

                case MoveState.Vertical:
                    MoveVertical();
                    break;
            }
        }

        void MoveHorizontal()
        {
            if (isTowardFirst) {
                moveDirection = Vector3.left;

                if (transform.position.x > cacheFirstPivot.x) {
                    velocity = (moveSpeed * moveDirection) * Time.fixedDeltaTime;
                    Move(velocity);
                }
                else
                {
                    isTowardFirst = false;
                    moveState = MoveState.None;

                    changeStateDelay.Countdown();
                }
            }
            else
            {
                moveDirection = Vector3.right;

                if (transform.position.x < cacheSecondPivot.x) {
                    velocity = (moveSpeed * moveDirection) * Time.fixedDeltaTime;
                    Move(velocity);
                }
                else
                {
                    isTowardFirst = true;
                    moveState = MoveState.None;

                    changeStateDelay.Countdown();
                }
            }
        }

        void MoveVertical()
        {
            if (isTowardFirst) {
                moveDirection = Vector3.up;

                if (transform.position.y < cacheFirstPivot.y) {
                    velocity = (moveSpeed * moveDirection) * Time.fixedDeltaTime;
                    Move(velocity);
                }
                else
                {
                    isTowardFirst = false;
                    moveState = MoveState.None;

                    changeStateDelay.Countdown();
                }
            }
            else
            {
                moveDirection = Vector3.down;

                if (transform.position.y > cacheSecondPivot.y) {
                    velocity = (moveSpeed * moveDirection) * Time.fixedDeltaTime;
                    Move(velocity);
                }
                else
                {
                    isTowardFirst = true;
                    moveState = MoveState.None;

                    changeStateDelay.Countdown();
                }
            }
        }

        void Move(Vector3 velocity)
        {
            newPos = transform.position + velocity;
            rigid.MovePosition(newPos);
        }

        void changeStateDelay_OnStop()
        {
            moveState = cacheMoveState;
        }
    }
}
