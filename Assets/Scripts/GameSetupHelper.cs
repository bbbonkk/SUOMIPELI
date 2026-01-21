using UnityEngine;

namespace SuomiPeli
{
    public class GameSetupHelper : MonoBehaviour
    {
        [ContextMenu("Create Basic Prefabs")]
        public void CreateBasicPrefabs()
        {
            Debug.Log("Use the Unity Editor to create prefabs manually. See setup instructions.");
        }

        public static GameObject CreateSimpleCube(string name, Color color, Vector3 scale)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = name;
            cube.transform.localScale = scale;
            
            Renderer renderer = cube.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                mat.color = color;
                renderer.material = mat;
            }

            return cube;
        }

        public static GameObject CreateSimpleSphere(string name, Color color, Vector3 scale)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = name;
            sphere.transform.localScale = scale;
            
            Renderer renderer = sphere.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                mat.color = color;
                renderer.material = mat;
            }

            return sphere;
        }

        public static GameObject CreateSimpleCylinder(string name, Color color, Vector3 scale)
        {
            GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cylinder.name = name;
            cylinder.transform.localScale = scale;
            
            Renderer renderer = cylinder.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                mat.color = color;
                renderer.material = mat;
            }

            return cylinder;
        }
    }
}
