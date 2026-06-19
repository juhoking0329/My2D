using UnityEngine;
using UnityEngine.InputSystem;

namespace My2DGame
{
    /// <summary>
    /// 플레이어 입력 및 이동을 담당하는 클래스
    /// </summary>
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

        private Vector2 moveInput = Vector2.zero;
        private bool isRunning = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            touchingDirection = GetComponent<TouchingDirection>();
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

            float currentSpeed = isRunning ? runSpeed : walkSpeed;
            if (!touchingDirection.IsGround) currentSpeed *= airMoveMultiplier;

            // 벽에 닿으면 이동 막기
            float xInput = moveInput.x;
            if (touchingDirection.IsWall) xInput = 0f;

            rb.linearVelocity = new Vector2(xInput * currentSpeed, rb.linearVelocity.y);

            // 천정에 닿으면 위로 가는 속도 제거
            if (touchingDirection.IsCeiling && rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
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
                Debug.Log("왼쪽 쉬프트키를 누르고 있습니다");
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
    }
}