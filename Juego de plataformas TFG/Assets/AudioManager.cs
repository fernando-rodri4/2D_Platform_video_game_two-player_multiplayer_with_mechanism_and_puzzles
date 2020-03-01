using UnityEngine;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    void Awake()
    {

        var objs = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "MusicMenu");

        if (objs.Count() > 1) {

            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
        
    }
}
