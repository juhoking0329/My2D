using UnityEngine;

namespace My2DGame
{
    /// <summary>
    /// 적을 관리하는 클래스
    /// </summary>
    public class EnemyController : MonoBehaviour
    {
        #region Variables
        //참조
        private Rigidbody2D rb2D;
        private TouchingDirection touchingDirection;
        private Animator animator;
        private Damageable damageable;

        public DetectionZone detectionZone;
        public DetectionZone groundDetectionZone;

        private bool isFlipped = false;

        //이동
        [SerializeField] private float runSpeed = 5f;

        //이동 방향
        public enum WalkableDirection
        {
            Left,
            Right
        }
        private WalkableDirection walkDirection = WalkableDirection.Right;
        private Vector2 directionVector = Vector2.right;

        //타겟 설정
        private bool hasTarget;
        #endregion

        #region Property
        public WalkableDirection WalkDirection
        {
            get { return walkDirection; }
            private set
            {
                //방향전환이 일어난 시점
                if(walkDirection != value)
                {
                    //이미지 플립
                    transform.localScale *= new Vector2(-1, 1);

                    if(value == WalkableDirection.Left)
                    {
                        directionVector = Vector2.left;
                    }
                    else if(value == WalkableDirection.Right)
                    {
                        directionVector = Vector2.right;
                    }
                }
                walkDirection = value;
            }
        }
        public bool CannotMove => animator.GetBool("CannotMove");

        //타겟 설정
        public bool HasTarget
        {
            get { return hasTarget; }
            private set
            {
                hasTarget = value;
                animator.SetBool(AnimationString.hasTarget, value);
            }
        }

        //어택 쿨타임
        public float CooldownTime
        {
            get
            {
                return animator.GetFloat(AnimationString.cooldownTime);
            }
            private set
            {
                animator.SetFloat(AnimationString.cooldownTime, value);
            }
        }

        //속도 잠김 상태 읽어오기
        public bool LockVelocity
        {
            get 
            {
                return animator.GetBool(AnimationString.lockVelocity);
            }
        }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            rb2D = GetComponent<Rigidbody2D>();
            touchingDirection = GetComponent<TouchingDirection>();
            animator = GetComponent<Animator>();

            damageable = GetComponent<Damageable>();
            //이벤트 함수 등록
            damageable.hitAction += OnHit;

            //그라운드 디텍션 이벤트 함수 등록
            groundDetectionZone.noRemainCollider += OnCliffDectection;
        }

        private void Update()
        {
            //타겟 디렉팅
            HasTarget = detectionZone.IsDectected;

            //공격 쿨타임 계산
            if(CooldownTime > 0f)
            {
                CooldownTime -= Time.deltaTime;
            }
        }

        private void FixedUpdate()
        {
            //벽 체크 - 반전
            if(touchingDirection.IsGround && touchingDirection.IsWall)
            {
                if (!isFlipped) // 한 번만 실행
                {
                    Flip();
                    isFlipped = true;
                }
                else
                {
                    isFlipped = false; // 벽에서 떨어지면 초기화
                }
            }

            //이동 - 넉백 효과가 없을때만 이동 처리
            if (LockVelocity == false)
            {
                if (CannotMove == false)
                {
                    rb2D.linearVelocity = new Vector2(directionVector.x * runSpeed, rb2D.linearVelocity.y);
                }
                else
                {
                    rb2D.linearVelocity = new Vector2(0f, rb2D.linearVelocity.y);
                }
            }
        }
        #endregion

        #region Custom Method
        void Flip()
        {
            if(WalkDirection == WalkableDirection.Left)
            {
                WalkDirection = WalkableDirection.Right;
            }
            else if(walkDirection == WalkableDirection.Right)
            {
                WalkDirection = WalkableDirection.Left;
            }
        }

        //데미지 이벤트 함수에 등록되는 함수
        void OnHit(float damage, Vector2 knockback)
        {
            //넉백 값 적용
            rb2D.linearVelocity = new Vector2(knockback.x, rb2D.linearVelocity.y + knockback.y);
        }

        //낭떨어지 체크 이벤트 함수에 등록된 함수
        void OnCliffDectection()
        {
            if(touchingDirection.IsGround)
            {
                Flip();
            }
        }
        #endregion
    }
}