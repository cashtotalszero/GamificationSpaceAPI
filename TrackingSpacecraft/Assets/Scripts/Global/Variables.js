#pragma strict

public static class Variables {
	//variables for scaleable graphics
	public var boxWidth : int;
	public var boxStartx : int;
	public var boxStarty : int;
	public var lsBoxWidth : int;
	public var buttonWidth : int;
	public var buttonHeight : int;
	public var buttonStartx : int;
	public var buttonStarty : int;
	public var delButtWidth : int;
	public var delButtHeight : int;
	public var delButtStartx : int;
	public var delButtStarty : int;
	public var buttonGap : int;
	public var textStart : int;
	public var textHeight : int;
	public var backWidth : int;
	public var boxHeight : int;
	public var iconGap : int;
	public var iconHeight : int;
	public var iconWidth : int;
	public var iconCenter : int;
	public var iconFarLeft : int;
	public var iconLeft : int;
	public var iconFarRight : int;
	public var iconRight : int;
	public var addCraftWidth : int;
	public var skinSize : int;
	//variable for tracking currently viewed space craft
	public var currentCraft : String;
	public var topBarHeight : int;
	public var groupHeight : int;
	public var backBtnStartx : int;
	public var backBtnStarty : int;
	public var scIconDimension : int;
	public var scIconStartX : int;
	public var scIconStartY : int;
	public var scImageStartX : int;
	public var scImageStartY : int;
	public var scImageSize : int;
	public var dataScrollStartY : int;
	
	public function setVariables(){
	
		//set up variables for scaleable graphics
		//these assignments will be executed once on app startup
		boxWidth = Mathf.Floor(Screen.width - ((Screen.width / 100) * 4));
		boxStartx = Mathf.Floor((Screen.width / 100) * 2);
		boxStarty = Mathf.Floor(Screen.height / 100);
		lsBoxWidth = Mathf.Floor(Screen.height - ((Screen.height / 100) * 4));
		buttonWidth = Mathf.Floor(boxWidth - (boxWidth / 10));
		buttonHeight = Mathf.Floor(Screen.height / 10);
		buttonStartx = Mathf.Floor((boxWidth / 100) * 5);
		buttonStarty = Mathf.Floor(buttonHeight);
		delButtWidth = Mathf.Floor(Screen.height / 9);
		delButtHeight = buttonHeight;
		delButtStartx = buttonWidth - delButtWidth;
		delButtStarty = buttonStarty;
		buttonGap = 1;
		textStart = 0;
		textHeight = Screen.height - textStart - (2 * buttonHeight) - (buttonHeight / 2);
		boxHeight = Screen.height - buttonHeight / 2 - boxStarty;
		iconWidth = Mathf.Floor(Screen.width / 5);
		iconHeight = iconWidth;
		iconGap = (boxWidth - iconWidth * 5) / 4;
		iconCenter = iconWidth * 2 + iconGap * 2;
		iconFarLeft = 0;
		iconLeft = iconWidth * 1 + iconGap * 1;
		iconFarRight = iconWidth * 4 + iconGap * 4;
		iconRight = iconWidth * 3 + iconGap * 3;
		topBarHeight = (boxStarty + 5) * 2 + buttonHeight / 2;
		groupHeight = Screen.height - Variables.boxStarty - (MakeTextures.logo.height * (Screen.width * 100 / MakeTextures.logo.width) / 100) - 2;
		backBtnStartx = boxStartx + 5;
		backBtnStarty = (topBarHeight / 2) - (buttonHeight / 4);
		scIconDimension = buttonHeight * 0.7;
		scIconStartX = (buttonWidth - delButtWidth - buttonGap) * 0.05;
		scIconStartY = (buttonHeight - scIconDimension) / 2;
		scImageStartX = (Screen.width - (Screen.width * 0.35)) / 2;
		scImageStartY = topBarHeight + (Screen.height / 20);
		scImageSize = Screen.width * 0.35;
		dataScrollStartY = scImageStartY + scImageSize;
			
			

		//set currently viewed space craft
		currentCraft = null;

		//variables for text
		if (Screen.height < 400 || Screen.width < 350) {
			textStart = 20;
			backWidth = 50;
			addCraftWidth = 25;
			skinSize = 0;
		}
		else {
			textStart = 40;
			backWidth = 100;
			addCraftWidth = 50;
			skinSize = 1;
		}
	}
}