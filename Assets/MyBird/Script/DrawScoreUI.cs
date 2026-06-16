using UnityEngine;
using TMPro;

namespace MyBird
{
    public class DrawScoreUI : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;

        private void Update()
        {
            if(scoreText != null)
            {
                scoreText.text = GameManager.instance.score.ToString();
            }
        }
    }
}