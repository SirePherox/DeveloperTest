using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTriggerController : MonoBehaviour
{
    [Header("References")]
    private PuzzleManager puzzleMan;

    // Start is called before the first frame update
    void Start()
    {
        puzzleMan = GetComponent<PuzzleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
