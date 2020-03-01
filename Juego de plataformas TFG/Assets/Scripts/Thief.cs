using UnityEngine;

public class Thief : MonoBehaviour
{
    /// <summary>
    /// The visual effects when the thief is captured
    /// </summary>
    [SerializeField] GameObject smokeParticles = null;

    /// <summary>
	/// Speed and radius vision of the thief
	/// </summary>
	public float visionRadius = 5;
    public float scapeRadius = 3;
    public float speed = 2;

    /// <summary>
	/// position of the thief
	/// </summary>
	Vector3 initialPosition;
    Vector3 target;

    /// <summary>
    /// The layer the player game object is on
    /// </summary>
    int playerLayer;

    /// <summary>
    /// Reference to the sprite renderer of the thief
    /// </summary>
    SpriteRenderer thiefRenderer;

    /// <summary>
    /// It is true when the player touches the thief, avoids more a collision
    /// </summary>
    bool collisionEnter = false;

    /// <summary>
    /// Variables to flip the thief
    /// </summary>
    float distanceX = 0;
    bool flipThief = true;

    void Start()
    {
        if (smokeParticles == null)
        {
            Destroy(this);
            Debug.LogError("Error with Thief script components " + this);
            return;
        }

        thiefRenderer = GetComponent<SpriteRenderer>();

        //Get the integer representation of the "Player" layer
        playerLayer = LayerMask.NameToLayer("Player");

        //Save the thief's position
        initialPosition = transform.position;

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
        //at this location and rotation and destroy the smokeParticles gameObject when pass 1.5 seconds
        GameObject instantiatedSmoke = Instantiate(smokeParticles, transform.position, transform.rotation);
        Destroy(instantiatedSmoke, 1.5f);

        //Tell audio manager to play orb collection audio
        AudioLevelManager.Instance.PlayThiefCollectionAudio();

        //Tell the game manager that this thief was collected
        LevelManager.Instance.PlayerCaptureThief(this);

        //Deactivate this thief to hide it and prevent further collection
        gameObject.SetActive(false);
    }

    void Update()
    {

        //Save an array of players by the tag
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        //Check that there are 2 players
        if (players.Length == 2)
        {

            //The distance of players's position in axis X is smaller than the position of thief
            distanceX = Input.GetAxis("Horizontal");

            //If the distance is smaller than the visionRadius, player will be the new target
            float distance = Vector3.Distance(players[0].transform.position, transform.position);
            float distance2 = Vector3.Distance(players[1].transform.position, transform.position);
            if (distance < visionRadius) { target = players[0].transform.position;}
            if (distance2 < visionRadius){ target = players[1].transform.position;}

            //If distances of players are smaller than the scapeRadius, the thief stop
            if (distance < scapeRadius && distance2 < scapeRadius)
            {
                target = transform.position;
            }

            //If the target is a player
            if (target == players[0].transform.position || target == players[1].transform.position) {

                //If some of the player is not in the scapeRadius, then the thief can flip to scape
                if (distance > scapeRadius || distance2 > scapeRadius)
                {

                    if (distanceX > 0 && flipThief)
                    {
                        Flip();
                    }
                    else if (distanceX < 0 && !flipThief)
                    {
                        Flip();
                    }
                }


                //Move the thief against the player
                float fixedSpeed = -1 * speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, target, fixedSpeed);
                
            }

            Debug.DrawLine(transform.position, target, Color.white);
        }
    }

    //Show the radius vision of the thief
    void OnDrawGizmos() {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, scapeRadius);
    }

    void Flip() 
    {
        flipThief = !flipThief;
        thiefRenderer.flipX = !thiefRenderer.flipX;
    }
}
