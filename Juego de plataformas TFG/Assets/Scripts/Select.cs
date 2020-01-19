using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{

    public GameObject girl;
    public GameObject boy;

    public GameObject elect1;
    public GameObject elect2;

    SpriteRenderer SR;
    public bool over;
    public Color32 ColourNormal;
    public Color32 ColourOver;

    void Start(){

        SR = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown() {

            if(girl.activeSelf){
                girl.SetActive(false);
                boy.SetActive(true);

                elect1.SetActive(false);
                elect2.SetActive(true);
            }
            else if(boy.activeSelf){
                girl.SetActive(true);
                boy.SetActive(false);

                elect1.SetActive(true);
                elect2.SetActive(false);
            }
    }

    void Update(){

        if(over == true){

            SR.color = ColourOver;
            over = false;
        }
        else{

            SR.color = ColourNormal;
        }
    }
}
