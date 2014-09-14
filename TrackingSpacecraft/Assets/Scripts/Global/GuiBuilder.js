/*Pocket Mission Control - a JA initiative - a cross platform application for
the review and management of personal and professional spacecraft

Questions and comments: support@PocketMissionControl.org

Written in 2012-2013 by:

Ashley Whetter
Claudiu Octavian Soare
Dominic Moylett
Francis Xavier Kwong
Fraser McQueen-Govan
Ioannis Kaoullas
James Savage
Jamie Daniell
Michael Johnson
Nicholas Phillips
Pakizat Masgutova
William Colaluca
Zahra Dasht Bozorgi

Edited in 2014 by Alex Parrott to integrate into Gamification Project.

To the extent possible under law, the author(s) have dedicated all
copyright and related and neighboring rights to this software to the
public domain worldwide. This software is distributed without any
warranty.

You should have received a copy of the CC0 Public Domain Dedication
along with this software. If not, see
<http://creativecommons.org/publicdomain/zero/1.0/>.

If you contribute to this file and are not listed above, please
complete the following waiver
<http://creativecommons.org/choose/zero/waiver>, email the
confirmation to rights@PocketMissionControl.org and add your name and
email address to the author list. If you are not willing/able to
complete the waiver, please do not contribute to this file.
*/

#pragma strict

public static class GuiBuilder {
	// TODO: Change to a call to back end
    private var scAccess : json = json.fromString('[{"name":"mySpacecraft", "accessible":true}, {"name":"craftGraph", "accessible":true}, {"name":"log", "accessible":true}, {"name":"AR", "accessible":true}, {"name":"telescope", "accessible":true}]');
	private var scIconNo : int = 0;
	private var scIconGap : int = 0;
	private var scIcons : Texture2D[];
	private var scActiveIcons : Texture2D[];
	
	private var labIconNo : int = 0;
	private var labIconGap : int = 0;
	private var labIcons : Texture2D[];
	private var labActiveIcons : Texture2D[];
	
	//private var currentScreen : Screens = Screens.languages;
	private var currentScreen : Screens = Screens.index;
	private var currentScreenBools = new Dictionary.<Screens,boolean>();
	private var doBack : boolean = false;
	public  var accessible : boolean[];
	
	var backStyle : GUIStyle;
	var scNameStyle : GUIStyle;
	var scGameStyle : GUIStyle;
	var defaultSkin : GUISkin = ScriptableObject.CreateInstance(typeof(GUISkin));
	
	private function GuiBuilder(){
		Debug.LogWarning("IN GUIBUILDER CONSTRUCTOR");
		currentScreenBools.Add(Screens.index, false);
		currentScreenBools.Add(Screens.mySpacecraft, false);
		currentScreenBools.Add(Screens.myLaboratory, false);
		currentScreenBools.Add(Screens.mySkills, false);
		currentScreenBools.Add(Screens.mySpaceApps, false);
		currentScreenBools.Add(Screens.myGroundstations, false);
		currentScreenBools.Add(Screens.craftGraph, false);
		currentScreenBools.Add(Screens.selectSpacecraft, false);
		currentScreenBools.Add(Screens.log, false);
		currentScreenBools.Add(Screens.telescope, false);
		currentScreenBools.Add(Screens.temperature, false);
		currentScreenBools.Add(Screens.magnet, false);
		currentScreenBools.Add(Screens.clipboard, false);
		currentScreenBools.Add(Screens.cogs, false);
		currentScreenBools.Add(Screens.about, false);
		currentScreenBools.Add(Screens.languages, false);
		currentScreenBools.Add(Screens.graphScreen, false);
		currentScreenBools.Add(Screens.AR, false);
		currentScreenBools.Add(Screens.QR, false);
		currentScreenBools.Add(Screens.manAdd, false);
		//currentScreenBools.Add(Screens.leaderboard, false);
		
		scIcons = new Texture2D[5];
		scActiveIcons = new Texture2D[5];

	   /* for (var i = 0; i < 5; i++) {
    		storeScIcons(i);
		}*/
		
		labIcons = new Texture2D[5];
		labActiveIcons = new Texture2D[5];
	/*	labIcons[0] = MakeTextures.lab;
		labActiveIcons[0] = MakeTextures.lab_Blue;
		labIcons[1] = MakeTextures.temp;
		labActiveIcons[1] = MakeTextures.temp_Blue;
		labIcons[2] = MakeTextures.clip;
		labActiveIcons[2] = MakeTextures.clip_Blue;
		labIcons[3] = MakeTextures.magnet;
		labActiveIcons[3] = MakeTextures.magnet_Blue;
		labIcons[4] = MakeTextures.cogs;
		labActiveIcons[4] = MakeTextures.cogs_Blue;
	
	    for (i = 0; i < 5; i++) {
	    	if (scAccess._get(i)._get('accessible').boolean_value) {
	    		scIconNo++;
			}
			
			// TODO: Rely on accessibility
			labIconNo++;
		}
	
		if (scIconNo > 1) {
			scIconGap = (Variables.boxWidth - Variables.iconWidth * scIconNo) / (scIconNo - 1);
		}
		
		if (labIconNo > 1) {
			labIconGap = (Variables.boxWidth - Variables.iconWidth * labIconNo) / (labIconNo - 1);
		}*/
		//initDefaultSkin();
	}
	
