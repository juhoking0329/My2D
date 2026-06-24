using UnityEngine;
using UnityEngine.Events;

namespace My2DGame
{
    /// <summary>
    /// 캐릭터의 체력을 관리하는 클래스
    /// </summary>
    public class Damageable : MonoBehaviour
    {
        #region Variables
        //참조
        private Animator animator;
        private Rigidbody2D rb;

        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float currentHealth;

        //무적 체크
        private bool isInvincible = false;

        [SerializeField] private float invincibleTimer = 0.5f;
        private float countdown = 0f;

        //죽음 체크
        private bool isDeath = false;

        //데미지 입을때 등록된 함수를 호출하는 이벤트 함수 정의
        public UnityAction<float, Vector2> hitAction;
        #endregion

        #region Property
        public float MaxHealth
        {
            get { return maxHealth; }
            private set { maxHealth = value; }
        }

        public float CurrentHealth
        {
            get { return currentHealth; }
            private set { currentHealth = value; }
        }

        public bool IsDeath
        {
            get { return isDeath; }
            private set 
            { 
                isDeath = value;
                animator.SetBool(AnimationString.isDeath, value);
            }
        }

        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            //초기화
            CurrentHealth = MaxHealth;
        }

        private void Update()
        {
            //죽음 체크
            if (IsDeath)
                return;

            //무적 모드
            if(isInvincible)
            {
                countdown += Time.deltaTime;
                if(countdown > invincibleTimer)
                {
                    //무적 모드 해제
                    isInvincible = false;

                    //타이머 초기화
                    countdown = 0f;
                }
            }
        }
        #endregion

        #region Custom Method
        public void TakeDamage(float damage, Vector2 knockback)
        {
            //죽음 체크, 무적모드 체크
            if (IsDeath || isInvincible)
                return;

            CurrentHealth -= damage;
            Debug.Log($"CurrentHealth : {CurrentHealth}");

            //무적 모드 설정
            isInvincible = true;
            //countdown = 0f;

            //애니메이션 처리
            animator.SetTrigger(AnimationString.hitTrigger);

            //데미지 효과 : vfx, sfx

            //데미지 처리 : 넉백, UI 
            if(hitAction != null )
            {
                hitAction.Invoke(damage, knockback);
            }
            //hitAction?.Invoke(damage, knockback);

            if(CurrentHealth <= 0f)
            {
                Death();
            }
        }

        void Death()
        {
            IsDeath = true;

            //죽음 처리
            rb.linearVelocity = Vector2.zero;        // 속도 초기화
            rb.bodyType = RigidbodyType2D.Static;    // 물리 정지 (중력 포함)
        }
        #endregion
    }
}