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

    List<GameObject> playersList;

    GameObject currentPlayer;

    bool isCorrect = false;

    bool startPuzzle = false;

    int activeForPlayer1 = 0, activeForPlayer2 = 1;

    [SerializeField] GameObject[] picturesAuthority;

    [SerializeField] GameObject puzzleControls;

    public GameObject rockList;
    int playerLayer;
    int sumPosY = 20;
    public int numCamera = 0;

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

        //AudioLevelManager.Instance.PlayChangeClipAudio(AudioLevelManager.Instance.puzzleClip);

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

        if (playersList.Count == 2 && !startPuzzle)
        {
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

        if ((pictures[0].rotation.z % 360) == 0 && (pictures[1].rotation.z % 360) == 0 && (pictures[2].rotation.z % 360) == 0 && (pictures[3].rotation.z % 360) == 0 &&
            (pictures[4].rotation.z % 360) == 0 && (pictures[5].rotation.z % 360) == 0 && (pictures[6].rotation.z % 360) == 0 && (pictures[7].rotation.z % 360) == 0 &&
            (pictures[8].rotation.z % 360) == 0 && (pictures[9].rotation.z % 360) == 0 && (pictures[10].rotation.z % 360) == 0 && (pictures[11].rotation.z % 360) == 0 &&
            (pictures[12].rotation.z % 360) == 0 && (pictures[13].rotation.z % 360) == 0 && (pictures[14].rotation.z % 360) == 0 && (pictures[15].rotation.z % 360) == 0 &&
            (pictures[16].rotation.z % 360) == 0 && (pictures[17].rotation.z % 360) == 0 && (pictures[18].rotation.z % 360) == 0 && (pictures[19].rotation.z % 360) == 0 &&
            (pictures[20].rotation.z % 360) == 0 && (pictures[21].rotation.z % 360) == 0 && (pictures[22].rotation.z % 360) == 0 && (pictures[23].rotation.z % 360) == 0 &&
            (pictures[24].rotation.z % 360) == 0 && (pictures[25].rotation.z % 360) == 0 && (pictures[26].rotation.z % 360) == 0 && (pictures[27].rotation.z % 360) == 0 &&
            (pictures[28].rotation.z % 360) == 0 && (pictures[29].rotation.z % 360) == 0 && (pictures[30].rotation.z % 360) == 0)
        {
            StartCoroutine(CompletePuzzle());
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer && !playersList.Contains(collision.gameObject) /*&& Input.GetButtonDown("Enter")*/) //Todo: Intentar poner confirmación de jugador
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
        else if ((Input.GetButtonDown("RotateRight") && currentPlayer.GetComponent<PlayerMovement>().GetId() == 1) || Input.GetButtonDown("Enter") && playersList[0].GetComponent<NetworkIdentity>() == null) //Todo:cambiar a enter2
        {
            pictures[activeForPlayer2].GetComponent<TouchRotate>().RotateRight();
            AudioLevelManager.Instance.PlayRotatePuzzleClipAudio();
        }

        if ((Input.GetButtonDown("RotateLeft") && currentPlayer.GetComponent<PlayerMovement>().GetId() == 0) || Input.GetButtonDown("Enter") && playersList[0].GetComponent<NetworkIdentity>() == null)
        {
            pictures[activeForPlayer1].GetComponent<TouchRotate>().RotateLeft();
            AudioLevelManager.Instance.PlayRotatePuzzleClipAudio();
        }
        else if ((Input.GetButtonDown("RotateLeft") && currentPlayer.GetComponent<PlayerMovement>().GetId() == 1) || Input.GetButtonDown("Enter") && playersList[0].GetComponent<NetworkIdentity>() == null) //Todo:cambiar a enter2
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

            if (activeForPlayer1 == 0 || activeForPlayer1 == 2 || activeForPlayer1 == 5 || activeForPlayer1 == 7 ||
                activeForPlayer1 == 12 || activeForPlayer1 == 14 || activeForPlayer1 == 18 || activeForPlayer1 == 20 ||
                activeForPlayer1 == 26 || activeForPlayer1 == 28)
            {
                activeForPlayer1 = (activeForPlayer1 + 2) % pictures.Length;
            }
            else if (activeForPlayer1 == 4 ||activeForPlayer1 == 16 || activeForPlayer1 == 17 || activeForPlayer1 == 25 ||
                     activeForPlayer1 == 30)
            {
                activeForPlayer1 = (activeForPlayer1 + 1) % pictures.Length;
            }
            else
            {
                activeForPlayer1 = (activeForPlayer1 + 3) % pictures.Length;
            }

            pictures[activeForPlayer1].localScale = new Vector3(5.09f, 5, 1);
        }
        else if ((Input.GetButtonDown("Enter") && currentPlayer.GetComponent<PlayerMovement>().GetId() == 1) || Input.GetButtonDown("Enter") && playersList[0].GetComponent<NetworkIdentity>() == null) //Todo:cambiar a enter2
        {

            pictures[activeForPlayer2].localScale = new Vector3(4.09f, 4, 1);
            AudioLevelManager.Instance.PlayChangePuzzleClipAudio();

            if (activeForPlayer2 == 1 || activeForPlayer2 == 6 || activeForPlayer2 == 8 ||
                activeForPlayer2 == 11 || activeForPlayer2 == 13 || activeForPlayer2 == 19 ||
                activeForPlayer2 == 21 || activeForPlayer2 == 27)
            {
                activeForPlayer2 = (activeForPlayer2 + 2) % pictures.Length;
            }
            else if (activeForPlayer2 == 10 || activeForPlayer2 == 23)
            {
                activeForPlayer2 = (activeForPlayer2 + 1) % pictures.Length;
            }
            else if (activeForPlayer2 == 15)
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

    IEnumerator FinishControls()
    {
        yield return new WaitForSeconds(1f);

        puzzleControls.SetActive(false);
    }

    IEnumerator CompletePuzzle()
    {
        yield return new WaitForSeconds(0.5f);

        AudioLevelManager.Instance.PlayPuzzleAudio();

        foreach (var picture in pictures)
        {
            picture.localScale = new Vector3(1, 1, 1);

            yield return new WaitForSeconds(0.5f);
        }

        isCorrect = true;

        ActivateCamera.Instance.DisableCamera(numCamera);

        foreach (var player in playersList)
        {
            player.GetComponent<PlayerMovement>().canMove = true;
        }

        playersList.Clear();

        // Active final ladder
        yield return new WaitForSeconds(1f);
        MoveRocks();

        AudioLevelManager.Instance.PlayChangeClipAudio(AudioLevelManager.Instance.musicClip);
    }

    IEnumerator MoveRocks()
    {
        yield return new WaitForSeconds(0.5f);

        AudioLevelManager.Instance.PlayRotatePuzzleClipAudio();

        Vector3 newPosition = rockList.transform.position;

        newPosition.y += sumPosY;

        rockList.transform.position = newPosition;
    }
}