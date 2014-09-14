#pragma strict
import System.IO;

static var rootPath : String;
static var ready : boolean = false;
private var cachingSpacecraftObject : GameObject; 
private var cachingSpacecraftScript : Caching_Spacecraft;
private var makeTexturesScript : MakeTextures;

function Start() {	
	
	rootPath = Application.persistentDataPath;
	
	// On first load - create directory on device to hold cached files
	if (!Directory.Exists(rootPath + '/Cached')) {
        Directory.CreateDirectory(rootPath + '/Cached');
    }	
	// Find referenced scripts 
    cachingSpacecraftObject = GameObject.Find("CachingObject");
    cachingSpacecraftScript = cachingSpacecraftObject.GetComponent("Caching_Spacecraft");
	
 	// On first load, save the GUI images onto the device (stored as a zip file on resources)
 	if (!Directory.Exists (Application.persistentDataPath + "/Cached/Skins")) {
	 	GUIUnzipper.unzipFolder();
	}

	// Load & assign all textures...
    MakeTextures.loadTextures();
    yield WaitForSeconds(1);
    MakeTextures.continueLoadTexture();

 	//.. and layout variables
    var i :int;
    for(i=0;i<10;i++) {
	    if(MakeTextures.ready) {
		    Variables.setVariables();
		    break;
		}
		else {
			Debug.Log("Loading. Please Wait....");
			yield WaitForSeconds(1);
		}
	}	
 	ready = true;
 	
 	// Initialise the GUI	
	GuiBuilder.initDefaultSkin();
	GuiBuilder.changeToScreen(Screens.index);  
}