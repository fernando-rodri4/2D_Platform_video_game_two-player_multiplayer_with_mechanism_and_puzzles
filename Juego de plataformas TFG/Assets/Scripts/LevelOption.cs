using UnityEngine;
using System.Linq;

public class LevelOption : MonoBehaviour
{

    public string elect;    //Level elected

    //Instances
    GameObject network;
    GameObject skin;
    GameObject level;
    
    /// <summary>
    /// //In this method destroy duplicated instances
    /// </summary>
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

    /// <summary>
    /// In this method we prepare the gameObject to still on load
    /// </summary>
    void Start()
    {
        elect = "";
        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// This method gets the level elected from the name 
    /// </summary>
    public void GetLevel(string name)
    {
        
        elect = name;
    }
}
