using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool pause = false;
    public GameObject pauseMenu;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){

            if(pause){

                Resume();
            }
            else{

                Pause();
            }
        }
    }

    public void Resume(){

        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        pause = false;        
    }

    void Pause(){

        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        pause = true;
    }

    public void LoadMenu(){

        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
        Debug.Log("Loading menu...");
    }

    public void QuitGame(){

        SceneManager.LoadScene("PrincipalMain");
        Debug.Log("Quitting game...");
    }
}
