using UnityEngine;

public class Select : MonoBehaviour
{
    public GameObject girl;
    public GameObject boy;

    public int elect;

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
