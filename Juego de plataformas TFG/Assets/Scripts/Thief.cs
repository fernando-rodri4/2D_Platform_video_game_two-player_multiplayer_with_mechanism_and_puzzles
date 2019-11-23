using UnityEngine;

public class Thief : MonoBehaviour
{
    /// <summary>
    /// The visual effects when the thief is captured
    /// </summary>
    [SerializeField] GameObject smokeParticles = null;

    /// <summary>
    /// The layer the player game object is on
    /// </summary>
    int playerLayer;

    /// <summary>
    /// It is true when the player touches the thief, avoids more a collision
    /// </summary>
    bool collisionEnter = false;

    void Start()
    {
        if (smokeParticles == null)
        {
            Destroy(this);
            Debug.LogError("Error with Thief script components " + this);
            return;
        }

        //Get the integer representation of the "Player" layer
        playerLayer = LayerMask.NameToLayer("Player");

        LevelManager.Instance.RegisterThief(this);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //If the collider object isn't on the Player layer, exit. This is more 
        //efficient than string comparisons using Tags
        if (collision.gameObject.layer != playerLayer || collisionEnter)
        {
            return;
        }

        collisionEnter = true;

        //The thief has been touched by the Player, so instantiate an smokeParticles prefab
        //at this location and rotation
        Instantiate(smokeParticles, transform.position, transform.rotation);

        //Tell audio manager to play orb collection audio
        AudioLevelManager.Instance.PlayThiefCollectionAudio();

        //Tell the game manager that this thief was collected
        LevelManager.Instance.PlayerCaptureThief(this);

        //Deactivate this thief to hide it and prevent further collection
        gameObject.SetActive(false);
    }
}