	function changeToScreen(newScreen : Screens) {
	Debug.LogWarning("Screens index: "+ newScreen);
	Debug.Log("Screens index: "+ newScreen);
			//if (accessible[newScreen]) {
				currentScreenBools[currentScreen] = false;
				currentScreenBools[newScreen] = true;
				currentScreen = newScreen;
			//}
			/*
			// Something has gone wrong and we've tried to go to an inaccessible page so try to go to the main menu 
			else if (accessible[Screens.index]) {
				Debug.Log("Crashed out to index");
				currentScreenBools[currentScreen] = false;
				currentScreenBools[Screens.index] = true;
				currentScreen = Screens.index;
			}
			// If we can't get to the main menu either then we don't know where to go so quit 
			else {
				Application.Quit();
			}*/
	}
	
	function isScreenActive(screen : Screens) {
		return currentScreenBools[screen];
	}
	
	function setDefaultSkin() {
		GUI.skin = defaultSkin;
	}
	
	function drawHeaderBar(headerText : String) {
		drawTopBar();
		GUI.Label(Rect(Variables.boxStartx, 0, Variables.boxWidth, Variables.topBarHeight), headerText);
	}
	
	
	
	
	function drawScNameBar(scName : String) {
		drawTopBar();
		GUI.Label(Rect(0, 0, Variables.boxWidth, Variables.topBarHeight), scName, scNameStyle);
	}
	
	function drawBackBtn(btnText : String) {
		if (doBack) {
			doBack = false;
			return renderBackBtn(btnText) || true;
		}
		
		return renderBackBtn(btnText);
	}
	
	// ALEX ADDITION
	function drawGameBtn(active : boolean, level : int) {
		
		if(!active) {
			GUI.DrawTexture(Rect(0, Screen.height-Variables.topBarHeight, Screen.width, Variables.topBarHeight), MakeTextures.topBar, ScaleMode.StretchToFill);	
			return GUI.Button(Rect(/*Screen.width-Screen.width*/0, Screen.height/2, Variables.lsBoxWidth, Variables.boxHeight), "   TAP HERE TO BEGIN LEVEL", scGameStyle);
		}
		else {
			GUI.DrawTexture(Rect(0, Screen.height-Variables.topBarHeight, Screen.width, Variables.topBarHeight), MakeTextures.topBar, ScaleMode.StretchToFill);	
			return GUI.Button(Rect(0, Screen.height/2, Variables.lsBoxWidth, Variables.boxHeight), ("      LEVEL "+level), scGameStyle);
		}

	}
	
