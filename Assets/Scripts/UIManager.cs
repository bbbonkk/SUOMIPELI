using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SuomiPeli
{
    public class UIManager : MonoBehaviour
    {
        [Header("Screens")]
        [SerializeField] private GameObject mainMenuScreen;
        [SerializeField] private GameObject gameplayScreen;
        [SerializeField] private GameObject gameOverScreen;

        [Header("Gameplay UI")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI instructionText;
        [SerializeField] private Image timerFillImage;

        [Header("Game Over UI")]
        [SerializeField] private TextMeshProUGUI finalScoreText;
        [SerializeField] private TextMeshProUGUI highScoreText;

        private ScoreManager scoreManager;

        private void Start()
        {
            scoreManager = FindFirstObjectByType<ScoreManager>();

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnScoreChanged.AddListener(UpdateScore);
                GameManager.Instance.OnGameOver.AddListener(ShowGameOver);
                GameManager.Instance.OnMicroGameStarted.AddListener(UpdateInstruction);
                GameManager.Instance.OnTimeUpdated.AddListener(UpdateTimer);
            }

            ShowMainMenu();
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnScoreChanged.RemoveListener(UpdateScore);
                GameManager.Instance.OnGameOver.RemoveListener(ShowGameOver);
                GameManager.Instance.OnMicroGameStarted.RemoveListener(UpdateInstruction);
                GameManager.Instance.OnTimeUpdated.RemoveListener(UpdateTimer);
            }
        }

        public void ShowMainMenu()
        {
            mainMenuScreen.SetActive(true);
            gameplayScreen.SetActive(false);
            gameOverScreen.SetActive(false);
        }

        public void ShowGameplay()
        {
            mainMenuScreen.SetActive(false);
            gameplayScreen.SetActive(true);
            gameOverScreen.SetActive(false);
        }

        public void ShowGameOver()
        {
            gameplayScreen.SetActive(false);
            gameOverScreen.SetActive(true);

            if (GameManager.Instance != null)
            {
                int finalScore = GameManager.Instance.GetCurrentScore();
                finalScoreText.text = $"Score: {finalScore}";
            }

            if (scoreManager != null)
            {
                highScoreText.text = $"Best: {scoreManager.GetHighScore()}";
            }
        }

        public void OnPlayButtonClicked()
        {
            ShowGameplay();
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartGame();
            }
        }

        public void OnRestartButtonClicked()
        {
            ShowGameplay();
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartGame();
            }
        }

        public void OnMainMenuButtonClicked()
        {
            ShowMainMenu();
        }

        private void UpdateScore(int score)
        {
            if (scoreText != null)
            {
                scoreText.text = $"Score: {score}";
            }
        }

        private void UpdateInstruction(string instruction)
        {
            if (instructionText != null)
            {
                instructionText.text = instruction;
            }
        }

        private void UpdateTimer(float timeRemaining)
        {
            if (timerFillImage != null && GameManager.Instance != null)
            {
                float timeLimit = GameManager.Instance.GetCurrentTimeLimit();
                timerFillImage.fillAmount = timeRemaining / timeLimit;
            }
        }
    }
}
