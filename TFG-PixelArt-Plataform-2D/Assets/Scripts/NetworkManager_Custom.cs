using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkManager_Custom : NetworkManager
{
    public int chosenCharacter = 0; //Selected character
    public GameObject[] characters; //List of characters
    Select select = null;   
    LevelOption level = null;   //Level selected
    public bool isServer = false;   //If player is server or not


    
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start(){

        //Set the configuration for the network
        select = GameObject.Find("player").GetComponent<Select>();
        level = GameObject.Find("level").GetComponent<LevelOption>();
        chosenCharacter = select.elect;
        Destroy(select.gameObject);
        NetworkManager.singleton.onlineScene = level.elect;
    }

    /// <summary>
    /// Subclass for sending network messages
    /// </summary>
    public class NetworkMessage : MessageBase
    {
        public int chosenClass;
    }

    /// <summary>
    /// Start the host
    /// </summary>
    public void StartupHost(){

        isServer = true;

        SetPort();
        NetworkManager.singleton.StartHost();
    }

    /// <summary>
    /// Start a client that joins a host
    /// </summary>
    public void JoinGame(){

        SetIPAddress();
        SetPort();
        NetworkManager.singleton.StartClient();
    }

    /// <summary>
    /// Set the IP Address for the host
    /// </summary>
    void SetIPAddress(){

        string ipAddress = GameObject.Find("InputFieldIPAddress").transform.Find("Text").GetComponent<Text>().text;
        NetworkManager.singleton.networkAddress = ipAddress;
    }

    /// <summary>
    /// Set the port used for the network
    /// </summary>
    void SetPort(){

        NetworkManager.singleton.networkPort = 7777;
    }

    void OnLevelWasLoaded(int level){

        if(level == 0){

            SetupMenuSceneButtons();
        }
        else{

            SetupOtherSceneButtons();
        }
    }

    /// <summary>
    /// Set listener for the host and the client
    /// </summary>
    void SetupMenuSceneButtons(){

        GameObject.Find("ButtonStartHost").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonStartHost").GetComponent<Button>().onClick.AddListener(StartupHost);

        GameObject.Find("ButtonJoinGame").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonJoinGame").GetComponent<Button>().onClick.AddListener(JoinGame);
    }

    /// <summary>
    /// Stet listener to quit the game
    /// </summary>
    void SetupOtherSceneButtons(){

        GameObject.Find("QuitButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("QuitButton").GetComponent<Button>().onClick.AddListener(NetworkManager.singleton.StopHost);
    }

    /// <summary>
    /// Add a player to the server, join the game
    /// </summary>
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader) {

        NetworkMessage message = extraMessageReader.ReadMessage<NetworkMessage>();
        int selectedClass = message.chosenClass;
        Debug.Log("server add with message " + selectedClass);

        GameObject player;
        Transform startPos = GetStartPosition();

        if (startPos != null)
        {
            player = Instantiate(characters[selectedClass], startPos.position, startPos.rotation) as GameObject;
        }
        else
        {
            player = Instantiate(characters[selectedClass], Vector3.zero, Quaternion.identity) as GameObject;

        }
 
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    /// <summary>
    /// Set the scene for the client
    /// </summary>
    public override void OnClientConnect(NetworkConnection conn)
    {
        NetworkMessage test = new NetworkMessage();
        test.chosenClass = chosenCharacter;
 
        ClientScene.AddPlayer(conn, 0, test);
    }
 
    /// <summary>
    /// Change the scene for the client
    /// </summary>
    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);
    }

    /// <summary>
    /// Stop network if server is disconnected
    /// </summary>
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        NetworkServer.DestroyPlayersForConnection(conn);

        if (conn.lastError != NetworkError.Ok)
        {
            if (LogFilter.logError) { Debug.LogError("ServerDisconnected due to error: " + conn.lastError); }
        }
        Debug.Log("A client disconnected from the server: " + conn);
    }

    /// <summary>
    /// Server is ready for the network
    /// </summary>
    public override void OnServerReady(NetworkConnection conn)
    {
        NetworkServer.SetClientReady(conn);
        Debug.Log("Client is set to the ready state (ready to receive state updates): " + conn);
    }

    /// <summary>
    /// Remove a player from the network
    /// </summary>
    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
        if (player.gameObject != null)
            NetworkServer.Destroy(player.gameObject);
    }

    /// <summary>
    /// Stop the client that is disconnected
    /// </summary>
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        StopClient();
        Destroy(this.gameObject);
        Shutdown();

        if (conn.lastError != NetworkError.Ok)
        {
            if (LogFilter.logError) 
            {
                Debug.LogError("ClientDisconnected due to error: " + conn.lastError); 
            }
        }
        Debug.Log("Client disconnected from server: " + conn);
    }
}
