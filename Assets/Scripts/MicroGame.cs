using UnityEngine;

namespace SuomiPeli
{
    public abstract class MicroGame : MonoBehaviour
    {
        [SerializeField] protected string gameName = "Micro Game";
        [SerializeField] protected string instructionText = "Do something!";

        protected float timeLimit;
        protected float timeRemaining;
        protected bool isCompleted;

        public virtual void Initialize(float timeLimit)
        {
            this.timeLimit = timeLimit;
            this.timeRemaining = timeLimit;
            this.isCompleted = false;
            SetupGame();
        }

        protected virtual void Update()
        {
            if (isCompleted) return;

            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0f)
            {
                OnTimeExpired();
            }
        }

        protected abstract void SetupGame();

        protected virtual void OnTimeExpired()
        {
            CompleteGame(false);
        }

        protected void CompleteGame(bool success)
        {
            if (isCompleted) return;

            isCompleted = true;
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnMicroGameCompleted(success);
            }
        }

        public virtual void CleanUp()
        {
            Destroy(gameObject);
        }

        public float GetTimeRemaining() => timeRemaining;
        public float GetTimeLimit() => timeLimit;
        public float GetTimeProgress() => 1f - (timeRemaining / timeLimit);
        public string GetGameName() => gameName;
        public string GetInstructionText() => instructionText;
    }
}
