using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool pause = false;   //If the game is paused or not
    public GameObject pauseMenu;
    public GameObject optionsMenu;

    /// <summary>
    /// Number of player in level
    /// </summary>
    int players = 0;

    void Awake() {

        Pause();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {

        if (players < 2)
        {
            players = GameObject.FindGameObjectsWithTag("Player").Length;
            
            return;
        }
        else if (players == 2)
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
    }

    /// <summary>
    /// Resume the pause and can continue with the game
    /// </summary>
    public void Resume(){

        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        pause = false;        
    }

    /// <summary>
    /// Pause the game
    /// </summary>
    public void Pause(){

        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        Time.timeScale = 0f;
        pause = true;
    }

    /// <summary>
    /// Show the options of the menu
    /// </summary>
    public void LoadOptions(){

        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Quit the game
    /// </summary>
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
