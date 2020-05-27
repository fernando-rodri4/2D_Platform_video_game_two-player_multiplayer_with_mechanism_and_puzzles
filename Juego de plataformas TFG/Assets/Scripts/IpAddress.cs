using UnityEngine.Networking;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;

public class IpAddress : NetworkBehaviour
{

    public Text IPtext; //Text that will show the IP
    public string localIP = ""; //Local IP

    /// <summary>
    /// //Update is called once per frame.
    /// </summary>
    void Update()
    {

        IPHostEntry host;
        host = Dns.GetHostEntry(Dns.GetHostName()); //Obtains the host 

        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork) //Find the IP that is used 
            {
                localIP = ip.ToString();
                break;

            }
        }

        IPtext.text = localIP;  //Now show the Local IP

    }
}
