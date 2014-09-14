#pragma strict
import System.IO;

private var gameScript : gameMain;
private var API : GamificationAPI1;

var issLine1 : String;
var issLine2 : String;
var tiangongLine1 : String;
var tiangongLine2 : String;
var uploadComplete : boolean = false;
var collectedData : String;
var uploadURL : String;
 
// Called once at the start of the program
function Start(){
	
	// Assign referenced scripts
	API = GameObject.Find("Main Camera").GetComponent("GamificationAPI1");
	gameScript = GameObject.Find("GameMain").GetComponent(gameMain);
	
	// Download the latest TLE information
	yield API.downloadTLE();
	
	// If any errors/problems use the last valid reading saved on the device
	if(String.IsNullOrEmpty(API.issLine1) ||
		String.IsNullOrEmpty(API.issLine2) ||
		String.IsNullOrEmpty(API.tiangongLine1) ||
		String.IsNullOrEmpty(API.tiangongLine2) 
		){
		API.loadLastTle();
	}
	// Otherwise save this updated TLE into the device as back up for next load
	else {
		API.saveLastTle();
	}
	
	// This data upload info is here as an example only - no active server at present
	collectedData = Application.persistentDataPath + "/downloaded.bytes";
	uploadURL = "http://192.168.178.29/php/upload.php";
	
	Debug.Log("ISS");
	Debug.Log(API.issLine1);
	Debug.Log(API.issLine2);
	Debug.Log("TIANGONG");
	Debug.Log(API.tiangongLine1);
	Debug.Log(API.tiangongLine2);	
}

// Called once per frame
function Update() {
	
	// ...save of downloaded data occurs here...
	
	// Upload all collected data to the server at the end of the game - commented out as the backend is not currently running
	if(gameScript.gameOver && !uploadComplete) {
		 //uploadDataToServer();
		 uploadComplete = true;
	}
}

