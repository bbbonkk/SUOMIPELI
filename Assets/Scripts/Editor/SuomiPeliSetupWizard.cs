using UnityEngine;
using UnityEditor;
using System.IO;

namespace SuomiPeli.Editor
{
    public class SuomiPeliSetupWizard : EditorWindow
    {
        [MenuItem("Suomipeli/Setup Wizard")]
        public static void ShowWindow()
        {
            GetWindow<SuomiPeliSetupWizard>("Suomipeli Setup");
        }

        private void OnGUI()
        {
            GUILayout.Label("Suomipeli Setup Wizard", EditorStyles.boldLabel);
            GUILayout.Space(10);

            if (GUILayout.Button("1. Create Folders"))
            {
                CreateFolders();
            }

            GUILayout.Space(5);

            if (GUILayout.Button("2. Create Basic Prefabs"))
            {
                CreateBasicPrefabs();
            }

            GUILayout.Space(5);

            if (GUILayout.Button("3. Create MicroGame Prefabs"))
            {
                CreateMicroGamePrefabs();
            }

            GUILayout.Space(5);

            if (GUILayout.Button("4. Setup Scene Objects"))
            {
                SetupSceneObjects();
            }

            GUILayout.Space(20);
            GUILayout.Label("Note: UI setup must be done manually.", EditorStyles.helpBox);
            GUILayout.Label("See Setup Instructions page for details.", EditorStyles.helpBox);
        }

        private void CreateFolders()
        {
            CreateFolderIfNotExists("Assets/Prefabs");
            CreateFolderIfNotExists("Assets/Prefabs/MicroGames");
            CreateFolderIfNotExists("Assets/Materials");
            
            AssetDatabase.Refresh();
            Debug.Log("Folders created successfully!");
        }

        private void CreateFolderIfNotExists(string path)
        {
            if (!AssetDatabase.IsValidFolder(path))
            {
                string parentFolder = Path.GetDirectoryName(path).Replace("\\", "/");
                string folderName = Path.GetFileName(path);
                AssetDatabase.CreateFolder(parentFolder, folderName);
            }
        }

        private void CreateBasicPrefabs()
        {
            CreateSeatPrefab();
            CreatePersonPrefab();
            CreateCoffeePrefab();
            CreateTeaPrefab();
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Basic prefabs created successfully!");
        }

        private void CreateSeatPrefab()
        {
            GameObject seat = GameObject.CreatePrimitive(PrimitiveType.Cube);
            seat.name = "Seat";
            seat.transform.localScale = new Vector3(0.8f, 0.3f, 0.8f);
            
            Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.color = new Color(0.3f, 0.5f, 0.8f);
            seat.GetComponent<Renderer>().material = mat;
            
            AssetDatabase.CreateAsset(mat, "Assets/Materials/SeatMaterial.mat");
            PrefabUtility.SaveAsPrefabAsset(seat, "Assets/Prefabs/Seat.prefab");
            DestroyImmediate(seat);
        }

        private void CreatePersonPrefab()
        {
            GameObject person = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            person.name = "Person";
            person.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            
            Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.color = new Color(0.8f, 0.3f, 0.3f);
            person.GetComponent<Renderer>().material = mat;
            
            AssetDatabase.CreateAsset(mat, "Assets/Materials/PersonMaterial.mat");
            PrefabUtility.SaveAsPrefabAsset(person, "Assets/Prefabs/Person.prefab");
            DestroyImmediate(person);
        }

        private void CreateCoffeePrefab()
        {
            GameObject coffee = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            coffee.name = "Coffee";
            coffee.transform.localScale = new Vector3(0.5f, 0.8f, 0.5f);
            
            DestroyImmediate(coffee.GetComponent<Collider>());
            coffee.AddComponent<CapsuleCollider>();
            
            Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.color = new Color(0.4f, 0.25f, 0.1f);
            coffee.GetComponent<Renderer>().material = mat;
            
            AssetDatabase.CreateAsset(mat, "Assets/Materials/CoffeeMaterial.mat");
            PrefabUtility.SaveAsPrefabAsset(coffee, "Assets/Prefabs/Coffee.prefab");
            DestroyImmediate(coffee);
        }

