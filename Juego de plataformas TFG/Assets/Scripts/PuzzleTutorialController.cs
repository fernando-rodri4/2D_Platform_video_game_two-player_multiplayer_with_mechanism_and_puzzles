using System.Collections;
using UnityEngine;
using Cinemachine;

public class PuzzleTutorialController : MonoBehaviour
{
    PuzzleTutorialController() { }

    /// <summary>
    /// This class holds a static reference to itself to ensure that there will only be
    /// one in existence. This is often referred to as a "singleton" design pattern. Other
    /// scripts access this one through this instance.
    /// </summary>
    public static PuzzleTutorialController Instance = null;

    /// <summary>
    /// Puzzle pictures
    /// </summary>
    [SerializeField] Transform[] picturesBackground = null;

    /// <summary>
    /// Puzzle pictures
    /// </summary>
    [SerializeField] Transform[] pictures = null;

    /// <summary>
    /// Box that active final ladder when puzzle is correct
    /// </summary>
    [SerializeField] GameObject activeLadder = null;

    /// <summary>
    /// Camera that show the puzzle
    /// </summary>
    [SerializeField] PlayerMovement[] players = null;

    bool elementEnter = false;

    bool isCorrect = false;

    void Awake()
    {
        //If an UIManager exists and it is not this...
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Error with PuzzleTutorialController script components, 2 instances " + this);
            //...destroy this and exit. There can be only one PuzzleTutorialController
            Destroy(gameObject);
            return;
        }

        //This is the Instance PuzzleTutorialController and it should persist between scene loads
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (picturesBackground == null || pictures == null || activeLadder == null || players == null)
        {
            Destroy(this);
            Debug.LogError("Error with PuzzleTutorialController script component " + this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (elementEnter && Input.GetKeyDown(KeyCode.E))
        {
            ActivateCamera.Instance.ActivateCamera_(0);

            foreach (var item in players)
            {
                item.canMove = false;
            }
        }

        if (pictures[0].rotation.z == 0 && pictures[1].rotation.z == 0 && pictures[2].rotation.z == 0 && pictures[3].rotation.z == 0 &&
            pictures[4].rotation.z == 0 && pictures[5].rotation.z == 0 && pictures[6].rotation.z == 0 && pictures[7].rotation.z == 0)
        {
            isCorrect = true;
            picturesBackground[0].rotation = pictures[0].rotation;
            picturesBackground[1].rotation = pictures[1].rotation;
            picturesBackground[2].rotation = pictures[2].rotation;
            picturesBackground[3].rotation = pictures[3].rotation;
            picturesBackground[4].rotation = pictures[4].rotation;
            picturesBackground[5].rotation = pictures[5].rotation;
            picturesBackground[6].rotation = pictures[6].rotation;
            picturesBackground[7].rotation = pictures[7].rotation;

            ActivateCamera.Instance.DeactivateCamera(0);

            foreach (var item in players)
            {
                item.canMove = true;
            }

            StartCoroutine(ActiveFinalLadder());
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        elementEnter = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        elementEnter = false;
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
    /// Active final ladder
    /// </summary>
    /// <returns></returns>
    IEnumerator ActiveFinalLadder()
    {
        yield return new WaitForSeconds(1f);
        activeLadder.SetActive(true);
    }
}
