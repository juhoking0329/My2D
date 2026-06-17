using UnityEngine;
using UnityEngine.InputSystem;

namespace My2DGame
{
    /// <summary>
    /// 플레이어 좌우 이동: Rigidbody2D.velocity를 사용하여 입력만큼 이동합니다.
    /// New Input System / Move 액션은 __Invoke Unity Events__로 설정하고
    /// 해당 이벤트에 이 클래스의 OnMove(Vector2) 메서드를 연결하세요.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("좌우 이동 속도")]
        private float moveSpeed = 5f;

        private Rigidbody2D rb;
        private Vector2 moveInput;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // 파라미터 타입을 InputAction.CallbackContext로 변경
        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            if (rb == null) return;

            Vector2 vel = rb.linearVelocity;
            vel.x = moveInput.x * moveSpeed;
            rb.linearVelocity = vel;
        }
    }
}
