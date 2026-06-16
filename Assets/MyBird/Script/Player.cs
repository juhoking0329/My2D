using UnityEngine;

namespace MyBird
{
    public class Player : MonoBehaviour
    {
        private enum State { Waiting, Playing, Dead }
        private State state = State.Waiting;

        public static event System.Action OnGameStart;
        public static event System.Action OnGameOver;

        [Header("Waiting")]
        [SerializeField] private float hoverForce = 1.5f;

        [Header("Jump")]
        [SerializeField] private float jumpVelocity = 5f;

        [Header("Rotation")]
        [SerializeField] private float maxUpAngle = 30f;
        [SerializeField] private float maxDownAngle = -90f;
        [SerializeField] private float rotationSpeed = 5f;

        private Rigidbody2D rb;

        // UI
        public GameObject playUI;
        public GameObject readyUI;
        public GameObject gameoverUi;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            rb.gravityScale = 0f;
            readyUI.SetActive(true);
            gameoverUi.SetActive(false);
        }

        private void Update()
        {
            // 입력: 게임 시작(대기 -> 플레이)
            if (state == State.Waiting)
            {
                if (GetInput())
                {
                    state = State.Playing;
                    rb.gravityScale = 1f;
                    OnGameStart?.Invoke();
                    Jump();
                    readyUI.SetActive(false);
                }

                // 대기 상태에서 아래로 떨어질 때만 위쪽으로 보정 힘을 줌
                if (rb != null && rb.linearVelocity.y < 0f)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y + hoverForce * Time.deltaTime);
                }
            }
            else if (state == State.Playing)
            {
                if (GetInput())
                {
                    Jump();
                }
            }
            // Dead 상태면 아무 입력도 받지 않음
            else if (state == State.Dead)
            {
                return;  // 아무것도 안 함
            }

            UpdateRotation();
        }

        /// <summary>
        /// 플랫폼에 따라 입력을 처리하는 함수
        /// </summary>
        private bool GetInput()
        {
#if UNITY_EDITOR
            // 에디터에서는 키보드 / 마우스
            return Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
#elif UNITY_ANDROID
            // 안드로이드에서는 화면 터치
            return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
#else
            return Input.GetMouseButtonDown(0);
#endif
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Point"))
            {
                GameManager.instance.AddScore();
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Pipe"))
            {
                state = State.Dead;
                rb.gravityScale = 0f;
                rb.linearVelocity = Vector2.zero;
                OnGameOver?.Invoke();
                gameoverUi.SetActive(true);

                GetComponent<CircleCollider2D>().enabled = false;
                readyUI.SetActive(false);
                playUI.SetActive(false);
            }
        }
    }
}