	function goBack() {
		// Mark that we should change screen on the next frame
		doBack = true;
	}
	
	function drawLogos() {
		var logo = MakeTextures.logo;
		
		if ((currentScreenBools[Screens.mySpacecraft]) || (currentScreenBools[Screens.craftGraph]) || (currentScreenBools[Screens.log]) || (currentScreenBools[Screens.telescope]) || (currentScreenBools[Screens.myLaboratory]) || (currentScreenBools[Screens.temperature]) || (currentScreenBools[Screens.magnet]) || (currentScreenBools[Screens.clipboard]) || (currentScreenBools[Screens.cogs]) || (currentScreenBools[Screens.AR])) {
			GUI.DrawTexture(Rect(2, Screen.height - (logo.height * (Screen.width * 100 / logo.width) / 100) - 2 - Variables.iconWidth, Screen.width - 4, (logo.height * (Screen.width * 100 / logo.width) / 100)), logo, ScaleMode.ScaleToFit);
		}
		else {
			GUI.DrawTexture(Rect(2, Screen.height - (logo.height * (Screen.width * 100 / logo.width) / 100) - 2, Screen.width - 4, (logo.height * (Screen.width * 100 / logo.width) / 100)), logo, ScaleMode.ScaleToFit);
		}
	}
	
	function drawScIcons(screenNo : int) {
		var i = 0;
		var iconPos;

		// Draw Icon Bar
		drawIconBar();
		
		toggleTransparentBtn(true);
		
		// Draw icons
		var icon : Texture2D;
		for (var cnt = 0; cnt < 5; cnt++) {
			if (scAccess._get(cnt)._get('accessible').toString() == 'true') {
				if (scIconNo > 1) {
					iconPos = Variables.iconWidth * i + scIconGap * i;
				}
				else {
					iconPos = (Variables.boxWidth - Variables.iconWidth) / 2;
				}
				
				if (cnt == screenNo) {
					icon = scActiveIcons[cnt];
				}
				else {
					icon = scIcons[cnt];
				}	
				
				if (GUI.Button(Rect(iconPos, Screen.height - Variables.iconHeight, Variables.iconWidth, Variables.iconHeight), icon)) {
					// ALL ICON BUTTONS DEACTIVATED APART FROM AR
					if(cnt == 3) {
						changeScScreen(cnt);
					}
				}
				i++;
			}
		}
		
		toggleTransparentBtn(false);
	}
	
	function drawLabIcons(screenNo : int) {
		var i = 0;
		var iconPos;

		// Draw Icon Bar
		drawIconBar();
		
		toggleTransparentBtn(true);
		
		// Draw icons
		var icon : Texture2D;
		for (var cnt = 0; cnt < 5; cnt++) {
			//if (labAccess._get(cnt)._get('accessible').toString() == 'true') {
				if (labIconNo > 1) {
					iconPos = Variables.iconWidth * i + labIconGap * i;
				}
				else {
					iconPos = (Variables.boxWidth - Variables.iconWidth) / 2;
				}
				
				if (cnt == screenNo) {
					icon = labActiveIcons[cnt];
				}
				else {
					icon = labIcons[cnt];
				}	
				
				if (GUI.Button(Rect(iconPos, Screen.height - Variables.iconHeight, Variables.iconWidth, Variables.iconHeight), icon)) {
					changeLabScreen(cnt);
				}
				i++;
			//}
		}
		
		toggleTransparentBtn(false);
	}
	
	function drawIconBar() {
		GUI.DrawTexture(Rect(0, Screen.height - Variables.iconHeight, Screen.width, Variables.iconHeight), MakeTextures.btmBar, ScaleMode.StretchToFill);
	}
	
	function drawTopBar() {
		GUI.DrawTexture(Rect(0, 0, Screen.width, Variables.topBarHeight), MakeTextures.topBar, ScaleMode.StretchToFill);
	}
	
