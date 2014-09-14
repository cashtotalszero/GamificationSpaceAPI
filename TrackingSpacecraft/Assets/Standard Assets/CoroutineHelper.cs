using UnityEngine;
using System.Collections;

public class CoroutineHelper : MonoBehaviour {

	private WWW response;
	private string sometext;

	public WWW runCoroutine(string url) {
		//Debug.Log("Start coroutine");
		StartCoroutine(download(url));
		return response;
	}

	IEnumerator download (string atlasName)
	{
		//Debug.Log("In coroutine");
		WWW url = new WWW (atlasName);
		//Debug.Log("Before yield");
		if(url.error!=null&&url.text!=null) {
			sometext = url.text;
			response = url;
			//Debug.Log("After globals set");
		}
		yield return url;
		//Debug.Log("After yield");
		//sometext = url.text;
		//response = url;

	}
	
}
