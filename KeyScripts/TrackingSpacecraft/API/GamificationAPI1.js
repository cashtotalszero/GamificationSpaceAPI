#pragma strict
import System.IO;
import System.Math;

// Device sensors 
private var gps : LocationInfo;
private var gyro : Gyroscope;
private var accel : Vector3;
private var compassHeading : float;	

// Quaternion information for camera rotator
private var quatMult : Quaternion;
private var quatMap : Quaternion;

// TLE containers
var tleArray : String[];
public var issLine1 : String;
public var issLine2 : String;
public var tiangongLine1 : String;
public var tiangongLine2 : String;


function intialiseGyro() : Vector3 {

	gyro = Input.gyro;
    gyro.enabled = true;

	var eulerAngles;

	#if UNITY_IPHONE
		eulerAngles = Vector3(90,90,0);
		if (Screen.orientation == ScreenOrientation.LandscapeLeft) {
        	quatMult = Quaternion(0f,0,0.7071,0.7071);
        } else if (Screen.orientation == ScreenOrientation.LandscapeRight) {
        	quatMult = Quaternion(0,0,-0.7071,0.7071);
        } else if (Screen.orientation == ScreenOrientation.Portrait) {
        	quatMult = Quaternion(0,0,1,0);
        } else if (Screen.orientation == ScreenOrientation.PortraitUpsideDown) {
        	quatMult = Quaternion(0,0,0,1);
        }
	#endif
     
    #if UNITY_ANDROID
    	eulerAngles = Vector3(-90,0,0);
        if (Screen.orientation == ScreenOrientation.LandscapeLeft) {
        	quatMult = Quaternion(0f,0,0.7071,-0.7071);
        } else if (Screen.orientation == ScreenOrientation.LandscapeRight) {
            quatMult = Quaternion(0,0,-0.7071,-0.7071);
        } else if (Screen.orientation == ScreenOrientation.Portrait) {
            quatMult = Quaternion(0,0,0,1);
        } else if (Screen.orientation == ScreenOrientation.PortraitUpsideDown) {
            quatMult = Quaternion(0,0,1,0);
        }
    #endif

	return eulerAngles;
}

function getDeviceRotation() : Quaternion {

	#if UNITY_IPHONE
    	quatMap = gyro.attitude;
    #endif
        
    #if UNITY_ANDROID
        quatMap = Quaternion(gyro.attitude.w,gyro.attitude.x,gyro.attitude.y,gyro.attitude.z);
    #endif
    
    var rotation = quatMap * quatMult;
    return rotation;
}

// Downloads the latest TLE data from the internet
function downloadTLE () {
			
	// TLE information is published and regularly updated by NORAD
	var url : WWW = new WWW ("http://www.celestrak.com/NORAD/elements/stations.txt");
	yield url;
			
	// Provided there are no errors copy the tle data into a string
	if(String.IsNullOrEmpty(url.error)){
		var tleData = url.text;
	}
	// Else leave the TLE information blank
	else {
		return;
	}
					
	// Split into an array & extract the TLE
	tleArray = tleData.Split("\n"[0]);		
	var i :int = 0;
	var currentLine : String;
	while(i < tleArray.length) {
		currentLine = tleArray[i];
		currentLine = currentLine.Trim();
	
		// In this version we are using ISS and Tiangong as examples. Future versions would store entire database on a server.
		if (currentLine.StartsWith("ISS")) {
			saveTle("ISS",i);
		}
		else if (currentLine.StartsWith("TIANGONG")) {
			saveTle("TIANGONG",i);
		}
		i++;
	}
}

// Saves the TLE information into the program
function saveTle(scName : String, i : int) {

	var currentLine : String;
	var j : int;
	
	// Skip the spacecraft name
	i++;
	
	// Copy tle into current variables
	for(j=0; j<3;j++) {
		currentLine = tleArray[i];
		currentLine = currentLine.Trim();
		
		// Assign line 1...
		if (currentLine.StartsWith("1 ")) {
			if(scName == "ISS") {
				issLine1 = currentLine;
			}
			if(scName == "TIANGONG") {
				tiangongLine1 = currentLine;
			}
		}
		// ...and line 2
		else if (currentLine.StartsWith("2 ")) {
			if(scName == "ISS") {
				issLine2 = currentLine;
			}
			if(scName == "TIANGONG") {
				tiangongLine2 = currentLine;
			}
		}
		i++;
		j++;
	}
}

// Loads the last TLE information from file stored on device if unable to get latest info from online	
function loadLastTle() {
	Debug.Log("WARNING = ERROR LOADING LATEST TLE: BACKUP IN USE. DATA MAY BE OUT OF DATE.");
	
	var rootPath : String = Application.persistentDataPath;
	
	// On first load create access as text file - from resources (tleBackUp.txt)
	if (!File.Exists(rootPath + '/Cached/tleBackup.txt')) {	 	 	
		var accessStream : StreamWriter = File.CreateText(rootPath + '/Cached/tleBackup.txt');
    	var textAsset = Resources.Load("tleBackup") as TextAsset;
    	var text : String = textAsset.text;
    	accessStream.Write(text);
    	accessStream.Close();
	}
		
	// Otherwise just load the existing file into streamreader
    var sr : StreamReader = File.OpenText(rootPath + '/Cached/tleBackup.txt');
    text = sr.ReadLine();
    
    // Save the information into the program
    if (text.StartsWith("ISS")) {
		issLine1 = sr.ReadLine();
		issLine2 = sr.ReadLine();	
	}
	text = sr.ReadLine();
	if (text.StartsWith("TIANGONG")) {
		tiangongLine1 = sr.ReadLine();
		tiangongLine2 = sr.ReadLine();
	}
    sr.Close();
    
    /* 
    Debug.Log("FROM FILE: ");
  	Debug.Log("ISS");
	Debug.Log(issLine1);
	Debug.Log(issLine2);
	Debug.Log("TIANGONG");
	Debug.Log(tiangongLine1);
	Debug.Log(tiangongLine2);
	*/
}

