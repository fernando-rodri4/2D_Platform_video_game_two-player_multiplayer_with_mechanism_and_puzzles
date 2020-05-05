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

    /// <summary>
    /// Variables of the images
    /// </summary>
    public int row, col, countStep;
    public int rowBlank, colBlank;
    public int rowBlank1, colBlank1;
    public int sizeRow, sizeCol;
    int countPoint = 0;
    int countImagePoint = 0;
    int countComplete = 0;

    GameObject temp;    //Auxiliar variable to sort images

    public bool startControl = false;   //If the image can be moved or not
    public bool checkComplete;  //Check if pieces are completed
    public bool gameIsComplete; //If puzzle is completed or not

    bool startPuzzle = false;   //If the puzzle has started or not

    /// <summary>
    /// List of images used for the puzzle
    /// </summary>
    public List<GameObject> imageKeyList;   //run from 0 -> list.count
    public List<GameObject> imagePuzzleList;
    public List<GameObject> checkPointList;

    [SerializeField] GameObject puzzleControls; //Controls of the puzzle

    /// <summary>
    /// Matrix of images used for the puzzle
    /// </summary>
    GameObject[,] imageKeyMatrix;
    GameObject[,] imagePuzzleMatrix;
    GameObject[,] checkPointMatrix;

    /// <summary>
    /// The layer the player game object is on
    /// </summary>
    int playerLayer;

    /// <summary>
    /// List of players
    /// </summary>
    List<GameObject> playersList;

    /// <summary>
    /// Player that is playing the puzzle
    /// </summary>
    GameObject currentPlayer;

    [SerializeField] GameObject[] picturesAuthority;    //Pictures with authority

    public Camera mainCamera;   //Main camera used for the puzzle

    public GameObject rockList;
    public Vector3 newPosition; //New position of the rocks

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

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {

        if (imageKeyList == null || imagePuzzleList == null || checkPointList == null)
        {
            Debug.LogError("Error with PuzzleAzteca script component " + this);
            Destroy(this);
            return;
        }

        LevelManager.Instance.RegisterPuzzle();

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
                }

                if(imagePuzzleMatrix[r,c].name.CompareTo("blank1") == 0){
                    rowBlank1=r;
                    colBlank1=c;
                }
            }
        }  
    }

    /// <summary>
    /// Set the images to check the correct position
    /// </summary>
    void CheckPointManager()
    {
        for(int r=0; r<sizeRow; r++){   //run rows

            for(int c=0; c<sizeCol; c++){   //run columns

                checkPointMatrix[r,c] = checkPointList[countPoint];
                countPoint++;
            }
        }  
    }

    /// <summary>
    /// Set the images that will be the reference to complete the puzzle
    /// </summary>
    void ImageKeyManager()
    {
        for(int r=0; r<sizeRow; r++){   //run rows

            for(int c=0; c<sizeCol; c++){   //run columns

                imageKeyMatrix[r,c] = imageKeyList[countImagePoint];
                countImagePoint++;
            }
        }  
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {

        if(gameIsComplete || playersList.Count == 0){
            return;
        }

        //Prepare the puzzle
        if (playersList.Count == 1 && !startPuzzle)
        {

            Camera.main.orthographic = true;
            mainCamera.orthographicSize = 10.0f;

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

        if(startControl){   //move for image of puzzle

            startControl = false;
        }

        if(countStep==1){

            //Check pieces near blank to sort them
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
                else{

                    countStep = 0; //not move
                }
            }
            
            //Check pieces near blank1 to sort them
            if(imagePuzzleMatrix[row, col]!=null && imagePuzzleMatrix[row, col].name.CompareTo("blank1")!=0){

                if(rowBlank1!=row && colBlank1==col){

                    if(Mathf.Abs(row-rowBlank1) == 1){   //move 1 cell
                        //move
                        SortImage1(); //call function ImageSort
                        countStep = 0;  //reset countStep
                    }
                    else{
                        countStep = 0;  //reset countStep
                    }
                }
                else if(rowBlank1==row && colBlank1!=col){

                   if(Mathf.Abs(col-colBlank1) == 1){   //move 1 cell
                        //move
                        SortImage1(); //call function ImageSort
                        countStep = 0;
                    }
                    else{
                        countStep = 0;  //reset countStep
                    }
                }
                else{

                    countStep = 0; //not move
                }
            }
        }

        //If game is completed, call CompletePuzzle
        if (gameIsComplete)
        {
            StartCoroutine(CompletePuzzle());
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

    /// <summary>
    /// Sort image for player 2
    /// </summary>
    void SortImage(){

        if(hasAuthority)
        {
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
    }

    /// <summary>
    /// Sort image for player 1
    /// </summary>
     void SortImage1(){

        if(hasAuthority)
        {
            temp = imagePuzzleMatrix[rowBlank1,colBlank1];
            imagePuzzleMatrix[rowBlank1, colBlank1] = null;

            imagePuzzleMatrix[rowBlank1,colBlank1] = imagePuzzleMatrix[row,col];

            imagePuzzleMatrix[row,col] = null;
            imagePuzzleMatrix[row,col] = temp;

            //set move for image
            imagePuzzleMatrix[rowBlank1,colBlank1].GetComponent<ImageController>().target = checkPointMatrix[rowBlank1,colBlank1];    //set new point for image blank
            imagePuzzleMatrix[row, col].GetComponent<ImageController>().target = checkPointMatrix[row, col];

            imagePuzzleMatrix[rowBlank1,colBlank1].GetComponent<ImageController>().startMove = true;
            imagePuzzleMatrix[row, col].GetComponent<ImageController>().startMove = true;

            //set row and col for image blank
            rowBlank1 = row; //position touch
            colBlank1 = col;
        }
    }

    /// <summary>
    /// Set the images of the puzzle
    /// </summary>
    void ImagePuzzleManager()
    {
        //first row
        imagePuzzleMatrix[0,0] = imagePuzzleList[0];
        imagePuzzleMatrix[0,1] = imagePuzzleList[1];
        imagePuzzleMatrix[0,2] = imagePuzzleList[2];
        imagePuzzleMatrix[0,3] = imagePuzzleList[3];
        //second row
        imagePuzzleMatrix[1,0] = imagePuzzleList[4];
        imagePuzzleMatrix[1,1] = imagePuzzleList[5];
        imagePuzzleMatrix[1,2] = imagePuzzleList[6];
        imagePuzzleMatrix[1,3] = imagePuzzleList[7];
        //third row
        imagePuzzleMatrix[2,0] = imagePuzzleList[8];
        imagePuzzleMatrix[2,1] = imagePuzzleList[9];
        imagePuzzleMatrix[2,2] = imagePuzzleList[10];
        imagePuzzleMatrix[2,3] = imagePuzzleList[11];
        //fourth row
        imagePuzzleMatrix[3,0] = imagePuzzleList[12];
        imagePuzzleMatrix[3,1] = imagePuzzleList[13];
        imagePuzzleMatrix[3,2] = imagePuzzleList[14];
        imagePuzzleMatrix[3,3] = imagePuzzleList[15];
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
    /// Disable controls of the puzzle
    /// </summary>
    IEnumerator FinishControls()
    {
        yield return new WaitForSeconds(1f);

        puzzleControls.SetActive(false);
    }

    /// <summary>
    /// If puzzle is completed, players can continue with the map
    /// </summary>
    IEnumerator CompletePuzzle()
    {
        LevelManager.Instance.CompletedPuzzle();

        yield return new WaitForSeconds(0.5f);

        AudioLevelManager.Instance.PlayPuzzleAudio();

        gameIsComplete = true;

        MoveRocks();

        Camera.main.orthographic = false;

        ActivateCamera.Instance.DisableCamera(2);

        foreach (var player in playersList)
        {
            player.GetComponent<PlayerMovement>().canMove = true;
        }

        playersList.Clear();

        AudioLevelManager.Instance.PlayChangeClipAudio(AudioLevelManager.Instance.musicClip);
    }

    /// <summary>
    /// Activate the mechanism that will move a rock to cross
    /// </summary>
    void MoveRocks()
    {

        AudioLevelManager.Instance.PlayRotatePuzzleClipAudio();

        rockList.transform.position = newPosition;
    }
}
