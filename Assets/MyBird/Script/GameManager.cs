using UnityEngine;

namespace MyBird
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public int score = 0;

        private void Awake()
        {
            instance = this;
        }

        private void OnEnable()
        {
            Player.OnGameOver += HandleGameOver;
        }

        private void OnDisable()
        {
            Player.OnGameOver -= HandleGameOver;
        }

        public void AddScore()
        {
            score++;
            Debug.Log($"Score: {score}");
            // 추후 UI 연결 가능
        }

        private void HandleGameOver()
        {
            Debug.Log("게임오버!");
            // 모든 움직이는 오브젝트 정지
            //Time.timeScale = 0f;

        }
    }
}