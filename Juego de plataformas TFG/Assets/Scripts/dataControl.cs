using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dataControl : MonoBehaviour
{

    public GameObject girl;
    public GameObject boy; 
    public GameObject elect1;
    public GameObject elect2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        elect1 = GameObject.FindGameObjectWithTag("elect1");
        elect2 = GameObject.FindGameObjectWithTag("elect2");
        
        if(elect1 == true){

            girl.SetActive(true);
        }

        if(elect2 == true){

            boy.SetActive(true);
        }
    }
}
