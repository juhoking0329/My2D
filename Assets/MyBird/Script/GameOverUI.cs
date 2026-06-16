using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyBird
{
    public class GameOverUI : MonoBehaviour
    {
        public void Retry()
        {
            Debug.Log("Retry 버튼 클릭됨!");
            Time.timeScale = 1f; // 멈춘 시간 되돌리기
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}