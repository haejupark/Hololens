using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Networking;

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
	}
    protected string text;
	WWW www;
    // Use this for initialization
	void Start() {
		//StartCoroutine (Upload ());
	}
	void Awake()
	{
		text = "";
	}

	IEnumerator SendmyData(string text){
		WWWForm w = new WWWForm ();
		w.AddField ("unity", text);


		UnityWebRequest www = UnityWebRequest.Post ("", w);
		yield return www.Send();

		TextMesh answerObject = GameObject.Find("answer").GetComponent<TextMesh>();
		var ourText = www.downloadHandler.text;
		Debug.Log (ourText);
		var myObject = JsonUtility.FromJson<MyClass> (ourText);
		Debug.Log (myObject.response_smalltalk);
		//Debug.Log (json);
	
		//string response_smalltalk = ourText.Substring(;

		answerObject.text = myObject.response_smalltalk;	

	}


    public void SendText(string text)
    {
        //TextMesh answerObject = GameObject.Find("answer").GetComponent<TextMesh>();
		StartCoroutine (SendmyData (text));
		//answerObject.text = "";
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
