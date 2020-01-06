﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesController : MonoBehaviour
{
    // Start is called before the first frame update
    public void changeScene(string name)
    {
        
        SceneManager.LoadScene(name);
    }

    // Update is called once per frame
    public void Exit()
    {

        Application.Quit();        
    }
}
