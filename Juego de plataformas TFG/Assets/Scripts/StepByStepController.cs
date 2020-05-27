using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StepByStepController : NetworkBehaviour
{
    public int row, col;
    PuzzleAzteca puzzAzt;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        GameObject pA = GameObject.Find("PuzzleAzteca");
        puzzAzt = pA.GetComponent<PuzzleAzteca>();
    }

    /// <summary>
    /// OnMouseDown is called when the buttons is pressed down on the mouse
    /// </summary>
    void OnMouseDown() {

        if(hasAuthority)
        {

            Debug.Log("Row is " + row + " Col is " + col);  //test touch
            puzzAzt.countStep += 1;
            puzzAzt.row = row;
            puzzAzt.col = col;
            puzzAzt.startControl = true;
        }
    }
}
