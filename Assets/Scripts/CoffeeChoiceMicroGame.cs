using UnityEngine;

namespace SuomiPeli
{
    public class CoffeeChoiceMicroGame : MicroGame
    {
        [Header("Coffee Configuration")]
        [SerializeField] private GameObject coffeePrefab;
        [SerializeField] private GameObject teaPrefab;
        [SerializeField] private float itemSpacing = 2f;

        private GameObject coffeeObject;
        private GameObject teaObject;

        private void Awake()
        {
            gameName = "Coffee Time";
            instructionText = "Always choose coffee!";
        }

        protected override void SetupGame()
        {
            bool coffeeOnLeft = Random.value > 0.5f;

            Vector3 leftPosition = new Vector3(-itemSpacing, 0, 0);
            Vector3 rightPosition = new Vector3(itemSpacing, 0, 0);

            if (coffeeOnLeft)
            {
                coffeeObject = Instantiate(coffeePrefab, transform);
                coffeeObject.transform.localPosition = leftPosition;
                
                teaObject = Instantiate(teaPrefab, transform);
                teaObject.transform.localPosition = rightPosition;
            }
            else
            {
                teaObject = Instantiate(teaPrefab, transform);
                teaObject.transform.localPosition = leftPosition;
                
                coffeeObject = Instantiate(coffeePrefab, transform);
                coffeeObject.transform.localPosition = rightPosition;
            }

            AddClickHandler(coffeeObject, true);
            AddClickHandler(teaObject, false);
        }

        private void AddClickHandler(GameObject obj, bool isCoffee)
        {
            ChoiceInteraction interaction = obj.AddComponent<ChoiceInteraction>();
            interaction.Initialize(isCoffee, this);
        }

        public void OnChoiceMade(bool choseCoffee)
        {
            if (isCompleted) return;

            if (choseCoffee)
            {
                HighlightChoice(coffeeObject, Color.green);
            }
            else
            {
                HighlightChoice(teaObject, Color.red);
                HighlightChoice(coffeeObject, Color.green);
            }

            CompleteGame(choseCoffee);
        }

        private void HighlightChoice(GameObject obj, Color color)
        {
            if (obj != null)
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = color;
                }
            }
        }
    }

    public class ChoiceInteraction : MonoBehaviour
    {
        private bool isCorrectChoice;
        private CoffeeChoiceMicroGame parentGame;

        public void Initialize(bool correct, CoffeeChoiceMicroGame game)
        {
            isCorrectChoice = correct;
            parentGame = game;
        }

        public void OnClicked()
        {
            if (parentGame != null)
            {
                parentGame.OnChoiceMade(isCorrectChoice);
            }
        }

        private void OnMouseDown()
        {
            OnClicked();
        }
    }
}
