using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GGJ
{
    public class PlayerController : MonoBehaviour
    {
        public static int CheckpointID = 0;

        [SerializeField]
        float moveForce;

        [SerializeField]
        float jumpForce;

        [SerializeField]
        float maxJumpVelocity;

        [SerializeField]
        float onGroundGravity;

        [SerializeField]
        float gravity;

        [SerializeField]
        float verticalTerminalVelocity;

        [SerializeField]
        Transform head;

        [SerializeField]
        Transform ground;

        [SerializeField]
        LayerMask groundLayer;

        [SerializeField]
        LayerMask headButtLayer;

        [SerializeField]
        LayerMask oneWayCollisionLayer;

        [SerializeField]
        Vector3 offset;

        [SerializeField]
        Vector2 size;

        [SerializeField]
        LayerMask enemyLayer;

        [SerializeField]
        AudioClip jumpAudio;

        [SerializeField]
        AudioClip landingAudio;

        int totalJump;
        int totalCoin;

        float newPitch;
        float currentJumpVelocity;

        bool isGrounded;
        bool isFalling;

        bool currentGroundState;
        bool previousGroundState;

        bool currentOneWayCollisionState;
        bool previousOneWayCollsionState;

        bool isPressJump;
        bool isJumped;
        bool isJumpKeyDown;
        bool isJumpKeyUp;

        bool currentPressJumpState;
        bool previousPressJumpState;

        bool isFacingRight = true;
        bool allowPlayJumpAudio;
        bool isInvinsible = false;

        bool isDead;
        bool isBeginOneWayCollision;

        Animator anim;
        AudioSource audio;

        Vector2 velocity;
        Vector2 inputVector;
        Vector2 groundRaycastDirection;
        Vector2 boxCastHeadSize;
        Vector2 boxCastBodySize;

        Vector3 newScale;
        Rigidbody2D rigid;

        RaycastHit2D raycastHeadHit;
        RaycastHit2D raycastGroundHit;
        RaycastHit2D raycastFallingCheck;
        RaycastHit2D raycastOneWayCollision;

        Stat stat;
        StatManipulator statManipulator;

        SpriteRenderer spriteRenderer;

        Color flickeringColor;
        WaitForSeconds flickeringWait;


#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + offset, size);
        }
#endif

        void Awake()
        {
            Initialize();
            SubscribeEvent();
        }

        void OnDestroy()
        {
            UnsubscribeEvent();
        }

        void Update()
        {
            InputHandler();
            AnimationHandler();
            FlipHandler();
        }

        void FixedUpdate()
        {
            CheckHitEnemy();
            MovementHandler();
        }

        void Initialize()
        {
            anim = GetComponent<Animator>();
            audio = GetComponent<AudioSource>();
            rigid = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            stat = GetComponent<Stat>();
            statManipulator = GetComponent<StatManipulator>();
            groundRaycastDirection = new Vector3(-1.0f, -1.0f);
            //boxCastHeadSize = new Vector2(0.855f, 1.0f);
            boxCastHeadSize = new Vector2(0.455f, 0.5f);
            //boxCastBodySize = new Vector2(0.855f, 0.4f);
            boxCastBodySize = new Vector2(0.490f, 0.4f);
            flickeringColor = new Color(1.0f, 1.0f, 1.0f, 0.2f);
            flickeringWait = new WaitForSeconds(0.8f);
        }

        void SubscribeEvent()
        {
            stat.OnValueChagned += health_OnValueChanged;
            GameController.OnGameStart += OnGameStart;
            GameController.OnGameStop += OnGameStop;
        }

        void UnsubscribeEvent()
        {
            stat.OnValueChagned -= health_OnValueChanged;
            GameController.OnGameStart -= OnGameStart;
            GameController.OnGameStop -= OnGameStop;
        }

        void health_OnValueChanged(float value)
        {
            if (isDead)
                return;

            if (value <= 0.0f && !isDead) {
                isDead = true;
                spriteRenderer.sortingOrder = 1;

                //Resetter reset?
                Resetter.instance.Reset();
                //GameController.GameStop();
            }
        }

        void InputHandler()
        {
            if (isDead || !GameController.IsGameStart) {
                inputVector = Vector2.zero;
                isPressJump = false;
                return;
            }

            inputVector.x = Input.GetAxisRaw("Horizontal");
            inputVector.y = Input.GetAxisRaw("Vertical");

            if (inputVector.x > 0.0f) {
                inputVector.x = 1.0f;
            }
            else if (inputVector.x < 0.0f) {
                inputVector.x = -1.0f;
            }

            if (inputVector.y > 0.0f) {
                inputVector.y = 1.0f;
            }
            else if (inputVector.y < 0.0f) {
                inputVector.y = -1.0f;
            }

            isPressJump = Input.GetButton("Jump");

            previousPressJumpState = currentPressJumpState;
            currentPressJumpState = isPressJump;

            isJumpKeyDown = (!previousPressJumpState && currentPressJumpState);
            isJumpKeyUp = (previousPressJumpState && !currentPressJumpState);

            if (isJumpKeyDown && totalJump < 1) {
                allowPlayJumpAudio = true;

            }

            if (isJumpKeyUp) {
                isJumped = false;
                totalJump += 1;
            }
        }

        void AnimationHandler()
        {
            if (isGrounded) {
                if (inputVector.x == 0.0f)
                    anim.Play("Idle");
                else
                    anim.Play("Walk");
            }
            else {
                if (velocity.y > 0.0f)
                    anim.Play("Jump");
            }
        }

        void CheckHitEnemy()
        {
            if (isDead)
                return;

            if (isInvinsible)
                return;

            Collider2D collider = Physics2D.OverlapBox(transform.position + offset, size, 0.0f, enemyLayer, 0.0f, 0.0f);

            if (collider == null)
                return;

            isInvinsible = true;
        }

        void MovementHandler()
        {
            groundRaycastDirection.x = isFacingRight ? -1.0f : 1.0f;

            raycastGroundHit = Physics2D.BoxCast(ground.position, boxCastBodySize, 0.0f, Vector2.down, 0.02f, groundLayer);
            raycastHeadHit = Physics2D.BoxCast(head.position, boxCastHeadSize, 0.0f, Vector2.up, 0.02f, headButtLayer);
            raycastFallingCheck = Physics2D.BoxCast(ground.position, boxCastBodySize, 0.0f, Vector2.down, 1.2f, groundLayer); 
            raycastOneWayCollision = Physics2D.BoxCast(ground.position, boxCastBodySize, 0.0f, Vector2.down, 0.03f, oneWayCollisionLayer);

            isGrounded = raycastGroundHit.collider != null;

            previousGroundState = currentGroundState;
            currentGroundState = isGrounded;

            if (raycastFallingCheck.collider == null)
                isFalling = true;
            else
                isFalling = velocity.y <= 0.0f;

            previousOneWayCollsionState = currentOneWayCollisionState;
            currentOneWayCollisionState = raycastOneWayCollision.collider != null;

            isBeginOneWayCollision = !previousOneWayCollsionState && currentOneWayCollisionState;

            if (!GameController.IsGameStart || stat.IsEmpty) {
                velocity.x = 0;
                velocity.y = -onGroundGravity * Time.fixedDeltaTime;
                rigid.velocity = velocity;
                return;
            }

            if (isGrounded) {
                if (velocity.y > 0.0f && raycastOneWayCollision.collider != null && isBeginOneWayCollision) {
                    totalJump = 1;
                    isJumped = true;
                    isBeginOneWayCollision = false;
                }
                else {
                    totalJump = 0;
                }

                isFalling = false;
                allowPlayJumpAudio = true;
                currentJumpVelocity = 0.0f;
                velocity.x = (inputVector.x * moveForce) * Time.fixedDeltaTime;
            }
            else {
                if (previousGroundState && !isPressJump && isFalling && totalJump <= 0) {
                    totalJump = 1;
                }

                velocity.x = (inputVector.x * (moveForce * 0.9f)) * Time.fixedDeltaTime;
            }

            if (isPressJump && !isJumped && totalJump < 1) {
                if (currentJumpVelocity < maxJumpVelocity && raycastHeadHit.collider == null) {
                    velocity.y = jumpForce * Time.fixedDeltaTime;
                    currentJumpVelocity += jumpForce * Time.fixedDeltaTime;

                    if (allowPlayJumpAudio) {
                        anim.Play("Jump");

                        if (audio.isPlaying)
                            audio.Stop();

                        audio.PlayOneShot(jumpAudio);
                        allowPlayJumpAudio = false;
                    }
                }
                else {
                    isJumped = true;

                    if (raycastHeadHit.collider != null) {
                        velocity.y = -onGroundGravity * Time.fixedDeltaTime;
                    }
                }
            }
            else {
                if (isGrounded)
                    velocity.y = rigid.velocity.y + (-onGroundGravity * Time.fixedDeltaTime);
                else
                    velocity.y -= ((rigid.velocity.y * rigid.velocity.y) + gravity) * Time.fixedDeltaTime;
            }

            velocity.y = Mathf.Clamp(velocity.y, -verticalTerminalVelocity * Time.fixedDeltaTime, jumpForce);
            rigid.velocity = velocity;
        }

        void FlipHandler()
        {
            if (!GameController.IsGameStart || Time.timeScale <= 0.0f)
                return;

            if (inputVector.x > 0.0f && !isFacingRight)
                FlipSprite();

            else if (inputVector.x < 0.0f && isFacingRight)
                FlipSprite();
        }

        void FlipSprite()
        {
            isFacingRight = !isFacingRight;
            newScale = transform.localScale;
            newScale.x *= -1.0f;
            transform.localScale = newScale;
        }

        float ConvertNumberLine(float value, float oldMin, float oldMax, float newMin, float newMax)
        {
            float oldRange = (oldMax - oldMin);
            float newRange = (newMax - newMin);
            return (((value - oldMin) * newRange) / oldRange) + newMin;
        }

        void OnGameStart()
        {
            statManipulator.StartManipulate();
        }

        void OnGameStop()
        {
            statManipulator.Stop();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Trophy")) {
                GameController.GameStop();
            }
        }

        public void Reset()
        {
            stat.FullRestore();
            isDead = false;
        }
    }
}