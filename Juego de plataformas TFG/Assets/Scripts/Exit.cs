using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    void OnMouseDown() {
        
        if(Input.GetMouseButtonDown(0)){
            Application.Quit();
            Debug.Log("Goodbye!");
        }
    }
}