        private void CreateTeaPrefab()
        {
            GameObject tea = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            tea.name = "Tea";
            tea.transform.localScale = new Vector3(0.5f, 0.8f, 0.5f);
            
            DestroyImmediate(tea.GetComponent<Collider>());
            tea.AddComponent<CapsuleCollider>();
            
            Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.color = new Color(0.3f, 0.7f, 0.3f);
            tea.GetComponent<Renderer>().material = mat;
            
            AssetDatabase.CreateAsset(mat, "Assets/Materials/TeaMaterial.mat");
            PrefabUtility.SaveAsPrefabAsset(tea, "Assets/Prefabs/Tea.prefab");
            DestroyImmediate(tea);
        }

        private void CreateMicroGamePrefabs()
        {
            GameObject seatPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Seat.prefab");
            GameObject personPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Person.prefab");
            GameObject coffeePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Coffee.prefab");
            GameObject teaPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Tea.prefab");

            if (seatPrefab == null || personPrefab == null)
            {
                Debug.LogError("Basic prefabs not found! Create them first.");
                return;
            }

            CreateBusGamePrefab(seatPrefab, personPrefab);
            CreateCoffeeGamePrefab(coffeePrefab, teaPrefab);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("MicroGame prefabs created successfully!");
        }

        private void CreateBusGamePrefab(GameObject seatPrefab, GameObject personPrefab)
        {
            GameObject busGame = new GameObject("BusSeatingGame");
            BusSeatingMicroGame busScript = busGame.AddComponent<BusSeatingMicroGame>();
            
            SerializedObject so = new SerializedObject(busScript);
            so.FindProperty("seatPrefab").objectReferenceValue = seatPrefab;
            so.FindProperty("occupiedPersonPrefab").objectReferenceValue = personPrefab;
            so.ApplyModifiedProperties();
            
            PrefabUtility.SaveAsPrefabAsset(busGame, "Assets/Prefabs/MicroGames/BusSeatingGame.prefab");
            DestroyImmediate(busGame);
        }

        private void CreateCoffeeGamePrefab(GameObject coffeePrefab, GameObject teaPrefab)
        {
            GameObject coffeeGame = new GameObject("CoffeeChoiceGame");
            CoffeeChoiceMicroGame coffeeScript = coffeeGame.AddComponent<CoffeeChoiceMicroGame>();
            
            SerializedObject so = new SerializedObject(coffeeScript);
            so.FindProperty("coffeePrefab").objectReferenceValue = coffeePrefab;
            so.FindProperty("teaPrefab").objectReferenceValue = teaPrefab;
            so.ApplyModifiedProperties();
            
            PrefabUtility.SaveAsPrefabAsset(coffeeGame, "Assets/Prefabs/MicroGames/CoffeeChoiceGame.prefab");
            DestroyImmediate(coffeeGame);
        }

        private void SetupSceneObjects()
        {
            GameObject busGame = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/MicroGames/BusSeatingGame.prefab");
            GameObject coffeeGame = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/MicroGames/CoffeeChoiceGame.prefab");

            if (busGame == null || coffeeGame == null)
            {
                Debug.LogError("MicroGame prefabs not found! Create them first.");
                return;
            }

            GameObject gameManager = new GameObject("GameManager");
            GameManager gmScript = gameManager.AddComponent<GameManager>();
            
            GameObject spawnPoint = new GameObject("MicroGameSpawnPoint");
            spawnPoint.transform.parent = gameManager.transform;
            spawnPoint.transform.position = new Vector3(0, 0, 5);
            
            SerializedObject so = new SerializedObject(gmScript);
            SerializedProperty prefabsArray = so.FindProperty("microGamePrefabs");
            prefabsArray.arraySize = 2;
            prefabsArray.GetArrayElementAtIndex(0).objectReferenceValue = busGame;
            prefabsArray.GetArrayElementAtIndex(1).objectReferenceValue = coffeeGame;
            so.FindProperty("microGameSpawnPoint").objectReferenceValue = spawnPoint.transform;
            so.ApplyModifiedProperties();

            GameObject scoreManager = new GameObject("ScoreManager");
            scoreManager.AddComponent<ScoreManager>();

            GameObject inputManager = new GameObject("InputManager");
            inputManager.AddComponent<InputManager>();

            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                mainCamera.transform.position = new Vector3(0, 5, -5);
                mainCamera.transform.rotation = Quaternion.Euler(45, 0, 0);
            }

            Debug.Log("Scene objects created successfully!");
            Debug.Log("You still need to create the UI manually. See Setup Instructions.");
        }
    }
}
