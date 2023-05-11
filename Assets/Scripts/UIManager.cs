using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[DefaultExecutionOrder(200)]
public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI colorToShoot;
    private PuzzleManager puzzleManager;

    // Start is called before the first frame update
    void Start()
    {
        puzzleManager = GameObject.Find(GameTags.puzzleMan).GetComponent<PuzzleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //call functions
        UpdateUI();
    }

    private void UpdateUI()
    {
        colorToShoot.text = GetColorText(puzzleManager.GetCurrentColorToShoot());
        colorToShoot.color = puzzleManager.GetCurrentColorToShoot();
    }

    private string GetColorText(Color color)
    {
        string colorText = " ";
        if(color == Color.red)
        {
            colorText = "RED";
        }
        else if(color == Color.green)
        {
            colorText = "GREEN";
        }
        else if(color == Color.yellow)
        {
            colorText = "YELLOW";
        }
        else
        {
            colorText = "...";
            Debug.Log("Color wasnt recognized");
        }
        return colorText;
    }
}
