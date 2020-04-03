using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkManager_Custom : NetworkManager
{
    public int chosenCharacter = 0;
    public GameObject[] characters;
    Select select = null;
    LevelOption level = null;
    public bool isServer = false;

    void Start(){

        select = GameObject.Find("player").GetComponent<Select>();
        level = GameObject.Find("level").GetComponent<LevelOption>();
        chosenCharacter = select.elect;
        ServerChangeScene(level.elect);
        Destroy(select.gameObject);
    }

    //subclass for sending network messages
    public class NetworkMessage : MessageBase
    {
        public int chosenClass;
    }

    public void StartupHost(){

        isServer = true;

        SetPort();
        NetworkManager.singleton.StartHost();
    }

    public void JoinGame(){

        SetIPAddress();
        SetPort();
        NetworkManager.singleton.StartClient();
    }

    void SetIPAddress(){

        string ipAddress = GameObject.Find("InputFieldIPAddress").transform.Find("Text").GetComponent<Text>().text;
        NetworkManager.singleton.networkAddress = ipAddress;
    }

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

    void SetupMenuSceneButtons(){

        GameObject.Find("ButtonStartHost").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonStartHost").GetComponent<Button>().onClick.AddListener(StartupHost);

        GameObject.Find("ButtonJoinGame").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonJoinGame").GetComponent<Button>().onClick.AddListener(JoinGame);
    }

    void SetupOtherSceneButtons(){

        GameObject.Find("QuitButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("QuitButton").GetComponent<Button>().onClick.AddListener(NetworkManager.singleton.StopHost);
    }

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

    public override void OnClientConnect(NetworkConnection conn)
    {
        NetworkMessage test = new NetworkMessage();
        test.chosenClass = chosenCharacter;
 
        ClientScene.AddPlayer(conn, 0, test);
    }
 
 
    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        NetworkServer.DestroyPlayersForConnection(conn);

        if (conn.lastError != NetworkError.Ok)
        {
            if (LogFilter.logError) { Debug.LogError("ServerDisconnected due to error: " + conn.lastError); }
        }
        Debug.Log("A client disconnected from the server: " + conn);
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        NetworkServer.SetClientReady(conn);
        Debug.Log("Client is set to the ready state (ready to receive state updates): " + conn);
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
        if (player.gameObject != null)
            NetworkServer.Destroy(player.gameObject);
    }

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
