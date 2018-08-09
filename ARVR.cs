using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using HoloToolkit.Unity.InputModule;
using System.Collections.Generic;
using UnityEngine.Networking;

public class arvr : MonoBehaviour, IInputClickHandler, IInputHandler {
	UnityEngine.TouchScreenKeyboard keyboard;

	public static string keyboardText = "";
	public class MyClass
	{
		public string query;
		public string query_sentiment;
		public string answer;
		public Sentiment answer_sentiment;
		public string characterName;
		public string objectName;
	}
	public class Queryii
	{
		public string query;
		public string characterName;
		public string objectName;
	}
	public class Sentiment
	{
		public string answer_class;
		public float intensity;
	}
	// emotion
	WWW www;

	void Start() { }
	void Awake()
	{
		//keyboard = TouchScreenKeyboard.Open ("", TouchScreenKeyboardType.Default, false, false, false, false);
	}

	IEnumerator SendmyData(Queryii my_query){
		WWWForm w = new WWWForm ();
		w.AddField ("query", my_query.query);
		w.AddField ("characterName", my_query.characterName);
		w.AddField ("objectName", my_query.objectName);

		UnityWebRequest www = UnityWebRequest.Post ("address", w);
		yield return www.SendWebRequest();

		TextMesh answerObject = GameObject.Find("answer").GetComponent<TextMesh>();
		var ourText = www.downloadHandler.text;
		Debug.Log (ourText);

		MyClass myObject = new MyClass ();
		myObject = JsonUtility.FromJson<MyClass> (ourText);

		answerObject.text = myObject.answer;

	}

	public void OnInputClicked(InputClickedEventData eventData)
	{
		// AirTap code goes here
		Queryii my_query = new Queryii();
		my_query.query = "what is this?";
		my_query.characterName = "boy";
		my_query.objectName = "tulip";
		string my_query2 = JsonUtility.ToJson(my_query, true);
		StartCoroutine (SendmyData (my_query));
		Debug.Log (my_query2);
	}
	public void OnInputDown(InputEventData eventData)
	{ }
	public void OnInputUp(InputEventData eventData)
	{ }
	void Update()
	{
		if (Input.GetKey (KeyCode.C)) {
			gameObject.GetComponent<Renderer>().material.color = new Color(Random.Range(0,1f),Random.Range(0,1f),Random.Range(0,1f));
		}
	}
}
