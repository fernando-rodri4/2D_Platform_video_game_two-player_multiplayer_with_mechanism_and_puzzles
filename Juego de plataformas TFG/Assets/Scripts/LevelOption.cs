using UnityEngine;
using System.Linq;

public class LevelOption : MonoBehaviour
{

    public string elect;

    GameObject network;
    GameObject skin;
    GameObject level;
    
    void Awake()
    {
        network = GameObject.Find("NetworkManager");
        skin = GameObject.Find("player");
        level = GameObject.Find("level");

        var objs = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "level");

        Destroy(network);
        Destroy(skin);

        if (objs.Count() > 1) Destroy(level);

    }

    void Start()
    {
        elect = "";
        DontDestroyOnLoad(this.gameObject);
    }

    public void GetLevel(string name)
    {
        
        elect = name;
    }
}
