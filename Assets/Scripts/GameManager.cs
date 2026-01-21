using UnityEngine;
using UnityEngine.Events;

namespace SuomiPeli
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game Configuration")]
        [SerializeField] private GameObject[] microGamePrefabs;
        [SerializeField] private Transform microGameSpawnPoint;
        [SerializeField] private float initialTimeLimit = 5f;
        [SerializeField] private float minimumTimeLimit = 1.5f;

        [Header("Events")]
        public UnityEvent<int> OnScoreChanged;
        public UnityEvent OnGameOver;
        public UnityEvent<string> OnMicroGameStarted;
        public UnityEvent<float> OnTimeUpdated;

        private MicroGame currentMicroGame;
        private int currentScore;
        private int consecutiveSuccesses;
        private float currentTimeLimit;
        private bool isGameActive;

        private const int POINTS_PER_SUCCESS = 100;
        private const int SPEED_INCREASE_INTERVAL = 3;
        private const float TIME_REDUCTION_PER_LEVEL = 0.2f;

        public bool IsGameActive => isGameActive;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Update()
        {
            if (isGameActive && currentMicroGame != null)
            {
                float timeRemaining = currentMicroGame.GetTimeRemaining();
                OnTimeUpdated?.Invoke(timeRemaining);
            }
        }

        public void StartGame()
        {
            ResetGame();
            isGameActive = true;
            LoadNextMicroGame();
        }

        public void OnMicroGameCompleted(bool success)
        {
            if (!isGameActive) return;

            if (success)
            {
                HandleSuccess();
            }
            else
            {
                HandleFailure();
            }
        }

        private void HandleSuccess()
        {
            currentScore += POINTS_PER_SUCCESS;
            consecutiveSuccesses++;
            OnScoreChanged?.Invoke(currentScore);

            if (consecutiveSuccesses % SPEED_INCREASE_INTERVAL == 0)
            {
                IncreaseDifficulty();
            }

            Invoke(nameof(LoadNextMicroGame), 0.5f);
        }

        private void HandleFailure()
        {
            isGameActive = false;
            OnGameOver?.Invoke();
        }

        private void LoadNextMicroGame()
        {
            if (currentMicroGame != null)
            {
                currentMicroGame.CleanUp();
            }

            GameObject nextGamePrefab = SelectRandomMicroGame();
            GameObject gameObject = Instantiate(nextGamePrefab, microGameSpawnPoint);
            currentMicroGame = gameObject.GetComponent<MicroGame>();
            
            if (currentMicroGame != null)
            {
                currentMicroGame.Initialize(currentTimeLimit);
                OnMicroGameStarted?.Invoke(currentMicroGame.GetGameName());
            }
            else
            {
                Debug.LogError("MicroGame component not found on prefab!");
            }
        }

        private GameObject SelectRandomMicroGame()
        {
            if (microGamePrefabs == null || microGamePrefabs.Length == 0)
            {
                Debug.LogError("No micro game prefabs assigned!");
                return null;
            }
            return microGamePrefabs[Random.Range(0, microGamePrefabs.Length)];
        }

        private void IncreaseDifficulty()
        {
            currentTimeLimit = Mathf.Max(
                minimumTimeLimit,
                currentTimeLimit - TIME_REDUCTION_PER_LEVEL
            );
            Debug.Log($"Difficulty increased! New time limit: {currentTimeLimit}s");
        }

        private void ResetGame()
        {
            currentScore = 0;
            consecutiveSuccesses = 0;
            currentTimeLimit = initialTimeLimit;
            OnScoreChanged?.Invoke(currentScore);
        }

        public int GetCurrentScore() => currentScore;
        public float GetCurrentTimeLimit() => currentTimeLimit;
    }
}
