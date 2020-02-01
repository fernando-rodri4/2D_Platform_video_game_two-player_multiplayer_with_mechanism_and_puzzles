using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Networking.NetworkSystem;

public class NetworkManager_Custom : NetworkManager
{
    public int chosenCharacter = 0;
    public GameObject[] characters;
    Select select = null;

    void Start(){

        select = GameObject.Find("player").GetComponent<Select>();
        chosenCharacter = select.elect;
    }

    //subclass for sending network messages
    public class NetworkMessage : MessageBase
    {
        public int chosenClass;
    }

    public void StartupHost(){

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
        //base.OnClientSceneChanged(conn);
    }
}
