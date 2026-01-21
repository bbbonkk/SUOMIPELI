using UnityEngine;

namespace SuomiPeli
{
    public class ScoreManager : MonoBehaviour
    {
        private const string HIGH_SCORE_KEY = "HighScore";

        private int currentSessionHighScore;

        private void Start()
        {
            currentSessionHighScore = GetHighScore();

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnScoreChanged.AddListener(OnScoreUpdated);
            }
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnScoreChanged.RemoveListener(OnScoreUpdated);
            }
        }

        private void OnScoreUpdated(int newScore)
        {
            if (newScore > currentSessionHighScore)
            {
                currentSessionHighScore = newScore;
                SaveHighScore(newScore);
            }
        }

        public int GetHighScore()
        {
            return PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
        }

        private void SaveHighScore(int score)
        {
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, score);
            PlayerPrefs.Save();
        }

        public void ResetHighScore()
        {
            PlayerPrefs.DeleteKey(HIGH_SCORE_KEY);
            PlayerPrefs.Save();
            currentSessionHighScore = 0;
        }
    }
}
