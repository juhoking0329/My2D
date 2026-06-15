using UnityEngine;

namespace MyBird
{
    public class PipeMove : MonoBehaviour
    {
        [SerializeField] private float speed = 2f;
        [SerializeField] private float destroyX = -12f;  // 화면 왼쪽 밖으로 나가면 삭제

        private void Update()
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);

            if (transform.position.x < destroyX)
            {
                Destroy(gameObject);
            }
        }
    }
}