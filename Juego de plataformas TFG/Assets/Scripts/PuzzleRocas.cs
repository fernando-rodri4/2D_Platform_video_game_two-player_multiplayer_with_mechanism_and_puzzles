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

    public GameObject selectedPiece;
    int OIL = 1;

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
    [SerializeField] Transform[] pieces = null;
    [SerializeField] GameObject[] piecesAuthority;
    [SerializeField] GameObject puzzleControls;

    public int numCamera = 1;

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
        if (pieces == null ||puzzleControls == null)
        {
            Debug.LogError("Error with PuzzleRocas script component " + this);
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
                    pieces[activeForPlayer1].localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    pieces[activeForPlayer2].localScale = new Vector3(1, 1, 1);
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
                    foreach (var item in piecesAuthority)
                    {
                        item.GetComponent<NetworkIdentity>().AssignClientAuthority(player.GetComponent<NetworkIdentity>().clientAuthorityOwner);
                    }
                }
            }

            startPuzzle = true;

            StartCoroutine(FinishControls());
        }

        if (isCorrect)
        {
            StartCoroutine(CompletePuzzle());
        }

        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit.transform.CompareTag("Puzzle"))
            {
                if(!hit.transform.GetComponent<Pieces>().inRightPosition)
                {
                    selectedPiece = hit.transform.gameObject;
                    selectedPiece.GetComponent<Pieces>().selected = true;
                    selectedPiece.GetComponent<SortingGroup>().sortingOrder = OIL;
                    OIL++;
                }
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            if(selectedPiece != null)
            {
                selectedPiece.GetComponent<Pieces>().selected = false;
                selectedPiece = null;
            }
        }

        if(selectedPiece != null)
        {
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedPiece.transform.position = new Vector3(mousePoint.x, mousePoint.y, 0);
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

        ActivateCamera.Instance.DisableCamera(numCamera);

        foreach (var player in playersList)
        {
            player.GetComponent<PlayerMovement>().canMove = true;
        }

        playersList.Clear();

        AudioLevelManager.Instance.PlayChangeClipAudio(AudioLevelManager.Instance.musicClip);
    }
}
