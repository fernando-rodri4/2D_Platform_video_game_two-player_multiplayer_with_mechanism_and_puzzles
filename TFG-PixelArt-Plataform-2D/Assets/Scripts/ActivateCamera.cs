using UnityEngine;

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
            //...destroy this and exit. There can be only one ActivateCamera
            Destroy(gameObject);
            return;
        }

        //This is the Instance ActivateCamera, we use it for use the class's methods.
        Instance = this;
    }

    public void EnableCamera(int index)
    {
        cameras[index].SetActive(true);
    }

    public void DisableCamera(int index)
    {
        cameras[index].SetActive(false);
    }
}