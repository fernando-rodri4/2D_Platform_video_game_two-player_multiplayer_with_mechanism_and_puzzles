using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Locking object to synchronize the function.
    /// </summary>
    public readonly object lock_ = new object();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsGameOver(){
        return false;
    }
}
