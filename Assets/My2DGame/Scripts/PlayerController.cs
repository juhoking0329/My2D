using UnityEngine;
using UnityEngine.InputSystem;

namespace My2DGame
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float walkSpeed = 3f;
        [SerializeField] private float runSpeed = 6f;
        [SerializeField] private float airMoveMultiplier = 0.5f;

        [Header("Jump")]
        [SerializeField] private float jumpForce = 10f;

        private Rigidbody2D rb;
        private Animator anim;
        private TouchingDirection touchingDirection;
        private Damageable damageable; // 추가

        private Vector2 moveInput = Vector2.zero;
        private bool isRunning = false;

        #region Property
        // Enemy 스타일로 프로퍼티 추가
        public bool CannotMove => anim.GetBool(AnimationString.cannotMove);

        public bool LockVelocity
        {
            get { return anim.GetBool(AnimationString.lockVelocity); }
        }
        #endregion

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            touchingDirection = GetComponent<TouchingDirection>();
            damageable = GetComponent<Damageable>(); // 추가

            // 이벤트 함수 등록
            damageable.hitAction += OnHit;
        }

        private void Update()
        {
            if (anim != null)
            {
                anim.SetBool(AnimationString.isMove, moveInput.x != 0);
                anim.SetBool(AnimationString.isRun, isRunning && moveInput.x != 0);
                anim.SetFloat(AnimationString.velocityY, rb.linearVelocity.y);
            }
        }

        private void FixedUpdate()
        {
            if (rb == null) return;

            // Enemy처럼 LockVelocity로 넉백 처리
            if (LockVelocity == false)
            {
                if (CannotMove == false)
                {
                    float currentSpeed = isRunning ? runSpeed : walkSpeed;
                    if (!touchingDirection.IsGround) currentSpeed *= airMoveMultiplier;

                    float xInput = moveInput.x;
                    if (touchingDirection.IsWall) xInput = 0f;

                    rb.linearVelocity = new Vector2(xInput * currentSpeed, rb.linearVelocity.y);

                    if (touchingDirection.IsCeiling && rb.linearVelocity.y > 0)
                    {
                        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                    }
                }
                else
                {
                    rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
                }
            }

            // 플립
            if (moveInput.x > 0)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                touchingDirection.SetFacingRight(true);
            }
            else if (moveInput.x < 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                touchingDirection.SetFacingRight(false);
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                isRunning = true;
            }
            else if (context.canceled)
            {
                isRunning = false;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started && touchingDirection.IsGround)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                anim.SetTrigger(AnimationString.attackTrigger);
            }
        }

        // Enemy처럼 넉백 이벤트 함수 추가
        void OnHit(float damage, Vector2 knockback)
        {
            rb.linearVelocity = new Vector2(knockback.x, rb.linearVelocity.y + knockback.y);
        }
    }
}