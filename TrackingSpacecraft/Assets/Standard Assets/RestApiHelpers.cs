using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading;

public class RestApiHelpers : MonoBehaviour {

	private static string baseURL = "http://pocketspacecraft-dev.dyndns.org:3002";

	//private static string baseFileURL = "http://underrun.org/~gadget";

	private static string client_id = "ccab3a46aa33866a2ca6bc37dd848c2efe5ffce3c3c9feb4b76d9f61bf525253";
	private static string client_secret = "ff3825fa72113749f2e647a491bd0d5ec4a8c66efba73efd072072617cf1781f";
	
	private static string token;
	private static string token_type;
	private static DateTime expires_at = DateTime.MinValue; 
	
	private static NameValueCollection toPost = new NameValueCollection();	
	
	//private static CoroutineHelper coroutineHelper;

	private static string sometext;
	private static string error;
	public static WWW globalWWW;

	//private static RestApiHelpers Instance;

	//Add some data to be POSTed
	public static void AddField(string key, string val) {
		toPost.Add(key, val);
	}
	
	//Add some data to be POSTed
	public static void AddField(string key, int val) {
		toPost.Add(key, val.ToString());
	}

	private static IEnumerator postDownload(string atlasName, byte[] data, Hashtable headers){
		WWW url = new WWW (atlasName, data, headers);
		yield return url;
		//sometext = url.text; 
		//error = url.error;
		globalWWW = url;
	}

	public  IEnumerator httpPost(string url) {

		Debug.LogWarning("In HTTP POST");

		yield return StartCoroutine(updateToken());

		Hashtable headers = new Hashtable();
		headers.Add ("Content-Type", "application/json");
		headers.Add ("Authorization", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(token_type) + " " + token);

		yield return StartCoroutine(postDownload(baseURL + url, System.Text.Encoding.UTF8.GetBytes(postDataToJson().ToString()), headers));

	}
	
	private IEnumerator updateToken() {

		if (DateTime.Now > expires_at) {

			yield return StartCoroutine(refreshToken());
		}
		yield return null;
	}

	private static IEnumerator downloadRefreshToken(string atlasName, byte[] json, Hashtable headers){
		WWW url = new WWW (atlasName, json, headers);

		yield return url;
		//sometext = url.text; 

		//error = url.error;

		var textAsset = Resources.Load("token") as TextAsset;
		sometext = textAsset.text;


		globalWWW = url;
	}

	private IEnumerator refreshToken () {

		NameValueCollection oldToPostData = toPost;
		toPost = new NameValueCollection();
		
		AddField("grant_type", "client_credentials");
		AddField("client_id", client_id);
		AddField("client_secret", client_secret);
		
		string requestJson = postDataToJson().ToString();
		Hashtable headers = new Hashtable();
		headers.Add ("Content-Type", "application/json");

			yield return StartCoroutine(downloadRefreshToken(baseURL + "/oauth/token", System.Text.Encoding.UTF8.GetBytes(requestJson), headers));
			
			if (error == null) {
				Dictionary<string, string> resp_json = new JSONObject(sometext).ToDictionary();
				token = resp_json["access_token"];
				token_type = resp_json["token_type"];
				
				int expires_in = int.Parse(resp_json["expires_in"]);
				expires_at = DateTime.Now.AddSeconds(expires_in);
			}
			else{
				Debug.Log (error);
		}
		toPost = oldToPostData;
	}
			
	

	// Convert the toPost data to a json object and return it
	private static JSONObject postDataToJson() {
		JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
		
		foreach(string key in toPost.AllKeys) {
			json.AddField(key, toPost[key]);
		}
		toPost.Clear();
		
		return json;
	}

}
