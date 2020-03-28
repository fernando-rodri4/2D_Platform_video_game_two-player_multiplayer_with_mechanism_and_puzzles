using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepByStepController : MonoBehaviour
{
    public int row, col;
    PuzzleAzteca puzzAzt;

    // Start is called before the first frame update
    void Start()
    {
        GameObject pA = GameObject.Find("PuzzleAzteca");
        puzzAzt = pA.GetComponent<PuzzleAzteca>();
    }

    void OnMouseDown() {
        
        Debug.Log("Row is " + row + " Col is " + col);  //test touch
        puzzAzt.countStep += 1;
        puzzAzt.row = row;
        puzzAzt.col = col;
        puzzAzt.startControl = true;
    }
}
