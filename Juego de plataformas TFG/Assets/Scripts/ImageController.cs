using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageController : MonoBehaviour
{
    PuzzleAzteca puzzAzt;
    public GameObject target;
    public bool startMove = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject pA = GameObject.Find("PuzzleAzteca");
        puzzAzt = pA.GetComponent<PuzzleAzteca>();
    }

    // Update is called once per frame
    void Update()
    {
        if(startMove){
            startMove = false;
            this.transform.position = target.transform.position;    //move to new position
            puzzAzt.checkComplete=true;
        }
        
    }
}
