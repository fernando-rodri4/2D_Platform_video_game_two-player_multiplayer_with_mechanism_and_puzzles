using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ActivateCamera : MonoBehaviour
{
    ActivateCamera() { }

    /// <summary>
    /// This class holds a static reference to itself to ensure that there will only be
    /// one in existence. This is often referred to as a "singleton" design pattern. Other
    /// scripts access this one through this instance.
    /// </summary>
    public static ActivateCamera Instance = null;

    [SerializeField] GameObject[] cameras = null;

    void Awake()
    {
        //If an ActivateCamera exists and it is not this...
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Error with ActivateCamera script components, 2 instances " + this);
            //...destroy this and exit. There can be only one UILevelManager
            Destroy(gameObject);
            return;
        }

        //This is the Instance ActivateCamera and it should persist between scene loads
        Instance = this;
    }

    public void ActivateCamera_(int index)
    {
        cameras[index].SetActive(true);
    }

    public void DeactivateCamera(int index)
    {
        cameras[index].SetActive(false);
    }
}