// This script is a Manager that controls all of the audio for the project. All audio
// commands are issued through the static methods of this class. Additionally, this 
// class creates AudioSource "channels" at runtime and manages them

using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    AudioManager() { }

    //This class holds a static reference to itself to ensure that there will only be
    //one in existence. This is often referred to as a "singleton" design pattern.
    public static AudioManager Instance = null;

    void Awake()
    {
        //If an AudioLevelManager exists and it is not this...
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Error with AudioLevelManager script components, 2 instances " + this);
            //...destroy this and exit. There can be only one AudioLevelManager
            Destroy(gameObject);
            return;
        }

        //This is the Instance AudioLevelManager, we use it for use the class's methods.
        Instance = this;

    }

}