	function toggleAloneButton(toggleOn : boolean) {
		/*
		if (toggleOn) {
			GUI.skin.button.normal.background = MakeTextures.lone_menu_btn;
			GUI.skin.button.hover.background = MakeTextures.lone_menu_btn_active;
			GUI.skin.button.active.background = MakeTextures.lone_menu_btn_active;
			GUI.skin.button.border.top = 15;
			GUI.skin.button.border.left = 15;
			GUI.skin.button.border.right = 15;
			GUI.skin.button.border.bottom = 15;
		}
		else {
			GUI.skin.button.normal.background = MakeTextures.menu_btn;
			GUI.skin.button.hover.background = MakeTextures.menu_btn_active;
			GUI.skin.button.active.background = MakeTextures.menu_btn_active;
			GUI.skin.button.border.top = 0;
			GUI.skin.button.border.left = 0;
			GUI.skin.button.border.right = 0;
			GUI.skin.button.border.bottom = 0;
		}*/
	}
	
	function toggleTopMenu(toggleOn : boolean) {
		if (toggleOn) {
			GUI.skin.button.normal.background = MakeTextures.top_menu_btn;
			GUI.skin.button.hover.background = MakeTextures.top_menu_btn_active;
			GUI.skin.button.active.background = MakeTextures.top_menu_btn_active;
			GUI.skin.button.border.top = 15;
			GUI.skin.button.border.left = 15;
			GUI.skin.button.border.right = 15;
			GUI.skin.button.border.bottom = 15;
		}
		else {
			GUI.skin.button.normal.background = MakeTextures.menu_btn;
			GUI.skin.button.hover.background = MakeTextures.menu_btn_active;
			GUI.skin.button.active.background = MakeTextures.menu_btn_active;
			GUI.skin.button.border.top = 0;
			GUI.skin.button.border.left = 0;
			GUI.skin.button.border.right = 0;
			GUI.skin.button.border.bottom = 0;
		}
	}
	
	function toggleBtmMenu(toggleOn : boolean) {
		if (toggleOn) {
			GUI.skin.button.normal.background = MakeTextures.btm_menu_btn;
			GUI.skin.button.hover.background = MakeTextures.btm_menu_btn_active;
			GUI.skin.button.active.background = MakeTextures.btm_menu_btn_active;
			GUI.skin.button.border.bottom = 15;
			GUI.skin.button.border.left = 15;
			GUI.skin.button.border.right = 15;
			GUI.skin.button.border.top = 15;
		}
		else {
			GUI.skin.button.normal.background = MakeTextures.menu_btn;
			GUI.skin.button.hover.background = MakeTextures.menu_btn_active;
			GUI.skin.button.active.background = MakeTextures.menu_btn_active;
			GUI.skin.button.border.bottom = 0;
			GUI.skin.button.border.left = 0;
			GUI.skin.button.border.right = 0;
			GUI.skin.button.border.top = 0;
		}
	}
	
	function toggleTransparentBtn(toggleOn : boolean) {
		if (toggleOn) {
			GUI.skin.button.normal.background = null;
			GUI.skin.button.hover.background = null;
			GUI.skin.button.active.background = null;
		}
		else {
			GUI.skin.button.normal.background = MakeTextures.menu_btn;
			GUI.skin.button.hover.background = MakeTextures.menu_btn_active;
			GUI.skin.button.active.background = MakeTextures.menu_btn_active;
		}
	}
	
	function toggleSelectScBtn(toggleOn : boolean) {
		if (toggleOn) {
			GUI.skin.button.alignment = TextAnchor.MiddleLeft;
			GUI.skin.button.padding.left = Variables.scIconStartX + Variables.scIconDimension + (Variables.buttonWidth - Variables.delButtWidth - Variables.buttonGap) * 0.05;
		}
		else {
			GUI.skin.button.alignment = TextAnchor.MiddleCenter;
			GUI.skin.button.padding.left = 6;
		}
	}
	
