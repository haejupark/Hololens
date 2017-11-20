using UnityEngine;
using System;
using System.Net;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

public class ChatBotIO : MonoBehaviour {

	protected string text,answer;

    private Socket m_Socekt;
    public string IPAdress = "165.132.106.225";
    public const int port = 1024;
    public string username = "yonsei";
    public string bot = "harry";

    private int SenddataLength;
    private int ReceivedataLength;

    private byte[] Sendbyte;
    private byte[] Receivebyte = new byte[20000];
    private string ReceiveString;


	private readonly Queue<Action> ExecuteOnMainThread = new Queue<Action>();

    private void Awake()
    {
        m_Socekt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        m_Socekt.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 10000);
        m_Socekt.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 10000);

        try
        {
            IPAddress ipAddr = System.Net.IPAddress.Parse(IPAdress);
            IPEndPoint ipEndPoint = new System.Net.IPEndPoint(ipAddr, port);
            m_Socekt.Connect(ipEndPoint);
            Debug.Log("Socket connected!");
        }
        catch (SocketException SCE)
        {
            Debug.Log("Socket connect error! : " + SCE.ToString());
        }

    }
	// Use this for initialization
	void Start()
	{
		text = "";
    }

	// Update is called once per frame
	void Update()
	{
		// dispatch stuff on main thread
		while (ExecuteOnMainThread.Count > 0)
		{
			ExecuteOnMainThread.Dequeue().Invoke();
		}
	}

	private void RunInMainThread(Action action)
	{
		ExecuteOnMainThread.Enqueue(action);
	}

	public void PluginInit()
	{

	}

	public void SendText(string text)
	{
		try
		{
            StringBuilder sb = new StringBuilder(); // String Builder Create
            sb.Append((username + (char)0 + bot + (char)0 + text + (char)0));

            Debug.Log(text);

            SenddataLength = Encoding.Default.GetByteCount(sb.ToString());
            Sendbyte = Encoding.Default.GetBytes(sb.ToString());
            m_Socekt.Send(Sendbyte, Sendbyte.Length, 0);

            m_Socekt.Receive(Receivebyte);
            ReceiveString = Encoding.Default.GetString(Receivebyte);
            ReceivedataLength = Encoding.Default.GetByteCount(ReceiveString.ToString());
            Debug.Log("Receive Data : " + ReceiveString + "(" + ReceivedataLength + ")");

            TextMesh answerObject = GameObject.Find("answer").GetComponent<TextMesh>();
            answerObject.text = ReceiveString;

		} 
		catch (Exception ex)
		{
			Debug.LogException(ex);
		}

	}
    private void OnApplicationQuit()
    {
        m_Socekt.Close();
        m_Socekt = null;
    }


    void OnGUI()
	{
		int LABEL_WIDTH = 90;
		int EDIT_WIDTH = 250;


		text = GUI.TextArea(new Rect(10, Screen.height - 60, EDIT_WIDTH + LABEL_WIDTH, 50), text, 512);
		if (text.Contains ("\n") || text.Contains ("...")) {
	//		StartCoroutine (PandoraBotRequestCoRoutine (text)); 
			SendText (text);
			text = "";
		}
		GUI.Label(new Rect(10, Screen.height - 80, 300, 20), "Say: ");
				
	}
}
