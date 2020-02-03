// This script is a Manager that controls the the flow and control of the level. It keeps
// track of player data (thieves count, total level time) and interfaces with
// the UILevelManager.
using UnityEngine;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine.Networking;

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
    /// Player that I controll
    /// </summary>
    GameObject player = null;

    /// <summary>
    /// The collection of scene thieves
    /// </summary>
    List<Thief> thieves;
    
    /// <summary>
    /// Number of thieves captured
    /// </summary>
    int numThieves = 0;

    /// <summary>
    /// Length of the total game time
    /// </summary>
    float totalGameTime;

    /// <summary>
    /// Is the game currently over?
    /// </summary>
    bool isGameOver = false;

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

        //Create out collection to hold the thieves
        thieves = new List<Thief>();
    }

    // Update is called once per frame
    void Update()
    {
        //If the game is over, exit
        if (isGameOver)
            return;

        if (cinemachineVC.Follow == null)
        {
            SetCameraCinemachine();
        }

        //Update the total game time and tell the UI Manager to update
        totalGameTime += Time.deltaTime;
    }

    /// <summary>
    /// Register to the thief "thief"
    /// </summary>
    /// <param name="thief">Thief who is going to register at the level</param>
    public void RegisterThief(Thief thief)
    {
        //If there is no current LevelManager, exit
        if (Instance == null)
            return;

        //If the thieves collection doesn't already contain this thief, add it
        if (!thieves.Contains(thief))
            thieves.Add(thief);
    }

    /// <summary>
    /// Remove to the thief "thief"
    /// </summary>
    /// <param name="thief">Thief who has been captured at the level</param>
    public void PlayerCaptureThief(Thief thief)
    {
        //If there is no current Game Manager, exit
        if (Instance == null)
            return;

        //If the thieves collection doesn't have this thief, exit
        if (!thieves.Contains(thief))
            return;

        //Remove the collected thief
        thieves.Remove(thief);

        numThieves ++;

        //Tell the UIManager to update the thief text
        StartCoroutine(UILevelManager.Instance.UpdateThiefUI(numThieves));
    }

    void SetCameraCinemachine()
    {
        var players = ClientScene.localPlayers;

        for (int i = 0; i < players.Count && cinemachineVC.Follow == null; i++)
        {
            if (players[i].gameObject.GetComponent<NetworkIdentity>().hasAuthority)
            {
                player = players[i].gameObject;

                cinemachineVC.LookAt = players[i].gameObject.transform;
                cinemachineVC.Follow = players[i].gameObject.transform;
            }
        }

        foreach (var actualPlayer in players)
        {
            foreach (var otherPlayer in players)
            {
                if (actualPlayer != otherPlayer)
                {
                    Physics2D.IgnoreCollision(actualPlayer.gameObject.GetComponent<Collider2D>(), otherPlayer.gameObject.GetComponent<Collider2D>());
                }
            }
        }
    }
}
