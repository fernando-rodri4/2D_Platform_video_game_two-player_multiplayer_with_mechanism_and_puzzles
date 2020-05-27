using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesController : MonoBehaviour
{
    [SerializeField] GameObject group, controls;

    /// <summary>
    /// Load the scene selected
    /// </summary>
    public void changeScene(string name)
    {
        AudioManager.Instance.PlayButtonAudio();
        SceneManager.LoadScene(name);
    }

    /// <summary>
    /// Quit the application
    /// </summary>
    public void Exit()
    {
        AudioManager.Instance.PlayButtonAudio();
        Application.Quit();        
    }

    /// <summary>
    /// Show the controls menu
    /// </summary>
    public void EnableControlsMenu()
    {
        group.SetActive(false);
        controls.SetActive(true);
    }

    /// <summary>
    /// Close the control menu
    /// </summary>
    public void DisableControlsMenu()
    {
        group.SetActive(true);
        controls.SetActive(false);
    }

}
