using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] instantiatePositions;
    [SerializeField] private GameObject spherePrefab;

    [Header("Variables")]
    [SerializeField] private int numOfSpheres = 3;

    [SerializeField] private List<Vector3> instantiatePos;
    [SerializeField] private List<Vector3> availablePos = new List<Vector3>();
    [SerializeField] private List<SphereColor> correctColorOrder = new List<SphereColor>();
    [SerializeField] private List<GameObject> instantiatedSpheres = new List<GameObject>();
    public enum SphereColor { Red, Yellow, Green, None };
    // Start is called before the first frame update
    void Start()
    {
        GetInstantiatePositions();
    }

    // Update is called once per frame
    void Update()
    {
        //call functions
        CheckSphereCount();
    }

    private void CreateSpheresRandomly()
    {
        for (int i = 0; i < numOfSpheres; i++)
        {
            //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            GameObject sphere = Instantiate(spherePrefab);
            sphere.transform.position = GetRandomInstaniatePos();
            SphereColor color = (SphereColor)Random.Range(0, System.Enum.GetValues(typeof(SphereColor)).Length - 1);
            sphere.GetComponent<Renderer>().material.color = GetColorFromEnum(color);

            instantiatedSpheres.Add(sphere);
            correctColorOrder.Add(color);
        }
    }

    private Vector3 GetRandomInstaniatePos()
    {
        if (availablePos.Count == 0)
        {
            availablePos = new List<Vector3>(instantiatePos);
        }
        int index = Random.Range(0, availablePos.Count - 1);
        Vector3 randomPos = availablePos[index];
        availablePos.RemoveAt(index);

        return randomPos;
    }

    private void GetInstantiatePositions()
    {
        for (int i = 0; i < instantiatePositions.Length; i++)
        {
            instantiatePos[i] = instantiatePositions[i].position;
        }
        availablePos = new List<Vector3>(instantiatePos);
    }

    private SphereColor GetSphereColor(GameObject sphere)
    {
        Color color = sphere.GetComponent<Renderer>().material.color;

        if (color == Color.red)
        {
            return SphereColor.Red;
        }
        else if (color == Color.yellow)
        {
            return SphereColor.Yellow;
        }
        else if (color == Color.green)
        {
            return SphereColor.Green;
        }
        else
        {
            return SphereColor.None;
        }
    }

    private Color GetColorFromEnum(SphereColor color)
    {
        switch (color)
        {
            case SphereColor.Red:
                return Color.red;
            case SphereColor.Yellow:
                return Color.yellow;
            case SphereColor.Green:
                return Color.green;
            default:
                return Color.white;
        }
    }

    private void CheckSphereCount()
    {
        GameObject[] sphereInScene = GameObject.FindGameObjectsWithTag(GameTags.sphereTag);
        if(sphereInScene.Length == 0)
        {
            CreateSpheresRandomly();
        }
        Debug.Log(sphereInScene.Length);
    }
}
