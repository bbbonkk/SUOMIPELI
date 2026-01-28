using UnityEngine;
using UnityEngine.InputSystem;

namespace SuomiPeli
{
    public class BusSeatingMicroGame : MicroGame
    {
        [Header("Bus Configuration")]
        [SerializeField] private GameObject seatPrefab;
        [SerializeField] private GameObject occupiedPersonPrefab;
        [SerializeField] private int totalSeats = 24;
        [SerializeField] private int rowCount = 8;
        [SerializeField] private float seatSpacing = 1.5f;
        [SerializeField] private int maxOccupants = 8;
        [SerializeField] private int minOccupants = 5;

        private GameObject[] seats;
        private bool[] seatOccupied;
        private int correctSeatIndex = -1;

        private Camera mainCamera;

        private void Awake()
        {
            gameName = "Bus Seating";
            instructionText = "Choose a seat far from others!";
            mainCamera = Camera.main;
        }

        protected override void SetupGame()
        {
            seats = new GameObject[totalSeats];
            seatOccupied = new bool[totalSeats];

            CreateBusSeats();
            PlaceRandomOccupants();
            DetermineCorrectSeat();
        }

        private void CreateBusSeats()
        {
            int seatsPerRow = totalSeats / rowCount;
            
            for (int i = 0; i < totalSeats; i++)
            {
                int row = i / seatsPerRow;
                int col = i % seatsPerRow;

                Vector3 position = new Vector3(
                    col * seatSpacing - (seatsPerRow - 1) * seatSpacing * 0.5f,
                    0,
                    row * seatSpacing
                );

                GameObject seat = Instantiate(seatPrefab, transform);
                seat.transform.localPosition = position;
                seat.name = $"Seat_{i}";
                
                SeatInteraction interaction = seat.AddComponent<SeatInteraction>();
                interaction.Initialize(i, this);

                seats[i] = seat;
                seatOccupied[i] = false;
            }
        }

        private void PlaceRandomOccupants()
        {
            int occupantCount = Random.Range(minOccupants, Mathf.Min(maxOccupants, totalSeats - 1));

            for (int i = 0; i < occupantCount; i++)
            {
                int randomSeat;
                int attempts = 0;
                
                do
                {
                    randomSeat = Random.Range(0, totalSeats);
                    attempts++;
                } while (seatOccupied[randomSeat] && attempts < 50);

                if (!seatOccupied[randomSeat])
                {
                    GameObject person = Instantiate(occupiedPersonPrefab, seats[randomSeat].transform);
                    person.transform.localPosition = Vector3.up * 0.5f;
                    seatOccupied[randomSeat] = true;
                }
            }
        }

        private void DetermineCorrectSeat()
        {
            float maxMinDistance = -1f;
            correctSeatIndex = -1;

            for (int i = 0; i < totalSeats; i++)
            {
                if (seatOccupied[i]) continue;

                float minDistanceToOccupied = float.MaxValue;

                for (int j = 0; j < totalSeats; j++)
                {
                    if (seatOccupied[j])
                    {
                        float distance = Vector3.Distance(
                            seats[i].transform.position,
                            seats[j].transform.position
                        );
                        minDistanceToOccupied = Mathf.Min(minDistanceToOccupied, distance);
                    }
                }

                if (minDistanceToOccupied > maxMinDistance)
                {
                    maxMinDistance = minDistanceToOccupied;
                    correctSeatIndex = i;
                }
            }

            Debug.Log($"Correct seat is index: {correctSeatIndex}");
        }

        public void OnSeatClicked(int seatIndex)
        {
            if (isCompleted) return;

            if (seatOccupied[seatIndex])
            {
                CompleteGame(false);
                return;
            }

            bool isCorrect = (seatIndex == correctSeatIndex);
            
            if (isCorrect)
            {
                HighlightSeat(seatIndex, Color.green);
            }
            else
            {
                HighlightSeat(seatIndex, Color.red);
                HighlightSeat(correctSeatIndex, Color.green);
            }

            CompleteGame(isCorrect);
        }

        private void HighlightSeat(int seatIndex, Color color)
        {
            if (seats[seatIndex] != null)
            {
                Renderer renderer = seats[seatIndex].GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = color;
                }
            }
        }
    }

    public class SeatInteraction : MonoBehaviour
    {
        private int seatIndex;
        private BusSeatingMicroGame parentGame;

        public void Initialize(int index, BusSeatingMicroGame game)
        {
            seatIndex = index;
            parentGame = game;
        }

        public void OnClicked()
        {
            if (parentGame != null)
            {
                parentGame.OnSeatClicked(seatIndex);
            }
        }

        private void OnMouseDown()
        {
            OnClicked();
        }
    }
}
