using UnityEngine;
using UnityEngine.UI;

namespace SuomiPeli
{
    public class SaunaTemperatureMicroGame : MicroGame
    {
        [Header("Sauna Configuration")]
        [SerializeField] private Slider temperatureSlider;
        [SerializeField] private GameObject sliderPrefab;
        
        private const int MIN_TEMP = 60;
        private const int MAX_TEMP = 120;
        private const int CORRECT_TEMP = 80;
        private const int TOLERANCE = 5;

        private Slider spawnedSlider;

        private void Awake()
        {
            gameName = "Sauna Temperature";
            instructionText = "Set sauna to 80°C!";
        }

        protected override void SetupGame()
        {
            if (sliderPrefab != null)
            {
                GameObject sliderObj = Instantiate(sliderPrefab, transform);
                spawnedSlider = sliderObj.GetComponent<Slider>();
            }
            else if (temperatureSlider != null)
            {
                spawnedSlider = temperatureSlider;
            }

            if (spawnedSlider != null)
            {
                spawnedSlider.minValue = MIN_TEMP;
                spawnedSlider.maxValue = MAX_TEMP;
                spawnedSlider.value = Random.Range(MIN_TEMP, MAX_TEMP);
                spawnedSlider.wholeNumbers = true;
            }
        }

        protected override void Update()
        {
            base.Update();

            if (isCompleted || spawnedSlider == null) return;

            int currentTemp = Mathf.RoundToInt(spawnedSlider.value);
            
            if (Mathf.Abs(currentTemp - CORRECT_TEMP) <= TOLERANCE)
            {
                CompleteGame(true);
            }
        }

        protected override void OnTimeExpired()
        {
            CompleteGame(false);
        }
    }
}
