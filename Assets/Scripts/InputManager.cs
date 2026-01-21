using UnityEngine;
using UnityEngine.InputSystem;

namespace SuomiPeli
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        private Camera mainCamera;

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

            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main Camera not found! Make sure your camera is tagged as MainCamera.");
            }
        }

        private void Update()
        {
            if (GameManager.Instance == null || !GameManager.Instance.IsGameActive)
                return;

            HandleInput();
        }

        private void HandleInput()
        {
            bool inputDetected = false;
            Vector2 screenPosition = Vector2.zero;

            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                inputDetected = true;
                screenPosition = Mouse.current.position.ReadValue();
            }
            else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                inputDetected = true;
                screenPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            }

            if (inputDetected)
            {
                HandleClick(screenPosition);
            }
        }

        private void HandleClick(Vector2 screenPosition)
        {
            if (mainCamera == null)
                return;

            Ray ray = mainCamera.ScreenPointToRay(screenPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f))
            {
                Debug.Log($"Hit object: {hit.collider.gameObject.name}");

                SeatInteraction seatInteraction = hit.collider.GetComponent<SeatInteraction>();
                if (seatInteraction != null)
                {
                    seatInteraction.OnClicked();
                    return;
                }

                ChoiceInteraction choiceInteraction = hit.collider.GetComponent<ChoiceInteraction>();
                if (choiceInteraction != null)
                {
                    choiceInteraction.OnClicked();
                    return;
                }
            }
        }
    }
}
