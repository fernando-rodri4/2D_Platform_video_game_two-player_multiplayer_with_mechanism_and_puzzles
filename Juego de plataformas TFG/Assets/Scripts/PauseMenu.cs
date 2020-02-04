using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool pause = false;
    public GameObject pauseMenu;
    public GameObject optionsMenu;

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

    public void Pause(){

        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        Time.timeScale = 0f;
        pause = true;
    }

    public void LoadOptions(){

        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void QuitGame(){

        Application.Quit();
        //SceneManager.LoadScene("PrincipalMain");
        Debug.Log("Quitting game...");
    }
}
