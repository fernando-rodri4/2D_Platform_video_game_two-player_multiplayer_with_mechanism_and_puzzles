using UnityEngine;
using UnityEngine.Audio;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    AudioManager() { }

    //This class holds a static reference to itself to ensure that there will only be
    //one in existence. This is often referred to as a "singleton" design pattern.
    public static AudioManager Instance = null;

    [Header("Buttons")]
    [SerializeField] AudioClip ButtonClip = null;     //Sounds when you push a button

    [Header("Mixer Groups")]
    [SerializeField] AudioMixerGroup effectGroup = null;    //The effect mixer group

    AudioSource effectSource;           //Reference to the generated effect Audio Source

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

        var objs = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "MusicMenu");

        if (objs.Count() > 1) {

            Destroy(Instance.gameObject);
        }

        //This is the Instance AudioLevelManager, we use it for use the class's methods.
        Instance = this;

        DontDestroyOnLoad(this.gameObject);

        effectSource = gameObject.AddComponent<AudioSource>() as AudioSource;

        effectSource.outputAudioMixerGroup = effectGroup;
    }

    public void PlayButtonAudio()
    {
        if (Instance != null)
        {
            //Set the thief sting clip and tell the source to play
            effectSource.clip = ButtonClip;
            effectSource.Play();
        }
    }
}
