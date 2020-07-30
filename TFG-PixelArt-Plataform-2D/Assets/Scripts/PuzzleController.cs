using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PuzzleController : NetworkBehaviour
{
    PuzzleController() { }

    /// <summary>
    /// This class holds a static reference to itself to ensure that there will only be
    /// one in existence. This is often referred to as a "singleton" design pattern.
    /// </summary>
    public static PuzzleController Instance = null;

    /// <summary>
    /// Puzzle pictures
    /// </summary>
    [SerializeField] Transform[] pictures = null;

    /// <summary>
    /// The layer the player game object is on
    /// </summary>

    /// <summary>
    /// List of players
    /// </summary>
    List<GameObject> playersList;

    /// <summary>
    /// Player that is playing
    /// </summary>
    GameObject currentPlayer;

    bool isCorrect = false; //If the puzzle is correct or not

    bool startPuzzle = false;   //If puzzle has started
    bool gameIsCompleted = true;

    int activeForPlayer1 = 0, activeForPlayer2 = 1; //Initial piece for each player

    [SerializeField] GameObject[] picturesAuthority;    //Pieces with authority

    [SerializeField] GameObject puzzleControls; //Controls of the puzzle

    public GameObject rockList;
    public GameObject waterList;
    int playerLayer;
    public Vector3 newPosition; //New position for the rocks
    Vector3 newPosition2;   //New position for the water
    public int numCamera;   //Number of the enabled camera for the puzzle

    void Awake()
    {
        //If an PuzzleController exists and it is not this...
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Error with PuzzleController script components, 2 instances " + this);
            //...destroy this and exit. There can be only one PuzzleController
            Destroy(gameObject);
            return;
        }

        //This is the Instance PuzzleController and it should persist
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (pictures == null || rockList == null || puzzleControls == null)
        {
            Debug.LogError("Error with PuzzleController script component " + this);
            Destroy(this);
            return;
        }

        LevelManager.Instance.RegisterPuzzle();

        //Get the integer representation of the "Player" layer
        playerLayer = LayerMask.NameToLayer("Player");

        playersList = new List<GameObject>();

        //Active the first image for each player

        var players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var player in players)
        {
            if (player.gameObject.GetComponent<NetworkIdentity>().hasAuthority)
            {
                if (player.gameObject.GetComponent<PlayerMovement>().GetId() == 0)
                {
                    pictures[activeForPlayer1].localScale = new Vector3(5.09f, 5, 1);
                }
                else
                {
                    pictures[activeForPlayer2].localScale = new Vector3(5.09f, 5, 1);
                }
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isCorrect || playersList.Count == 0)
        {
            return;
        }

        NextPicture();

        Rotate();

        //Prepare the puzzle
        if (playersList.Count == 2 && !startPuzzle)
        {

            AudioLevelManager.Instance.PlayChangeClipAudio(AudioLevelManager.Instance.puzzleClip);
            ActivateCamera.Instance.EnableCamera(numCamera);

            puzzleControls.SetActive(true);

            foreach (var player in playersList)
            {
                player.GetComponent<PlayerMovement>().canMove = false;

                if (player.GetComponent<PlayerMovement>().hasAuthority)
                {
                    currentPlayer = player;
                }
                if (isServer && !player.GetComponent<PlayerMovement>().hasAuthority)
                {
                    foreach (var item in picturesAuthority)
                    {
                        item.GetComponent<NetworkIdentity>().AssignClientAuthority(player.GetComponent<NetworkIdentity>().clientAuthorityOwner);
                    }
                }
            }

            startPuzzle = true;

            StartCoroutine(FinishControls());
        }

        print("PICTURE 2 = 0: " + (pictures[2].rotation.z % 360));
        print("PICTURE 4 = 0: " + (pictures[4].rotation.z % 360));
        print("PICTURE 8 -2 < x < 2: " + (pictures[8].rotation.z));
        print("PICTURE 19 -2 < x < 2: " + (pictures[19].rotation.z));
        print("PICTURE 21 -2 < x < 2: " + (pictures[21].rotation.z));
        print("PICTURE 25 = 0: " + (pictures[25].rotation.z % 360));
        print("PICTURE 26 -2 < x < 2: " + (pictures[26].rotation.z));
        print("PICTURE 27 -1 < x < 1: " + (pictures[27].rotation.z));
        print("PICTURE 29 = 1: " + (pictures[29].rotation.z));

        //If the puzzle is correct, call CompletePuzzle function
        if ((pictures[2].rotation.z % 360) == 0 && (pictures[3].rotation.z % 360) == 0 &&
            (pictures[4].rotation.z % 360) == 0 && ((pictures[8].rotation.z) > -2 && (pictures[8].rotation.z) < 2) &&
            ((pictures[14].rotation.z) > -2 && (pictures[14].rotation.z) < 2) && ((pictures[19].rotation.z) > -2 && (pictures[19].rotation.z) < 2) &&
            ((pictures[21].rotation.z) > -2 && (pictures[21].rotation.z) < 2) && ((pictures[22].rotation.z) > -2 && (pictures[22].rotation.z) < 2) &&
            ((pictures[23].rotation.z) > -2 && (pictures[23].rotation.z) < 2) && (pictures[25].rotation.z % 360) == 0 &&
            ((pictures[26].rotation.z) > -2 && (pictures[26].rotation.z) < 2) && ((pictures[27].rotation.z) > -1 && (pictures[27].rotation.z) < 1) &&
            (pictures[28].rotation.z % 360) == 0 && ((pictures[29].rotation.z) == 1 ||(pictures[29].rotation.z) == -1) && gameIsCompleted)
        {
            gameIsCompleted = false;
            StartCoroutine(CompletePuzzle());
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer && !playersList.Contains(collision.gameObject))
        {
            playersList.Add(collision.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        playersList.Remove(collision.gameObject);
    }

    /// <summary>
    /// Return the variable 'isCorrect'
    /// </summary>
    /// <returns>Variable that says if the puzzle is solved</returns>
    public bool GetIsCorrect()
    {
        return isCorrect;
    }

    /// <summary>
    /// Rotate picture left or right when player press the correct button
    /// </summary>
    void Rotate()
    {
        if ((Input.GetButtonDown("RotateRight") && currentPlayer.GetComponent<PlayerMovement>().GetId() == 0) || Input.GetButtonDown("Enter") && playersList[0].GetComponent<NetworkIdentity>() == null)
        {
            pictures[activeForPlayer1].GetComponent<TouchRotate>().RotateRight();
            AudioLevelManager.Instance.PlayRotatePuzzleClipAudio();
        }
        else if ((Input.GetButtonDown("RotateRight") && currentPlayer.GetComponent<PlayerMovement>().GetId() == 1) || Input.GetButtonDown("Enter") && playersList[0].GetComponent<NetworkIdentity>() == null)
        {
            pictures[activeForPlayer2].GetComponent<TouchRotate>().RotateRight();
            AudioLevelManager.Instance.PlayRotatePuzzleClipAudio();
        }

        if ((Input.GetButtonDown("RotateLeft") && currentPlayer.GetComponent<PlayerMovement>().GetId() == 0) || Input.GetButtonDown("Enter") && playersList[0].GetComponent<NetworkIdentity>() == null)
        {
            pictures[activeForPlayer1].GetComponent<TouchRotate>().RotateLeft();
            AudioLevelManager.Instance.PlayRotatePuzzleClipAudio();
        }
        else if ((Input.GetButtonDown("RotateLeft") && currentPlayer.GetComponent<PlayerMovement>().GetId() == 1) || Input.GetButtonDown("Enter") && playersList[0].GetComponent<NetworkIdentity>() == null)
        {
            pictures[activeForPlayer2].GetComponent<TouchRotate>().RotateLeft();
            AudioLevelManager.Instance.PlayRotatePuzzleClipAudio();
        }
    }

    /// <summary>
    /// Select the next picture
    /// </summary>
    void NextPicture()
    {
        if ((Input.GetButtonDown("Enter") && currentPlayer.GetComponent<PlayerMovement>().GetId() == 0) || Input.GetButtonDown("Enter") && playersList[0].GetComponent<NetworkIdentity>() == null)
        {
            pictures[activeForPlayer1].localScale = new Vector3(4.09f, 4, 1);
            AudioLevelManager.Instance.PlayChangePuzzleClipAudio();

            if (activeForPlayer1 == 0 || activeForPlayer1 == 2 || activeForPlayer1 == 5 ||
                activeForPlayer1 == 11 || activeForPlayer1 == 13 || activeForPlayer1 == 17 ||
                activeForPlayer1 == 19 || activeForPlayer1 == 25 || activeForPlayer1 == 27)
            {
                activeForPlayer1 = (activeForPlayer1 + 2) % pictures.Length;
            }
            else if (activeForPlayer1 == 4 || activeForPlayer1 == 7 || activeForPlayer1 == 15 ||
            activeForPlayer1 == 16 || activeForPlayer1 == 24 || activeForPlayer1 == 29)
            {
                activeForPlayer1 = (activeForPlayer1 + 1) % pictures.Length;
            }
            else
            {
                activeForPlayer1 = (activeForPlayer1 + 3) % pictures.Length;
            }

            pictures[activeForPlayer1].localScale = new Vector3(5.09f, 5, 1);
        }
        else if ((Input.GetButtonDown("Enter") && currentPlayer.GetComponent<PlayerMovement>().GetId() == 1) || Input.GetButtonDown("Enter") && playersList[0].GetComponent<NetworkIdentity>() == null)
        {

            pictures[activeForPlayer2].localScale = new Vector3(4.09f, 4, 1);
            AudioLevelManager.Instance.PlayChangePuzzleClipAudio();

            if (activeForPlayer2 == 1 || activeForPlayer2 == 10 ||
                activeForPlayer2 == 12 || activeForPlayer2 == 18 ||
                activeForPlayer2 == 20 || activeForPlayer2 == 26)
            {
                activeForPlayer2 = (activeForPlayer2 + 2) % pictures.Length;
            }
            else if (activeForPlayer2 == 9 || activeForPlayer2 == 22)
            {
                activeForPlayer2 = (activeForPlayer2 + 1) % pictures.Length;
            }
            else if (activeForPlayer2 == 6 || activeForPlayer2 == 14)
            {
                activeForPlayer2 = (activeForPlayer2 + 4) % pictures.Length;
            }
            else
            {
                activeForPlayer2 = (activeForPlayer2 + 3) % pictures.Length;
            }

            pictures[activeForPlayer2].localScale = new Vector3(5.09f, 5, 1);
        }
    }

    /// <summary>
    /// Disable the controls of the puzzle
    /// </summary>
    IEnumerator FinishControls()
    {
        yield return new WaitForSeconds(1f);

        puzzleControls.SetActive(false);
    }

    /// <summary>
    /// If puzzle is completed, activate a mechanism and continue with the map
    /// </summary>
    IEnumerator CompletePuzzle()
    {
        LevelManager.Instance.CompletedPuzzle();

        yield return new WaitForSeconds(0.5f);

        AudioLevelManager.Instance.PlayPuzzleAudio();

        isCorrect = true;
        
        MoveRocks();

        ActivateCamera.Instance.DisableCamera(numCamera);

        foreach (var player in playersList)
        {
            player.GetComponent<PlayerMovement>().canMove = true;
        }

        playersList.Clear();

        AudioLevelManager.Instance.PlayChangeClipAudio(AudioLevelManager.Instance.musicClip);
    }

    /// <summary>
    /// Activate a mechanism that will move rocks to cross a lake
    /// </summary>
    void MoveRocks()
    {
        newPosition2 = rockList.transform.position;

        AudioLevelManager.Instance.PlayRotatePuzzleClipAudio();

        rockList.transform.position = newPosition;
        waterList.transform.position = newPosition2;
    }
}