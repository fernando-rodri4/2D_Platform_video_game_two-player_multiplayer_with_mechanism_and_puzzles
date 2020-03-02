using UnityEngine;
using System.Linq;

public class Select : MonoBehaviour
{
    public GameObject girl;
    public GameObject boy;

    public int elect;

    GameObject network;
    GameObject skin;
    
    void Awake()
    {
        network = GameObject.Find("NetworkManager");
        skin = GameObject.Find("player");

        var objs = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "player");

        Destroy(network);

        if (objs.Count() > 1) Destroy(skin);

    }

    void Start()
    {
        elect = 0;
        DontDestroyOnLoad(this.gameObject);
    }

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
