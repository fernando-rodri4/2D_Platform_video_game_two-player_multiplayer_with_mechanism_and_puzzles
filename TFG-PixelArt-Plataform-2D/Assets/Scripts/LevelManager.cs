// This script is a Manager that controls the the flow and control of the level. It keeps
// track of player data (shadows count, total level time).
using UnityEngine;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine.Networking;
using UnityEngine.UI;

public sealed class LevelManager : MonoBehaviour
{
    LevelManager() { }

    /// <summary>
    /// This class holds a static reference to itself to ensure that there will only be
    /// one in existence. This is often referred to as a "singleton" design pattern. Other
    /// scripts access this one through this instance.
    /// </summary>
    public static LevelManager Instance = null;

    /// <summary>
    /// Reference to the CinemachineVirtualCamera component that control the camera
    /// </summary>
    public CinemachineVirtualCamera cinemachineVC = null;

    /// <summary>
    /// The collection of scene shadows
    /// </summary>
    List<Shadow> shadows;
    
    /// <summary>
    /// Number of shadows captured
    /// </summary>
    int numShadows = 0;

    /// <summary>
    /// Number of puzzles in level
    /// </summary>
    int totalPuzzles = 0;

    /// <summary>
    /// Number of puzzles completed in level 
    /// </summary>
    int completedPuzzles = 0;

    /// <summary>
    /// Length of the total game time
    /// </summary>
    float totalGameTime;

    /// <summary>
    /// Is the game currently over?
    /// </summary>
    bool isGameOver = false;

    /// <summary>
    /// Number of player in level
    /// </summary>
    int players = 0;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] Button buttonContinue;

    // Start is called before the first frame update
    void Awake()
    {
        //If a LevelManager exists and this isn't it...
        if (Instance != null && Instance != this)
        {
            //...destroy this and exit. There can only be one LevelManager
            Destroy(gameObject);
            return;
        }

        //Set this as the current game manager
        Instance = this;

        //Create out collection to hold the shadows
        shadows = new List<Shadow>();
    }

    // Update is called once per frame
    void Update()
    {
        if (players < 2)
        {
            players = GameObject.FindGameObjectsWithTag("Player").Length;
            
            return;
        }
        else if (players == 2)
        {
            players++;

            buttonContinue.interactable = true;
            buttonContinue.transform.GetChild(0).GetComponent<Text>().text = "CONTINUAR";
        }

        //If the game is over, exit
        if (isGameOver)
        {
            return;
        }

        if (cinemachineVC.Follow == null)
        {
            SetCameraCinemachine();
        }

        //Update the total game time and tell the UI Manager to update
        totalGameTime += Time.deltaTime;
    }

    /// <summary>
    /// Register to the shadow "shadow"
    /// </summary>
    /// <param name="shadow">Shadow who is going to register at the level</param>
    public void RegisterShadow(Shadow shadow)
    {
        //If there is no current LevelManager, exit
        if (Instance == null)
            return;

        //If the shadows collection doesn't already contain this shadow, add it
        if (!shadows.Contains(shadow))
            shadows.Add(shadow);
    }

    /// <summary>
    /// Remove to the shadow "shadow"
    /// </summary>
    /// <param name="shadow">Shadow who has been captured at the level</param>
    public void PlayerCaptureShadow(Shadow shadow)
    {
        //If there is no current Game Manager, exit
        if (Instance == null)
            return;

        //If the shadows collection doesn't have this shadow, exit
        if (!shadows.Contains(shadow))
            return;

        //Remove the collected shadow
        shadows.Remove(shadow);

        numShadows ++;

        //Tell the UIManager to update the shadow text
        StartCoroutine(UILevelManager.Instance.UpdateShadowUI(numShadows));

        switch (numShadows)
        {
            case 1:
                GameManager.Instance.CapturedShadows(1);
                break;
            case 3:
                GameManager.Instance.CapturedShadows(2);
                break;
        }
    }

    public void RegisterPuzzle()
    {
        //If there is no current LevelManager, exit
        if (Instance == null)
            return;

        //Add one to the level's puzzle counter
        totalPuzzles++;
    }

    public void CompletedPuzzle()
    {
        //If there is no current LevelManager, exit
        if (Instance == null)
            return;

        //Add one to the level's puzzle counter
        completedPuzzles++;
    }

    void SetCameraCinemachine()
    {
        var players = ClientScene.localPlayers;

        for (int i = 0; i < players.Count && cinemachineVC.Follow == null; i++)
        {
            if (players[i].gameObject.GetComponent<NetworkIdentity>().hasAuthority)
            {
                cinemachineVC.LookAt = players[i].gameObject.transform;
                cinemachineVC.Follow = players[i].gameObject.transform;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void PlayerWin()
    {
        isGameOver = true;

        int minutes = (int)(totalGameTime / 60);
        float seconds = totalGameTime % 60f;

        int shadowsTotal = numShadows + shadows.Count;
        string shadowText = numShadows + "/" + shadowsTotal;
        
        string puzzleText = completedPuzzles + "/" + totalPuzzles;

        UILevelManager.Instance.SetStatistics(minutes.ToString("00") + ":" + seconds.ToString("00"), puzzleText, shadowText);
    }

    public void SetGameOverTrue()
    {
        isGameOver = true;
    }
}
