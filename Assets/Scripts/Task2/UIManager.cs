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

    void Start()
    {
        puzzleManager = GameObject.Find(GameTags.puzzleMan).GetComponent<PuzzleManager>();
    }
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
        if (color == Color.red)
        {
            colorText = "RED";
        }
        else if (color == Color.green)
        {
            colorText = "GREEN";
        }
        else if (color == Color.yellow)
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
