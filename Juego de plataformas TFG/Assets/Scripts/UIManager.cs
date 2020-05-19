// This script is a Manager that controls the UI HUD (thieves captured, time) for the project.

using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    UIManager() { }

    /// <summary>
    /// This class holds a static reference to itself to ensure that there will only be
    /// one in existence. This is often referred to as a "singleton" design pattern. Other
    /// scripts access this one through this instance.
    /// </summary>
    public static UIManager Instance = null;

    void Awake()
    {
        //If an UILevelManager exists and it is not this...
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Error with UILevelManager script components, 2 instances " + this);
            //...destroy this and exit. There can be only one UILevelManager.
            Destroy(gameObject);
            return;
        }

        //This is the Instance UILevelManager.
        Instance = this;
    }
}
