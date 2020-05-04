using UnityEngine;
using System.Linq;

public class Select : MonoBehaviour
{
    //Options to select
    public GameObject girl;
    public GameObject boy;

    public int elect;   //Player elected

    //Instances
    GameObject network;
    GameObject skin;
    
    /// <summary>
    /// Destroy duplicated instances
    /// </summary>
    void Awake()
    {
        network = GameObject.Find("NetworkManager");
        skin = GameObject.Find("player");

        var objs = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "player");

        Destroy(network);

        if (objs.Count() > 1) Destroy(skin);

    }

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        elect = 0;
        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// Change the view of the player selected
    /// </summary>
    public void OnMouseDown() 
    {
        if(girl.activeSelf)
        {
            girl.SetActive(false);
            boy.SetActive(true);

            elect = 1;
        }
        else if(boy.activeSelf)
        {
            girl.SetActive(true);
            boy.SetActive(false);

            elect = 0;
        }
    }
}
