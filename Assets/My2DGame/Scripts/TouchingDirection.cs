using UnityEngine;

namespace My2DGame
{
    /// <summary>
    /// 접촉면 Ground, Wall, Ceiling 체크하는 클래스
    /// </summary>
    public class TouchingDirection : MonoBehaviour
    {
        #region Variables
        private CapsuleCollider2D touchingCol;
        private Animator animator;

        [SerializeField] private float groundDistance = 0.05f;      //그라운드와 체크 거리
        [SerializeField] private float wallDistance = 0.1f;         //벽까지 체크 거리
        [SerializeField] private float ceilDistance = 0.05f;        //천장까지 체크 거리

        [SerializeField] private ContactFilter2D contactFilter;

        private RaycastHit2D[] groundHits = new RaycastHit2D[5];
        private RaycastHit2D[] wallHits = new RaycastHit2D[5];
        private RaycastHit2D[] ceilHits = new RaycastHit2D[5];

        private bool isFacingRight = true;

        private bool isGround;      //그라운드 체크
        private bool isWall;        //땅 체크
        private bool isCeiling;     //천장 체크
        #endregion

        #region Property
        public bool IsGround
        {
            get { return isGround; }
            private set
            {
                isGround = value;
                animator.SetBool(AnimationString.isGround, value);
            }
        }

        public bool IsWall
        {
            get { return isWall; }
            private set
            {
                isWall = value;
                animator.SetBool(AnimationString.isWall, value);
            }
        }

        public bool IsCeiling
        {
            get { return isCeiling; }
            private set
            {
                isCeiling = value;
                animator.SetBool(AnimationString.isCeiling, value);
            }
        }
        #endregion

        #region Unity Event Methods
        private void Awake()
        {
            touchingCol = GetComponent<CapsuleCollider2D>();
            animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            // 바닥 체크
            IsGround = touchingCol.Cast(Vector2.down, contactFilter, groundHits, groundDistance) > 0;

            // 천정 체크
            IsCeiling = touchingCol.Cast(Vector2.up, contactFilter, ceilHits, ceilDistance) > 0;

            // 벽 체크
            Vector2 wallDir = isFacingRight ? Vector2.right : Vector2.left;
            IsWall = touchingCol.Cast(wallDir, contactFilter, wallHits, wallDistance) > 0;
        }

        public void SetFacingRight(bool facingRight)
        {
            isFacingRight = facingRight;
        }
        #endregion
    }
}