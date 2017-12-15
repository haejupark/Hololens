using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Linq;

public class ChatBotIO : MonoBehaviour
{
	public class MyClass
	{
		public string spo_baseline;
		public string spo_ours;
		public string response_base;
		public string response_ours;
		public string response_smalltalk;
		public bool isSmalltalk;
		public string sentiment;
		public List<int> senti_base;
		public List<int> senti_small;
		public List<int> senti_query;
	}
	// emotion
	// "anger", "anticipation", "disgust", "fear", "joy", "sadness", "surprise", "trust"

    protected string text;
	WWW www;

	void Start() { }
	void Awake()
	{
		text = "";
	}

	IEnumerator SendmyData(string text){
		WWWForm w = new WWWForm ();
		w.AddField ("unity", text);


		UnityWebRequest www = UnityWebRequest.Post ("url", w);
		yield return www.Send();

		TextMesh answerObject = GameObject.Find("answer").GetComponent<TextMesh>();
		var ourText = www.downloadHandler.text;
		Debug.Log (ourText);
		var myObject = JsonUtility.FromJson<MyClass> (ourText);

		if (myObject.isSmalltalk == true) {
			answerObject.text = myObject.response_smalltalk;
		} else {
			answerObject.text = myObject.response_base;
		}
	
	}


    public void SendText(string text)
    {
		StartCoroutine (SendmyData (text));
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