	private function renderBackBtn(btnText : String) {
		return GUI.Button(Rect(Variables.backBtnStartx, Variables.backBtnStarty, Variables.backWidth, Variables.buttonHeight / 2), btnText, backStyle);
	}
	
	private function setBackStyle() {
		backStyle = new GUIStyle(defaultSkin.GetStyle('Button'));
		backStyle.font = Resources.Load('fonts/Arial Narrow Bold Small', typeof(Font));
		backStyle.fontStyle = FontStyle.Bold;
		backStyle.alignment = TextAnchor.MiddleCenter;
		backStyle.normal.textColor = Color.white;
		backStyle.active.textColor = Color.white;
		backStyle.hover.textColor = Color.white;
		backStyle.normal.background = MakeTextures.back_btn;
		backStyle.active.background = MakeTextures.back_btn_active;
		backStyle.hover.background = MakeTextures.back_btn_active;
		backStyle.border.bottom = 0;
		backStyle.border.left = 0;
		backStyle.border.right = 0;
		backStyle.border.top = 0;
		backStyle.padding.top = 0;
		backStyle.padding.left = (60 * Variables.backWidth) / 320; // Width of (arrowhead * newWidth) / originalWidth
		backStyle.padding.right = 0;
		backStyle.padding.bottom = 0;
		backStyle.margin.top = 0;
		backStyle.margin.left = 0;
		backStyle.margin.right = 0;
		backStyle.margin.bottom = 0;
	}
	
	private function setScNameStyle() {
		scNameStyle = new GUIStyle(defaultSkin.GetStyle('Box'));
		var fontGrey : Color = new Color(0.8, 0.8, 0.8, 1);
		scNameStyle.font = Resources.Load('fonts/Arial Narrow Bold Large', typeof(Font));
		scNameStyle.fontSize = Variables.topBarHeight * 0.6;
		scNameStyle.fontStyle = FontStyle.Normal;
		scNameStyle.alignment = TextAnchor.MiddleRight;
		scNameStyle.normal.textColor = fontGrey;
		scNameStyle.active.textColor = fontGrey;
		scNameStyle.hover.textColor = fontGrey;
		scNameStyle.border.bottom = 6;
		scNameStyle.border.left = 6;
		scNameStyle.border.right = 6;
		scNameStyle.border.top = 6;
		scNameStyle.margin.bottom = 4;
		scNameStyle.margin.left = 4;
		scNameStyle.margin.right = 4;
		scNameStyle.margin.top = 4;
		scNameStyle.padding.bottom = 4;
		scNameStyle.padding.left = 4;
		scNameStyle.padding.right = 4;
		scNameStyle.padding.top = 4;
	}
	
	private function setGameStyle() {
		scGameStyle = new GUIStyle(defaultSkin.GetStyle('Box'));
		var fontGrey : Color = new Color(0.8, 0.8, 0.8, 1);
		scGameStyle.font = Resources.Load('fonts/Arial Narrow Bold Large', typeof(Font));
		scGameStyle.fontSize = Variables.topBarHeight * 0.6;
		scGameStyle.fontStyle = FontStyle.Normal;
		scGameStyle.alignment = TextAnchor.MiddleLeft;
		scGameStyle.normal.textColor = fontGrey;
		scGameStyle.active.textColor = fontGrey;
		scGameStyle.hover.textColor = fontGrey;
		scGameStyle.border.bottom = 6;
		scGameStyle.border.left = 6;
		scGameStyle.border.right = 6;
		scGameStyle.border.top = 6;
		scGameStyle.margin.bottom = 4;
		scGameStyle.margin.left = 4;
		scGameStyle.margin.right = 4;
		scGameStyle.margin.top = 4;
		scGameStyle.padding.bottom = 4;
		scGameStyle.padding.left = 4;
		scGameStyle.padding.right = 4;
		scGameStyle.padding.top = 4;
	}
	
