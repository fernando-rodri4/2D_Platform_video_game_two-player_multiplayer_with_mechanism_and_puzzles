using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Networking;

public class PuzzleRocas : NetworkBehaviour
{
    PuzzleRocas() { }

    /// <summary>
    /// This class holds a static reference to itself to ensure that there will only be
    /// one in existence. This is often referred to as a "singleton" design pattern.
    /// </summary>
    public static PuzzleRocas Instance = null;

    /// <summary>
    /// The layer the player game object is on
    /// </summary>
    int playerLayer;

    List<GameObject> playersList;

    GameObject currentPlayer;

    bool isCorrect = false;
    bool startPuzzle = false;
    int activeForPlayer1 = 0, activeForPlayer2 = 1;

    /// <summary>
    /// Puzzle pictures
    /// </summary>
    [SerializeField] GameObject[] pieces;
    [SerializeField] GameObject[] piecesAuthority;
    [SerializeField] GameObject puzzleControls;

    public Camera mainCamera;

    public int numCamera = 1;

    public GameObject rockList;
    public GameObject waterList;
    public Vector3 newPosition;
    Vector3 newPosition2;

    void Awake()
    {
        //If an PuzzleRocas exists and it is not this...
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Error with PuzzleRocas script components, 2 instances " + this);
            //...destroy this and exit. There can be only one PuzzleRocas
            Destroy(gameObject);
            return;
        }

        //This is the Instance PuzzleRocas and it should persist
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (pieces == null || puzzleControls == null)
        {
            Debug.LogError("Error with PuzzleRocas script component " + this);
            Destroy(this);
            return;
        }

        //AudioLevelManager.Instance.PlayChangeClipAudio(AudioLevelManager.Instance.puzzleClip);

        //Get the integer representation of the "Player" layer
        playerLayer = LayerMask.NameToLayer("Player");

        playersList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCorrect || playersList.Count == 0)
        {
            return;
        }

        if (playersList.Count == 2 && !startPuzzle)
        {
            Camera.main.orthographic = true;
            mainCamera.orthographicSize = 10.0f;

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
                    foreach (var item in piecesAuthority)
                    {
                        item.GetComponent<NetworkIdentity>().AssignClientAuthority(player.GetComponent<NetworkIdentity>().clientAuthorityOwner);
                    }
                }
            }

            startPuzzle = true;

            StartCoroutine(FinishControls());
        }

        if(pieces[0].GetComponent<Pieces>().isCorrect() && pieces[1].GetComponent<Pieces>().isCorrect() && pieces[2].GetComponent<Pieces>().isCorrect() &&
           pieces[3].GetComponent<Pieces>().isCorrect() && pieces[4].GetComponent<Pieces>().isCorrect() && pieces[5].GetComponent<Pieces>().isCorrect() &&
           pieces[6].GetComponent<Pieces>().isCorrect() && pieces[7].GetComponent<Pieces>().isCorrect() && pieces[8].GetComponent<Pieces>().isCorrect() &&
           pieces[9].GetComponent<Pieces>().isCorrect() && pieces[10].GetComponent<Pieces>().isCorrect() && pieces[11].GetComponent<Pieces>().isCorrect() &&
           pieces[12].GetComponent<Pieces>().isCorrect() && pieces[13].GetComponent<Pieces>().isCorrect() && pieces[14].GetComponent<Pieces>().isCorrect() &&
           pieces[15].GetComponent<Pieces>().isCorrect())
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

    IEnumerator FinishControls()
    {
        yield return new WaitForSeconds(1f);

        puzzleControls.SetActive(false);
    }

    IEnumerator CompletePuzzle()
    {
        yield return new WaitForSeconds(0.5f);

        AudioLevelManager.Instance.PlayPuzzleAudio();

        isCorrect = true;

        MoveRocks();

        Camera.main.orthographic = false;

        ActivateCamera.Instance.DisableCamera(numCamera);

        foreach (var player in playersList)
        {
            player.GetComponent<PlayerMovement>().canMove = true;
        }

        playersList.Clear();

        AudioLevelManager.Instance.PlayChangeClipAudio(AudioLevelManager.Instance.musicClip);
    }

    void MoveRocks()
    {
        newPosition2 = rockList.transform.position;

        AudioLevelManager.Instance.PlayRotatePuzzleClipAudio();

        rockList.transform.position = newPosition;
        waterList.transform.position = newPosition2;
    }
}
