#pragma strict

var scoreDisplay : String;
var score : int;
var downloadDisplay : String;
var downloaded : float;
var rescueDisplay : String;
var rescued : float;
var levelDisplay : String;
var endScore : int;
var endDisplay : String;

// Displays all GUI elements
function OnGUI() {

	if (GuiBuilder.isScreenActive(Screens.about)) {
		
		// Draw header bar
		GuiBuilder.setDefaultSkin();
		GuiBuilder.drawHeaderBar("Game Stats");
		
		// Draw "Done" button to allow user leave the screen
		if (GuiBuilder.drawBackBtn("Done")) {
	        GuiBuilder.changeToScreen(Screens.index);
		}
		
		// Generate information for the display
		calculateFinalScore();
		downloadDisplay = "Data Download Bonus : "+downloaded+" seconds";
		scoreDisplay = "Rescue Score : "+score;
		rescueDisplay = "Packets Rescued : "+rescued+"%";
		endDisplay = "FINAL SCORE: "+endScore;
		
		//Make a group for stats display
		var i : int = 0;
		GUI.BeginGroup(Rect(Variables.boxStartx, Variables.boxStarty, Variables.boxWidth, Variables.groupHeight));
		GUI.BeginGroup(Rect(Variables.buttonStartx, Variables.buttonStarty, Variables.buttonWidth, Variables.groupHeight - Variables.buttonStarty));
		GUILayout.BeginVertical();

		// Display final score
		GuiBuilder.toggleTopMenu(true);
		GUI.Button(Rect(0, (i * (Variables.buttonGap + Variables.buttonHeight)), Variables.buttonWidth, Variables.buttonHeight), endDisplay);
		i++;
		GuiBuilder.toggleTopMenu(false);

		// Display level reached
		GuiBuilder.toggleBtmMenu(true);
		GUI.Button(Rect(0, (i * (Variables.buttonGap + Variables.buttonHeight)), Variables.buttonWidth, Variables.buttonHeight), levelDisplay);
		i++;
		GuiBuilder.toggleBtmMenu(false);

		// This creates are space between the 2 sections
		GUI.Button(Rect(0, (i * (Variables.buttonGap + Variables.buttonHeight)), 0,0),"");	
		i++;
	
		// Display points breakdown header
		GuiBuilder.toggleTopMenu(true);		
		GUI.Button(Rect(0, (i * (Variables.buttonGap + Variables.buttonHeight)), Variables.buttonWidth, Variables.buttonHeight),"POINTS BREAKDOWN:");	
		i++;
		GuiBuilder.toggleTopMenu(false);
		
		// Display in game rescue score
		GUI.Button(Rect(0, (i * (Variables.buttonGap + Variables.buttonHeight)), Variables.buttonWidth, Variables.buttonHeight), scoreDisplay);		
		i++;

		// Display % of packets rescued
		GUI.Button(Rect(0, (i * (Variables.buttonGap + Variables.buttonHeight)), Variables.buttonWidth, Variables.buttonHeight), rescueDisplay);		
		i++;

		// Display download time 
        GuiBuilder.toggleBtmMenu(true);
		GUI.Button(Rect(0, (i * (Variables.buttonGap + Variables.buttonHeight)), Variables.buttonWidth, Variables.buttonHeight), downloadDisplay);
		i++;
		GuiBuilder.toggleBtmMenu(false);
		
		// End the group
		GUILayout.EndVertical();
		GUI.EndGroup();
		GUI.EndGroup();
		GuiBuilder.drawLogos();
	}
}

function lastScore(finalScore : int) {
	score = finalScore;
}

function lastDownload(downloadAmount : float) {
	downloaded = downloadAmount;
}

function lastRescue(rescueCount : int, endLevel : int, lastPackets: int) {
	var maxPackets = 10; // FUNCTION TO GET MAX PACKETS
	
	// Calculate % of rescued packets
	var totalPackets = (endLevel-1) * maxPackets;
	totalPackets += lastPackets;	
	rescued = (100 / totalPackets) * rescueCount;
	
	levelDisplay = "Level "+endLevel;
}

// Calculates the players final score
function calculateFinalScore() {
	
	// Player score = rescue score (10 points for each rescued packet) + download bonus (1 point for each second)
	endScore = score + (toInt(downloaded));
}

// Rounds float value to int
function toInt(number : float){ 
	return Mathf.Round(number);
}