	public function initDefaultSkin() {
	
	Debug.Log("initDefaultSkin");
	
	for (var i = 0; i < 5; i++) {
    		storeScIcons(i);
		}
	/*
		labIcons[0] = MakeTextures.lab;
		labActiveIcons[0] = MakeTextures.lab_Blue;
		labIcons[1] = MakeTextures.temp;
		labActiveIcons[1] = MakeTextures.temp_Blue;
		labIcons[2] = MakeTextures.clip;
		labActiveIcons[2] = MakeTextures.clip_Blue;
		labIcons[3] = MakeTextures.magnet;
		labActiveIcons[3] = MakeTextures.magnet_Blue;
		labIcons[4] = MakeTextures.cogs;
		labActiveIcons[4] = MakeTextures.cogs_Blue;
	*/
	    for (i = 0; i < 5; i++) {
	    	if (scAccess._get(i)._get('accessible').boolean_value) {
	    		scIconNo++;
			}
			
			// TODO: Rely on accessibility
			labIconNo++;
		}
	
		if (scIconNo > 1) {
			scIconGap = (Variables.boxWidth - Variables.iconWidth * scIconNo) / (scIconNo - 1);
		}
		
		if (labIconNo > 1) {
			labIconGap = (Variables.boxWidth - Variables.iconWidth * labIconNo) / (labIconNo - 1);
		}
	

		defaultSkin.font = Resources.Load('fonts/Arial Narrow Bold Large', typeof(Font));
		
		// Attributes for boxes
		defaultSkin.box.font = Resources.Load('fonts/Arial Narrow Bold Large', typeof(Font));
		defaultSkin.box.fontSize = 30;
		defaultSkin.box.fontStyle = FontStyle.Bold;
		defaultSkin.box.active.textColor = Color.white;
		defaultSkin.box.normal.textColor = Color.white;
		defaultSkin.box.hover.textColor = Color.white;
		defaultSkin.box.alignment = TextAnchor.UpperCenter;
		defaultSkin.box.border.bottom = 6;
		defaultSkin.box.border.left = 6;
		defaultSkin.box.border.right = 6;
		defaultSkin.box.border.top = 6;
		defaultSkin.box.margin.bottom = 4;
		defaultSkin.box.margin.left = 4;
		defaultSkin.box.margin.right = 4;
		defaultSkin.box.margin.top = 4;
		defaultSkin.box.padding.bottom = 4;
		defaultSkin.box.padding.left = 4;
		defaultSkin.box.padding.right = 4;
		defaultSkin.box.padding.top = 4;
		
		Debug.Log("Background: " );
		
		// Attributes for buttons
		defaultSkin.button.font = Resources.Load('fonts/Arial Narrow Bold Large', typeof(Font));
		defaultSkin.button.fontSize = 24;
		defaultSkin.button.alignment = TextAnchor.MiddleCenter;
		defaultSkin.button.normal.background = MakeTextures.menu_btn;
		defaultSkin.button.hover.background = MakeTextures.menu_btn_active;
		defaultSkin.button.active.background = MakeTextures.menu_btn_active;
		defaultSkin.button.border.bottom = 6;
		defaultSkin.button.border.left = 6;
		defaultSkin.button.border.right = 6;
		defaultSkin.button.border.top = 4;
		defaultSkin.button.margin.bottom = 4;
		defaultSkin.button.margin.left = 4;
		defaultSkin.button.margin.right = 4;
		defaultSkin.button.margin.top = 4;
		defaultSkin.button.padding.bottom = 0;
		defaultSkin.button.padding.left = 6;
		defaultSkin.button.padding.right = 3;
		defaultSkin.button.padding.top = 0;
		
		Debug.Log("Background1: ");
		
		// Attributes for labels
		defaultSkin.label.font = Resources.Load('fonts/Arial Narrow Bold Large', typeof(Font));
		defaultSkin.label.fontSize = 24;
		defaultSkin.label.fontStyle = FontStyle.Bold;
		defaultSkin.label.active.textColor = Color.white;
		defaultSkin.label.normal.textColor = Color.white;
		defaultSkin.label.hover.textColor = Color.white;
		defaultSkin.label.alignment = TextAnchor.MiddleCenter;
		defaultSkin.label.border.bottom = 6;
		defaultSkin.label.border.left = 6;
		defaultSkin.label.border.right = 6;
		defaultSkin.label.border.top = 6;
		defaultSkin.label.margin.bottom = 4;
		defaultSkin.label.margin.left = 4;
		defaultSkin.label.margin.right = 4;
		defaultSkin.label.margin.top = 4;
		defaultSkin.label.padding.bottom = 4;
		defaultSkin.label.padding.left = 4;
		defaultSkin.label.padding.right = 4;
		defaultSkin.label.padding.top = 4;
		
		Debug.Log("Background2: ");
		
		// Initialise other GUI styles
		setBackStyle();
		setScNameStyle();
		setGameStyle();
		
		Debug.Log("Background3: ");
		// Initialise the background
		var background : Texture2D = MakeTextures.background;

		Debug.Log("Background4: " + background.ToString);

		RenderSettings.skybox.SetTexture("_FrontTex", background);
		RenderSettings.skybox.SetTexture("_BackTex", background);
		RenderSettings.skybox.SetTexture("_LeftTex", background);
		RenderSettings.skybox.SetTexture("_RightTex", background);
		RenderSettings.skybox.SetTexture("_UpTex", background);
		RenderSettings.skybox.SetTexture("_DownTex", background);
		
		Debug.Log("Background5: ");
	}
	
