using UnityEngine;

namespace MyBird
{
    public class Player : MonoBehaviour
    {
        private enum State { Waiting, Playing }
        private State state = State.Waiting;

        public static event System.Action OnGameStart;

        [Header("Waiting")]
        [SerializeField] private float hoverForce = 1.5f; // 아래로 떨어질 때 보정해줄 위쪽 힘
        [Header("Jump")]
        [SerializeField] private float jumpVelocity = 5f;

        [Header("Rotation")]
        [SerializeField] private float maxUpAngle = 30f;
        [SerializeField] private float maxDownAngle = -90f;
        [SerializeField] private float rotationSpeed = 5f;

        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogError("Player: Rigidbody2D 컴포넌트가 필요합니다.", this);
            }
        }

        private void Update()
        {
            // 입력: 게임 시작(대기 -> 플레이)
            if (state == State.Waiting)
            {
                // 대기 상태에서 스페이스 또는 클릭 시 플레이 시작
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    state = State.Playing;
                    OnGameStart?.Invoke();
                    Jump();
                }

                // 대기 상태에서 공중에 떠있는 효과: 아래로 떨어질 때만 위쪽으로 보정 힘을 줌
                if (rb != null && rb.linearVelocity.y < 0f)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y + hoverForce * Time.deltaTime);
                }
            }
            else
            {
                // 입력: 점프
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    Jump();
                }
            }

            UpdateRotation();
        }

        private void Jump()
        {
            if (rb == null) return;

            // 기존 수직속도는 덮어쓰고 즉시 위로 튀게 설정
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
        }

        private void UpdateRotation()
        {
            if (rb == null) return;

            // 속도에 비례해 목표 각도 계산 (계수는 튜닝 가능)
            float target = Mathf.Clamp(rb.linearVelocity.y * 7f, maxDownAngle, maxUpAngle);

            // 현재 회전에서 목표 회전으로 부드럽게 보간
            Quaternion targetRot = Quaternion.Euler(0f, 0f, target);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
        }
    }
}