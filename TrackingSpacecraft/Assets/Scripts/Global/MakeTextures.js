#pragma strict
import System.Collections.Generic;

public static class MakeTextures {
	var logo : Texture2D;
	var background : Texture2D;

	//var overview : Texture2D;
	//var graphs : Texture2D;
	//var info : Texture2D;
	//var ar : Texture2D;
	//var telescope : Texture2D;

	//var overview_Blue : Texture2D;
	//var graphs_Blue : Texture2D;
	//var info_Blue : Texture2D;
	//var ar_Blue : Texture2D;
	//var telescope_Blue : Texture2D;

/*
	var lab : Texture2D;
	var temp : Texture2D;
	var clip : Texture2D;
	var magnet : Texture2D;
	var cogs : Texture2D;

	var lab_Blue : Texture2D;
	var temp_Blue : Texture2D;
	var clip_Blue : Texture2D;
	var magnet_Blue : Texture2D;
	var cogs_Blue : Texture2D;
*/
	var delete_sc : Texture2D;
	var back_btn : Texture2D;
	var back_btn_active : Texture2D;
	var top_menu_btn : Texture2D;
	var top_menu_btn_active : Texture2D;
	var menu_btn : Texture2D;
	var menu_btn_active : Texture2D;
	var btm_menu_btn : Texture2D;
	var btm_menu_btn_active : Texture2D;
//	var lone_menu_btn : Texture2D;
//	var lone_menu_btn_active : Texture2D;
	
	var topBar : Texture2D;
	var btmBar : Texture2D;
	
	public var ready : boolean = false;

	// texture variable that holds the JA logo
	private var logoWWW : WWW;
	private var www_background : WWW;

	//for mySpacecraft bottom icons
//	private var mySCraft = new List.<WWW>();
//	private var mySCraft_Blue = new List.<WWW>();

	// for myLaboratory bottom icons
//	private var myLabIcons = new List.<WWW>();
//	private var myLabIcons_Blue = new List.<WWW>();

	private var www_delete_sc : WWW;
	private var www_back_btn : WWW;
	private var www_back_btn_active : WWW;
	private var www_top_menu_btn : WWW;
	private var www_top_menu_btn_active : WWW;
	private var www_menu_btn : WWW;
	private var www_menu_btn_active : WWW;
	private var www_btm_menu_btn : WWW;
	private var www_btm_menu_btn_active : WWW;
//	private var www_lone_menu_btn : WWW;
//	private var www_lone_menu_btn_active : WWW;

	private var www_topBar : WWW;
	private var www_btmBar : WWW;

	private var rootPath : String;
//	private var craftIconPath : String;
//	private var labIconPath : String;

	function loadTextures() {
		
		ready = false;		
		rootPath = Application.persistentDataPath + '/Cached/Skins';
			
		logoWWW = new WWW('file://' + rootPath + '/GUI/logo/white_Logo.png');
		www_background = new WWW('file://' + rootPath + '/GUI/main_Menu/background.png');
		www_delete_sc = new WWW('file://' + rootPath + '/Buttons/Delete_Button.png');
		www_back_btn = new WWW('file://' + rootPath + '/Buttons/Back_Button.png');
		www_back_btn_active = new WWW('file://' + rootPath + '/Buttons/Back_Button_Active.png');
		www_top_menu_btn = new WWW('file://' + rootPath + '/Buttons/Menu_Button_Top.png');
		www_top_menu_btn_active = new WWW('file://' + rootPath + '/Buttons/Menu_Button_Active_Top.png');
		www_menu_btn = new WWW('file://' + rootPath + '/Buttons/Menu_Button.png');
		www_menu_btn_active = new WWW('file://' + rootPath + '/Buttons/Menu_Button_Active.png');
		www_btm_menu_btn = new WWW('file://' + rootPath + '/Buttons/Menu_Button_Bottom.png');
		www_btm_menu_btn_active = new WWW('file://' + rootPath + '/Buttons/Menu_Button_Active_Bottom.png');
		www_topBar = new WWW('file://' + rootPath + '/Bars/Top_Bar.png');
		www_btmBar = new WWW('file://' + rootPath + '/Bars/Icon_Bar.png');
	}
		
	function continueLoadTexture() {
		
		logo = logoWWW.texture;
		background = www_background.texture;
		Debug.Log("Logo :" +logo.ToString);

		delete_sc = www_delete_sc.texture;
		back_btn = www_back_btn.texture;
		back_btn_active = www_back_btn_active.texture;
		top_menu_btn = www_top_menu_btn.texture;
		menu_btn = www_menu_btn.texture;
		btm_menu_btn = www_btm_menu_btn.texture;
		top_menu_btn_active = www_top_menu_btn_active.texture;
		menu_btn_active = www_menu_btn_active.texture;
		btm_menu_btn_active = www_btm_menu_btn_active.texture;
		topBar = www_topBar.texture;
		btmBar = www_btmBar.texture;
		
		ready = true;
	}
};
