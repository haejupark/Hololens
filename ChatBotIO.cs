 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatBotIO : MonoBehaviour {

	protected string text, response;
	protected string sessionId;
	protected bool waiting;

	public string botid = "";
	public string appid = "";
	public string userkey = "";


	// Use this for initialization
	void Start () {
		text = "";
		response = "Waiting for text";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	string sanitizePandoraResponse(string wwwText)
	{
		string responseString = "";

		int startIndex = wwwText.IndexOf (" [") + 2;
		int endIndex = wwwText.IndexOf ("].");
		responseString = wwwText.Substring (startIndex, endIndex - startIndex);

		Debug.Log ("Sanitized response: " + responseString);
		return responseString;
	}
	void getSessionIdOfPandoraResponse(string wwwText)
	{
		int startIndex = wwwText.IndexOf ("sessionId") + 12;
		int endIndex = wwwText.IndexOf ("]") - 1;

		sessionId = wwwText.Substring (startIndex, endIndex - startIndex);
	}

	private IEnumerator PandoraBotRequestCoRoutine(string text)
	{
		//waiting = true;
		string url = "https://aiaas.pandorabots.com/talk/" + appid;
		url = url + "/" + botid;
		url = url + "?input=" + WWW.EscapeURL (text);
		if (sessionId != null) {
			url = url + "&sessionID=" + sessionId;
		}
		url = url + "&user_key=" + userkey;

		//Debug.Log (url);
		WWW www = new WWW(url, new byte[](0));

		yield return www;

		if(www.error == null)
		{
			//Debug.Log(www.text);
			getSessionIdOfPandoraResponse(www.text);
			//Debug.Log("SessionId:" + sessionId + ".");

			response = sanitizePandoraResponse(www.text);
		}
		else
		{
			Debug.LogWarning(www.error);
		}
	}
	void OnGUI()
	{
		int LABEL_WIDTH = 90;
		int EDIT_WIDTH = 250;

		text = GUI.TextArea(new Rect(10, Screen.height - 60, EDIT_WIDTH + LABEL_WIDTH, 50), text, 512);

		if (text.Contains ("\n") || text.Contains ("...")) {
			StartCoroutine (PandoraBotRequestCoRoutine (text));
			text = ""; 
		}
		GUI.Label(new Rect(10, Screen.height - 80, 300, 20), "Type something below and hit enter!");
				
	}
}
