using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{

    public GameObject chica;
    public GameObject chico;

    void OnMouseDown() {
        
        if(Input.GetMouseButtonDown(0)){

            if(chica.activeSelf){
                chica.SetActive(false);
                chico.SetActive(true);
            }
            else if(chico.activeSelf){
                chica.SetActive(true);
                chico.SetActive(false);
            }
        }
    }
}
