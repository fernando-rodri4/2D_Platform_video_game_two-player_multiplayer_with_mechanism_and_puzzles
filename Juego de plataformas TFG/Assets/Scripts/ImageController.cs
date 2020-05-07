using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ImageController : NetworkBehaviour
{

    PuzzleAzteca puzzAzt;   //Instace of PuzzleAzteca
    public GameObject target;   //Target to move the image
    public bool startMove = false;  //If can move the image or not

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        GameObject pA = GameObject.Find("PuzzleAzteca");
        puzzAzt = pA.GetComponent<PuzzleAzteca>();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {
        if(startMove){  //If has authority and can move the image
            startMove = false;
            transform.position = target.transform.position;    //move to new position
            puzzAzt.checkComplete=true;
        }
        
    }
}
