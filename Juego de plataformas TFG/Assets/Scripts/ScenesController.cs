using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesController : MonoBehaviour
{
    [SerializeField] GameObject group, controls;

    // Start is called before the first frame update
    public void changeScene(string name)
    {
        AudioManager.Instance.PlayButtonAudio();
        SceneManager.LoadScene(name);
    }

    // Update is called once per frame
    public void Exit()
    {
        AudioManager.Instance.PlayButtonAudio();
        Application.Quit();        
    }

    public void EnableControlsMenu()
    {
        group.SetActive(false);
        controls.SetActive(true);
    }

    public void DisableControlsMenu()
    {
        group.SetActive(true);
        controls.SetActive(false);
    }

}
