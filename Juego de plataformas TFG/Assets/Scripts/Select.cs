using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{

    public GameObject chica;
    public GameObject chico;

    SpriteRenderer SR;
    public bool over;
    public Color32 ColourNormal;
    public Color32 ColourOver;

    void Start(){

        SR = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown() {

            if(chica.activeSelf){
                chica.SetActive(false);
                chico.SetActive(true);
            }
            else if(chico.activeSelf){
                chica.SetActive(true);
                chico.SetActive(false);
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
