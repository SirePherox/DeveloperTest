using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] instantiatePositions;
    [SerializeField] private GameObject spherePrefab;

    [Header("Variables")]
    public bool isStartPuzzle;
    [SerializeField] private int numOfSpheres = 3;

    [SerializeField] private List<Vector3> instantiatePos;
    [SerializeField] private List<Vector3> availablePos = new List<Vector3>();
    [SerializeField] private List<Color> sphereColors;
    [SerializeField] private List<Color> availableColors = new List<Color>();
     public List<Color> correctColorOrder = new List<Color>();
    [SerializeField] private List<GameObject> instantiatedSpheres = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        isStartPuzzle = false;
        GetInstantiatePositions();
        AddSphereColorsToList();
    }

    // Update is called once per frame
    void Update()
    {
        //call functions
        if (isStartPuzzle)
        {
            CheckSphereCount();

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(GameTags.playerTag))
        {
            Debug.Log("Player activated the trigger");
            isStartPuzzle = true;
        }
    }
    private void CreateSpheresRandomly()
    {
        for (int i = 0; i < numOfSpheres; i++)
        {
            //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            GameObject sphere = Instantiate(spherePrefab);
            sphere.transform.position = GetRandomInstaniatePos();
            //SphereColor color = (SphereColor)Random.Range(0, System.Enum.GetValues(typeof(SphereColor)).Length - 1);

            // sphere.GetComponent<Renderer>().material.color = GetColorFromEnum(color);
            Color randColor = GetRandomColor();
            sphere.GetComponent<Renderer>().material.color = randColor;

            instantiatedSpheres.Add(sphere);
            correctColorOrder.Add(randColor);
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

    private Color GetRandomColor()
    {
        if(availableColors.Count == 0)
        {
            availableColors = new List<Color>(sphereColors);
        }

        int index = Random.Range(0, availableColors.Count - 1);
        Color randomColor = availableColors[index];
        availableColors.RemoveAt(index);

        return randomColor;
    }
    private void GetInstantiatePositions()
    {
        for (int i = 0; i < instantiatePositions.Length; i++)
        {
            instantiatePos[i] = instantiatePositions[i].position;
        }
        availablePos = new List<Vector3>(instantiatePos);
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

    private void AddSphereColorsToList()
    {
        sphereColors.Add(Color.red);
        sphereColors.Add(Color.green);
        sphereColors.Add(Color.yellow);
    }

    public Color GetCurrentColorToShoot()
    {
        if(correctColorOrder.Count != 0)
        {
            Color colorToShoot = correctColorOrder[0];
            return colorToShoot;
        }
        else
        {
            Debug.Log("Can't get any color after correctColorOrder list becomes empty");
            return Color.white;
        }

    }
}
