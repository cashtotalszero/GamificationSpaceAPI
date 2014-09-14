#pragma strict

static var supportsGyro : boolean;

// References to other scripts
private var gameScript : gameMain;
private var API : GamificationAPI1;

// Called once when script is first called
function Awake() {
      
    // Assign the referenced scripts
    gameScript = GameObject.Find("GameMain").GetComponent(gameMain);   
    API = GameObject.Find("Main Camera").GetComponent("GamificationAPI1");
              
    // Check whether device supports gyroscope
    supportsGyro = SystemInfo.supportsGyroscope;
    
    if (supportsGyro) {
  
  		// Intialise the rotation angles of the camera to match the device gyroscope  
       	transform.eulerAngles = API.intialiseGyro();
        
        // Ensure screen doesn't go to sleep
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    } 
    else {
    	Debug.Log("NO GYRO");
    }
}

function Update () {
    
    if (supportsGyro) {    
    
        // Only rotate camera if game is active
        if(gameScript.getARStatus()) {
        	transform.localRotation = API.getDeviceRotation();  
	    }
    }
}