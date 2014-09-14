#pragma strict

import System.IO;

private var gameScript : gameMain;
static var ARstarted = false;

function Start() {
	gameScript = GameObject.Find("GameMain").GetComponent(gameMain);
}

function OnGUI () {

	// Activates the AR game screen when this screen is called
	if (GuiBuilder.isScreenActive(Screens.AR)) {
	
		if (!ARstarted) {
			EnableAR();
		}
	} 
	else if (ARstarted) {
		DisableAR();
	}
}

// Activates procedures related to AR game screen
function EnableAR() {
	
	//gameScript = GameObject.Find("ARObjs").GetComponent(gameMain);
	gameScript.StartAR(1);
	ARstarted = true;
	Screen.sleepTimeout = SleepTimeout.NeverSleep;
}

// Deactivates procedures related to AR game screen
function DisableAR() {
	
	gameScript.StopAR();
	ARstarted = false;
	Screen.sleepTimeout = SleepTimeout.SystemSetting;
}