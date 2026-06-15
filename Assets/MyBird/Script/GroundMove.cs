using UnityEngine;

namespace MyBird
{
    public class GroundMove : MonoBehaviour
    {
        [Header("Move")]
        [SerializeField] private float speed = 2f;

        private bool isPlaying = false;

        private void OnEnable()
        {
            Player.OnGameStart += StartMoving;
        }

        private void OnDisable()
        {
            Player.OnGameStart -= StartMoving;
        }

        private void StartMoving()
        {
            isPlaying = true;
        }

        private void Update()
        {
            if (!isPlaying) return;

            transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);

            if (transform.localPosition.x < -8.4f)
            {
                transform.localPosition = new Vector3(transform.localPosition.x + 8.4f, transform.localPosition.y, transform.localPosition.z);
            }
        }
    }
}