using UnityEngine;
using UnityEngine.UI;

namespace MyBird
{
    public class RetryButton : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnRetryClick);
        }

        private void OnRetryClick()
        {
            Debug.Log("클릭됨!");
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}