using UnityEngine;

namespace MyBird
{
    public class PipeMove : MonoBehaviour
    {
        [SerializeField] private float speed = 2f;
        [SerializeField] private float destroyX = -12f;

        private bool isGameOver = false;

        private void OnEnable()
        {
            Player.OnGameOver += StopMoving;
        }

        private void OnDisable()
        {
            Player.OnGameOver -= StopMoving;
        }

        private void StopMoving()
        {
            isGameOver = true;
        }

        private void Update()
        {
            if (isGameOver) return;

            transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);

            if (transform.position.x < destroyX)
            {
                Destroy(gameObject);
            }
        }
    }
}