	function initAccessible(toParse : json) {
		/*
		accessible = new boolean[Enum.GetNames(typeof(Screens)).Length];
		for (var aScreen : Screens in Enum.GetValues(typeof(Screens))) {
			accessible[aScreen] = toParse._get(aScreen.ToString()).boolean_value;
		}*/
	}
	
	private function storeScIcons(i : int) {
		/*
		if (scAccess._get(i)._get('name').toString() == 'mySpacecraft') {
			scIcons[i] = MakeTextures.overview;
			scActiveIcons[i] = MakeTextures.overview_Blue;
		}
		else if (scAccess._get(i)._get('name').toString() == 'craftGraph') {
			scIcons[i] = MakeTextures.graphs;
			scActiveIcons[i] = MakeTextures.graphs_Blue;
		}
		else if (scAccess._get(i)._get('name').toString() == 'log') {
			scIcons[i] = MakeTextures.info;
			scActiveIcons[i] = MakeTextures.info_Blue;
		}
		else if (scAccess._get(i)._get('name').toString() == 'AR') {
			scIcons[i] = MakeTextures.ar;
			scActiveIcons[i] = MakeTextures.ar_Blue;
		}
		else if (scAccess._get(i)._get('name').toString() == 'telescope') {
			scIcons[i] = MakeTextures.telescope;
			scActiveIcons[i] = MakeTextures.telescope_Blue;
		}
		*/
	}

	private function changeScScreen(i : int) {
		switch (i) {
			case 0:
				changeToScreen(Screens.mySpacecraft);
				break;
			case 1:
				changeToScreen(Screens.craftGraph);
				break;
			case 2:
				changeToScreen(Screens.log);
				break;
			case 3:
				changeToScreen(Screens.AR);
				//changeToScreen(Screens.screen_AR);
				break;
			case 4:
				changeToScreen(Screens.telescope);
				break;
		}
	}

	private function changeLabScreen(i : int) {
		switch (i) {
			case 0:
				changeToScreen(Screens.myLaboratory);
				break;
			case 1:
				changeToScreen(Screens.temperature);
				break;
			case 2:
				changeToScreen(Screens.clipboard);
				break;
			case 3:
				changeToScreen(Screens.magnet);
				break;
			case 4:
				changeToScreen(Screens.cogs);
				break;
		}
	}
};