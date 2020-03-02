using UnityEngine.Networking;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;

public class IpAddress : NetworkBehaviour
{

    public Text IPtext;
    public string localIP = "";

    // Update is called once per frame
    void Update()
    {

        IPHostEntry host;
        host = Dns.GetHostEntry(Dns.GetHostName());

        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;

            }
        }

        IPtext.text = localIP;

    }
}