// Saves the latest TLE information onto the device as a backup
function saveLastTle() {
	Debug.Log("TLE updated successfully");
	
	var rootPath : String = Application.persistentDataPath;
	var toWrite : String = "ISS\n"+issLine1+"\n"+issLine2+"\n"+"TIANGONG\n"+tiangongLine1+"\n"+tiangongLine2+"\n";
	
	// Create a file to hold the data if it doesnt already exist
	var accessStream : StreamWriter;
	if (!File.Exists(rootPath + '/Cached/tleBackup.txt')) {	 	 	
		accessStream = File.CreateText(rootPath + '/Cached/tleBackup.txt');
	}
	else {
		accessStream = new StreamWriter((rootPath + '/Cached/tleBackup.txt'),false);
	}
	// Update with the latest TLE information
	accessStream.Write(toWrite);
    accessStream.Close();
}

// Returns as a vector3 the TLE data for the requested sapcecraft at the given (current) timestamp
function getSpacecraftPosition(sc_id : int) : Vector3 {
			
	var ts = getCurrentUnix();
	var line1;
	var line2;
	
	if(sc_id == 0) {
		line1 = issLine1;
		line2 = issLine2;
	}
	else {
		line1 = tiangongLine1;
		line2 = tiangongLine2;
	}
	

	// Create reference to YAMTRAK script and send it the TLE data to calculate spacecraft position
	var YTScript = new YAMTRAK();
	YTScript.calculate(parseInt(ts), line1, line2);
	var coords = YTScript.GetPos();
			
	// Return position of the requested spacecraft
	return Vector3(coords[0], coords[1], coords[2]);
}

// Gets the current timestamp 	
function getCurrentUnix() : int {
	var unixStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
	return (System.DateTime.UtcNow - unixStart).TotalSeconds;
}

// Uploads any collected data from the satellite to an online server
function uploadDataToServer(uploadURL : String, collectedData : String) {
	
	// Prepare the data collected during the game
	var toUpload : WWW = new WWW (collectedData);
	yield toUpload;

	// Add it to a WWWForm to allow upload
	var postForm : WWWForm = new WWWForm();
	postForm.AddBinaryData("theFile",toUpload.bytes,collectedData,"text/plain");
 
 	// Upload it to the server
	var upload : WWW = new WWW(uploadURL,postForm);
	yield upload;		
}		




// Get the latest GPS reading for device location
function updateGPSInfo() {
	gps = Input.location.lastData;
}


function getNewSCPosition(sc_id : int) : Vector3 {
	
	var posn = getSCVector(sc_id);
	var unityAxes = toUnityAxes(posn) / 100;
	return unityAxes;
}

function toUnityAxes(posn : Vector3) : Vector3 {
	return Vector3(posn.y, -posn.z, posn.x);
}

function getSCVector(sc_id : int) : Vector3 {
		
	// Spacecraft vector in ECEF coordinates
	var sc_posn = getSpacecraftPosition(sc_id);
	
	// Device vector
	var device_posn : Vector3;
	device_posn = convertLLAtoECEF(gps.latitude, gps.longitude, gps.altitude);

	// Return the difference between spacecraft & the device
	return (sc_posn - device_posn);
}


/* 
Converts GPS latitude and longitude into x, y and z coordinated relative to centre of the earth. Equations taken from:
mathforum.org/library/drmath/view/51832.html
http://stackoverflow.com/questions/8981943/lat-long-to-x-y-z-position-in-js-not-working
*/
function convertLLAtoECEF(latitude : float, longitude : float, altitude : float) : Vector3 {
	
	var cosLat = Mathf.Cos(latitude * Mathf.PI / 180.0);
	var sinLat = Mathf.Sin(latitude * Mathf.PI / 180.0);
	var cosLon = Mathf.Cos(longitude * Mathf.PI / 180.0);
	var sinLon = Mathf.Sin(longitude * Mathf.PI / 180.0);
	
	// Equatorial radius of the earth in metres
	var earthRad : float = 6378137;
	// Flattening parameter (measure of how elliptical a polar cross-section of the earth is)
	var f : float = (1.0 / 298.257224);
	var minusF : float = (1.0 - f);

	var C : float = 1.0 / Mathf.Sqrt(cosLat * cosLat + minusF * minusF * sinLat * sinLat);
	var S : float = minusF * minusF * C;

	var x : float = (earthRad * C + altitude) * cosLat * cosLon;
	var y : float = (earthRad * C + altitude) * cosLat * sinLon;
	var z : float = (earthRad * S + altitude) * sinLat;

	// Convert to km
	x /= 1000;
	y /= 1000;
	z /= 1000;

	return Vector3(x, y, z);
}
