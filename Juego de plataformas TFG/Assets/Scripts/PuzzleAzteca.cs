using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PuzzleAzteca : NetworkBehaviour
{

    PuzzleAzteca() { }

    /// <summary>
    /// This class holds a static reference to itself to ensure that there will only be
    /// one in existence. This is often referred to as a "singleton" design pattern.
    /// </summary>
    public static PuzzleAzteca Instance = null;

    public int row, col, countStep;
    public int rowBlank, colBlank;
    public int sizeRow, sizeCol;
    int countPoint = 0;
    int countImagePoint = 0;
    int countComplete = 0;

    GameObject temp;

    public bool startControl = false;
    public bool checkComplete;
    public bool gameIsComplete;

    bool startPuzzle = false;

    public List<GameObject> imageKeyList;   //run from 0 -> list.count
    public List<GameObject> imagePuzzleList;
    public List<GameObject> checkPointList;

    [SerializeField] GameObject puzzleControls;

    GameObject[,] imageKeyMatrix;
    GameObject[,] imagePuzzleMatrix;
    GameObject[,] checkPointMatrix;

    /// <summary>
    /// The layer the player game object is on
    /// </summary>
    int playerLayer;

    List<GameObject> playersList;

    GameObject currentPlayer;

    int activeForPlayer1 = 0, activeForPlayer2 = 1;

    [SerializeField] GameObject[] picturesAuthority;

    void Awake()
    {
        //If an PuzzleAzteca exists and it is not this...
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Error with PuzzleAzteca script components, 2 instances " + this);
            //...destroy this and exit. There can be only one PuzzleAzteca
            Destroy(gameObject);
            return;
        }

        //This is the Instance PuzzleAzteca and it should persist
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        if (imageKeyList == null || imagePuzzleList == null || checkPointList == null)
        {
            Debug.LogError("Error with PuzzleAzteca script component " + this);
            Destroy(this);
            return;
        }

        //Get the integer representation of the "Player" layer
        playerLayer = LayerMask.NameToLayer("Player");

        playersList = new List<GameObject>();

        //Active the first image for each player
        var players = GameObject.FindGameObjectsWithTag("Player");

        imageKeyMatrix = new GameObject[sizeRow, sizeCol];
        imagePuzzleMatrix = new GameObject[sizeRow, sizeCol];
        checkPointMatrix = new GameObject[sizeRow, sizeCol];

        ImagePuzzleManager();
        CheckPointManager();
        ImageKeyManager();

        for(int r=0; r<sizeRow; r++){   //run rows

            for(int c=0; c<sizeCol; c++){   //run columns

                if(imagePuzzleMatrix[r,c].name.CompareTo("blank") == 0){
                    rowBlank=r;
                    colBlank=c;
                    break;
                }
            }
        }  
    }

    void CheckPointManager()
    {
        for(int r=0; r<sizeRow; r++){   //run rows

            for(int c=0; c<sizeCol; c++){   //run columns

                checkPointMatrix[r,c] = checkPointList[countPoint];
                countPoint++;
            }
        }  
    }

    void ImageKeyManager()
    {
        for(int r=0; r<sizeRow; r++){   //run rows

            for(int c=0; c<sizeCol; c++){   //run columns

                imageKeyMatrix[r,c] = imageKeyList[countImagePoint];
                countImagePoint++;
            }
        }  
    }

    // Update is called once per frame
    void Update()
    {

        if(gameIsComplete || playersList.Count == 0){
            return;
        }

        if (playersList.Count == 2 && !startPuzzle)
        {
            AudioLevelManager.Instance.PlayChangeClipAudio(AudioLevelManager.Instance.puzzleClip);
            ActivateCamera.Instance.EnableCamera(2);

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

        if (gameIsComplete)
        {
            StartCoroutine(CompletePuzzle());
        }

        if(startControl){   //move for image of puzzle

            startControl = false;
        }
        if(countStep==1){
            if(imagePuzzleMatrix[row, col]!=null && imagePuzzleMatrix[row, col].name.CompareTo("blank")!=0){

                if(rowBlank!=row && colBlank==col){

                    if(Mathf.Abs(row-rowBlank) == 1){   //move 1 cell
                        //move
                        SortImage(); //call function ImageSort
                        countStep = 0;  //reset countStep
                    }
                    else{
                        countStep = 0;  //reset countStep
                    }
                }
                else if(rowBlank==row && colBlank!=col){

                   if(Mathf.Abs(col-colBlank) == 1){   //move 1 cell
                        //move
                        SortImage(); //call function ImageSort
                        countStep = 0;
                    }
                    else{
                        countStep = 0;  //reset countStep
                    }
                }
                else if((rowBlank!=row && colBlank!=col) ||(rowBlank==row && colBlank==col)){

                    countStep = 0;  //not move
                }
                else{

                    countStep = 0; //not move
                }
            }
        }      
    }

    void FixedUpdate() {
        if(checkComplete){
            checkComplete=false;
            for(int r=0; r<sizeRow; r++){   //run rows

                for(int c=0; c<sizeCol; c++){   //run columns

                    if(imageKeyMatrix[r,c].gameObject.name.CompareTo(imagePuzzleMatrix[r,c].gameObject.name) == 0){
                        countComplete++;
                    }
                    else{
                        break;  //out loop
                    }
                }
            }
            if(countComplete == checkPointList.Count){  //if 16 imagePuzzle == 16 imageKey (in 2 array) (CheckPointList.Count=9)
                gameIsComplete=true;
                Debug.Log("Game Is Complete");
            }  
            else{
                countComplete=0;
            }
        }        
    }

    void SortImage(){

        temp = imagePuzzleMatrix[rowBlank,colBlank];
        imagePuzzleMatrix[rowBlank,colBlank] = null;

        imagePuzzleMatrix[rowBlank,colBlank] = imagePuzzleMatrix[row,col];

        imagePuzzleMatrix[row,col] = null;
        imagePuzzleMatrix[row,col] = temp;

        //set move for image
        imagePuzzleMatrix[rowBlank,colBlank].GetComponent<ImageController>().target = checkPointMatrix[rowBlank,colBlank];    //set new point for image blank
        imagePuzzleMatrix[row, col].GetComponent<ImageController>().target = checkPointMatrix[row, col];

        imagePuzzleMatrix[rowBlank,colBlank].GetComponent<ImageController>().startMove = true;
        imagePuzzleMatrix[row, col].GetComponent<ImageController>().startMove = true;

        //set row and col for image blank
        rowBlank = row; //position touch
        colBlank = col;
    }

    void ImagePuzzleManager()
    {
        //first row
        imagePuzzleMatrix[0,0] = imagePuzzleList[7];
        imagePuzzleMatrix[0,1] = imagePuzzleList[4];
        imagePuzzleMatrix[0,2] = imagePuzzleList[6];
        imagePuzzleMatrix[0,3] = imagePuzzleList[0];
        //second row
        imagePuzzleMatrix[1,0] = imagePuzzleList[2];
        imagePuzzleMatrix[1,1] = imagePuzzleList[3];
        imagePuzzleMatrix[1,2] = imagePuzzleList[1];
        imagePuzzleMatrix[1,3] = imagePuzzleList[5];
        //third row
        imagePuzzleMatrix[2,0] = imagePuzzleList[15];
        imagePuzzleMatrix[2,1] = imagePuzzleList[12];
        imagePuzzleMatrix[2,2] = imagePuzzleList[14];
        imagePuzzleMatrix[2,3] = imagePuzzleList[8];
        //fourth row
        imagePuzzleMatrix[3,0] = imagePuzzleList[10];
        imagePuzzleMatrix[3,1] = imagePuzzleList[11];
        imagePuzzleMatrix[3,2] = imagePuzzleList[9];
        imagePuzzleMatrix[3,3] = imagePuzzleList[13];
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

    IEnumerator FinishControls()
    {
        yield return new WaitForSeconds(1f);

        puzzleControls.SetActive(false);
    }

    IEnumerator CompletePuzzle()
    {
        yield return new WaitForSeconds(0.5f);

        AudioLevelManager.Instance.PlayPuzzleAudio();

        gameIsComplete = true;

        ActivateCamera.Instance.DisableCamera(2);

        foreach (var player in playersList)
        {
            player.GetComponent<PlayerMovement>().canMove = true;
        }

        playersList.Clear();

        AudioLevelManager.Instance.PlayChangeClipAudio(AudioLevelManager.Instance.musicClip);
    }
}
