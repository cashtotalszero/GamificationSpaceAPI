#pragma strict

private var showCraftRef : showCraft;
private var gameScript : gameMain;
private var API : GamificationAPI1;
private var showBoard : boolean;
private var barString : String;

function Start() {
	
	// Find referenced scripts
	showCraftRef = GetComponent(showCraft);
	gameScript = GameObject.Find("GameMain").GetComponent(gameMain);
	API = GameObject.Find("Main Camera").GetComponent("GamificationAPI1");
	showBoard = false;
}

function OnGUI() {
	
	if (GuiBuilder.isScreenActive(Screens.index)) {
		GuiBuilder.setDefaultSkin();
		barString = (showBoard == true) ? "Top Scores" : "Alex Parrott MSc Gamification Project";
		GuiBuilder.drawHeaderBar(barString);
		
		// Draw "Back" button when leaderboard is displayed
		if(showBoard) {
			if (GuiBuilder.drawBackBtn("Back")) {
		       showBoard = false;
			}
		}
						
		var i : int = 0;

		//Make a group for the menu buttons
		GUI.BeginGroup(Rect(Variables.boxStartx, Variables.boxStarty, Variables.boxWidth, Variables.groupHeight));	
		GUI.BeginGroup(Rect(Variables.buttonStartx, Variables.buttonStarty, Variables.buttonWidth, Variables.groupHeight - Variables.buttonStarty));
		GUILayout.BeginVertical();

		// Display the main menu
		if(!showBoard) {
			// New game button
			GuiBuilder.toggleTopMenu(true);
			if (GUI.Button(Rect(0, (i * (Variables.buttonGap + Variables.buttonHeight)), Variables.buttonWidth, Variables.buttonHeight), "NEW GAME")) {	
				gameScript.resetGame();
				showCraftRef.changeSpacecraft();
				GuiBuilder.changeToScreen(Screens.AR);
			}		
			i++;
			GuiBuilder.toggleTopMenu(false);
	
			// Show leaderboard button
			GuiBuilder.toggleBtmMenu(true);	
			if (GUI.Button(Rect(0, (i * (Variables.buttonGap + Variables.buttonHeight)), Variables.buttonWidth, Variables.buttonHeight), "HI SCORES")) {		
				showBoard = true;
			}
			i++;
			GuiBuilder.toggleBtmMenu(false);
		}
		// Unless the player wants to see the leaderboard
		else {
			GuiBuilder.toggleTopMenu(true);
			GUI.Button(Rect(0, (i * (Variables.buttonGap + Variables.buttonHeight)), Variables.buttonWidth, Variables.buttonHeight), "Top score");				
			i++;
			GuiBuilder.toggleTopMenu(false);
			GUI.Button(Rect(0, (i * (Variables.buttonGap + Variables.buttonHeight)), Variables.buttonWidth, Variables.buttonHeight), "Middle Score");		
			i++;
			GuiBuilder.toggleBtmMenu(true);	
			GUI.Button(Rect(0, (i * (Variables.buttonGap + Variables.buttonHeight)), Variables.buttonWidth, Variables.buttonHeight), "Bottom score");
			i++;
			GuiBuilder.toggleBtmMenu(false);
		}
		GUILayout.EndVertical();
		GUI.EndGroup();
		GUI.EndGroup();
		GuiBuilder.drawLogos();
	}
}
