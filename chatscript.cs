using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;


public class ChatBotIO : MonoBehaviour
{

    protected string text;

    private Socket m_Socekt;

    public string IPAdress = "165.132.106.225";
    public const int port = 1024;
    public string username = "yonsei";
    public string bot = "harry";

    private int SenddataLength;
    private int ReceivedataLength;

    private byte[] Sendbyte;
    private byte[] Receivebyte = new byte[2000];
    private string ReceiveString;

    // Use this for initialization
    void Awake()
    {
        text = "";
    }

    public void SendText(string text)
    {
        m_Socekt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ipAddr = IPAddress.Parse(IPAdress);
        IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);
        try
        {
            m_Socekt.Connect(ipEndPoint);
            Debug.Log("Socket connected!");
        }
        catch (SocketException SCE)
        {
            Debug.Log("Socket connect error!");
            Debug.Log(SCE.ToString());
        }

        try
        {
            StringBuilder sb = new StringBuilder(); // String Builder Create
            sb.Append((username + (char)0 + bot + (char)0 + text + (char)0));

            //send
            SenddataLength = Encoding.Default.GetByteCount(sb.ToString());
            Sendbyte = Encoding.Default.GetBytes(sb.ToString());
            m_Socekt.Send(Sendbyte, Sendbyte.Length, 0);
            //receive
            m_Socekt.Receive(Receivebyte); 
            ReceiveString = Encoding.Default.GetString(Receivebyte);
            ReceivedataLength = Encoding.Default.GetByteCount(ReceiveString.ToString());
            Debug.Log("Receive Data : " + ReceiveString + "(" + ReceivedataLength + ")");

            TextMesh answerObject = GameObject.Find("answer").GetComponent<TextMesh>();
            answerObject.text = ReceiveString;

            m_Socekt.Close();
        }
        catch (SocketException ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    void OnGUI()
    {
        int LABEL_WIDTH = 90;
        int EDIT_WIDTH = 250;


        text = GUI.TextArea(new Rect(10, Screen.height - 60, EDIT_WIDTH + LABEL_WIDTH, 50), text, 512);
        if (text.Contains("\n") || text.Contains("..."))
        {
            SendText(text);  
            text = "";
        }
        GUI.Label(new Rect(10, Screen.height - 80, 300, 20), "Say: ");

    }

}
