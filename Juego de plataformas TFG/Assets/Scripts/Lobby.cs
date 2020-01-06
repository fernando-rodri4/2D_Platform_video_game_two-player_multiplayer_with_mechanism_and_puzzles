using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : MonoBehaviour
{
    void OnMouseDown() {
    
        if(Input.GetMouseButtonDown(0)){
            Application.LoadLevel("MultiplayerMain");
        }
    }
}