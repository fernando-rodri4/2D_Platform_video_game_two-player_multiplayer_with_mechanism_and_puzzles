using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameManager() { }

    /// <summary>
    /// This class holds a static reference to itself to ensure that there will only be
    /// one in existence. This is often referred to as a "singleton" design pattern. Other
    /// scripts access this one through this instance.
    /// </summary>
    public static GameManager Instance = null;

    public int currentWidth;
    public int currentHeight;

    void Awake()
    {
        //If an GameManager exists and it is not this...
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Error with GameManager script components, 2 instances " + this);
            //...destroy this and exit. There can be only one UILevelManager.
            Destroy(gameObject);
            return;
        }

        //This is the Instance GameManager.
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentWidth = Screen.width;
        currentHeight = Screen.height;

        DontDestroyOnLoad(this.gameObject);
    }

    public void LevelComplete(int level)
    {
        //The function that shows the corresponding achievement is called
        StartCoroutine(UIManager.Instance.LevelAchievement(level));
    }

    public void CapturedShadows(int index)
    {
        //The function that shows the corresponding achievement is called
        StartCoroutine(UIManager.Instance.CapturedShadows(index));
    }
}
