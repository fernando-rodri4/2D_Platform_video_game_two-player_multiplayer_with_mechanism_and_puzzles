using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool pause = false;
    public GameObject pauseMenu;
    public GameObject optionsMenu;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){

            AudioLevelManager.Instance.PlayButtonAudio();

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

        //Application.Quit();

        NetworkManager_Custom network = (NetworkManager_Custom)FindObjectOfType(typeof(NetworkManager_Custom));

        if (network.isServer)
        {
            network.StopClient();
            network.StopHost();
            network.StopServer();
            Destroy(network.gameObject);
            NetworkManager_Custom.Shutdown();

        }
        else
        {
            network.StopClient();
            Destroy(network.gameObject);
            NetworkManager_Custom.Shutdown();
        }

        //SceneManager.LoadScene("PrincipalMain");
        Debug.Log("Quitting game...");
    }
}
