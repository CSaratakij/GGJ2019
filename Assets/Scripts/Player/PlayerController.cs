using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GGJ
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        float gravity;

        [SerializeField]
        float moveForce;

        [SerializeField]
        float floatForce;

        [SerializeField]
        float maxFloatSpeed;

        [SerializeField]
        Transform ground;

        [SerializeField]
        Vector2 groundSize;

        [SerializeField]
        LayerMask groundMask;

        [SerializeField]
        LayerMask obstacleMask;

        enum MovementState
        {
            Float,
            Ground
        }

        MovementState movementState;

        Vector2 inputVector;
        Vector2 velocity;

        Rigidbody2D rigid;
        Collider2D groundCollider;

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (ground == null)
                return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(ground.position, groundSize);
        }
#endif

        void Awake()
        {
            Initialize();
        }

        void Update()
        {
            InputHandler();
        }

        void FixedUpdate()
        {
            MovementHandler();
            GroundCheckHandler();
            ObstacleCheckHandler();
        }

        void Initialize()
        {
            rigid = GetComponent<Rigidbody2D>();
        }

        void InputHandler()
        {
            inputVector.x = Input.GetAxisRaw("Horizontal");
            inputVector.y = Input.GetAxisRaw("Vertical");
        }

        void MovementHandler()
        {
            switch (movementState)
            {
                case MovementState.Ground:
                    MovementOnGround();
                    break;

                case MovementState.Float:
                    MovementFloating();
                    break;

                default:
                    break;
            }
        }

        void GroundCheckHandler()
        {
            groundCollider = Physics2D.OverlapBox(ground.position, groundSize, 0.0f, groundMask);
            movementState = (groundCollider == null) ? MovementState.Float : MovementState.Ground;
        }

        void ObstacleCheckHandler()
        {

        }

        void MovementOnGround()
        {
            velocity.x = inputVector.x * moveForce;
            velocity.y = -gravity;

            rigid.velocity = velocity * Time.fixedDeltaTime;
        }

        void MovementFloating()
        {
            velocity.y = -gravity * Time.fixedDeltaTime;

            velocity.x = inputVector.x * floatForce * Time.fixedDeltaTime;
            velocity.x = Mathf.Clamp(velocity.x, -maxFloatSpeed, maxFloatSpeed);

            rigid.AddForce(velocity, ForceMode2D.Force);
        }
    }
}
