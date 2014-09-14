#pragma strict
import System.Math;

private var sc_id : int;							// The ID of the spacecraft to be tracked				
private var gameActive = false;						// The game status

// References to other scripts and game objects 
private var gameScript : gameMain;
private var API : GamificationAPI1;

// References to game objects
private var scModel : GameObject;					// The main horseshoe shaped spacecraft model
private var crosshair : GameObject;					// The player crosshair
private var pod : GameObject;						// The triangular escape pod
private var arrow : GameObject;						// The arrow which points to the spacecraft

// Called once at start of the program
function Start(){
	
	// Assign referenced scripts 
	gameScript = GameObject.Find("GameMain").GetComponent(gameMain);
	API = GameObject.Find("Main Camera").GetComponent("GamificationAPI1");
			
	// Find all the scene/game objects & models
	scModel = GameObject.FindGameObjectWithTag("Enemy");
	pod = GameObject.FindGameObjectWithTag("Pod");
	arrow = GameObject.FindGameObjectWithTag("Arrow");
	crosshair = GameObject.FindGameObjectWithTag("Crosshair");
	
}

// Called once per frame
function Update() {
	
	// Only update spacecraft information when the game is active
	if(gameActive) {	
		
		// Update GPS information					
		API.updateGPSInfo();
		
		// Update the spacecraft position	
		updateSCPosition(sc_id);		
	}
}

// Initialises all game display models
function showSpacecraft(sc_id : int) {
	
	// Get the tracked spacecraft's relative position for the scene
	var newPosn = API.getNewSCPosition(sc_id);
	
	// Render the spacecraft models & AR arrow
	displayModels();
	arrow.renderer.enabled = true;
	
	// Update game object tags
	scModel.tag = "selectedCraft";
	scModel.transform.localScale = Vector3(5,5,5);
	scModel.gameObject.name = "spacecraftid=" + sc_id;
	
	// Move spacecraft model to correct position
	scModel.transform.position = newPosn;
	
	// Reset the game ready for a new player
	//ARScript.resetGame();
	gameScript.resetGame();
	
	// End game if game is over
	//if(ARScript.gameOver) {
	if(gameScript.gameOver) {
		//ARScript.gameOver = false;
		gameScript.gameOver = false;
		displayModels();
	}
	Debug.Log("NEW SPACECRAFT VECTOR 3 = " + newPosn);
}

// Updates game model positions to match tracked spacecraft 
function updateSCPosition(sc_id : int) {
	
	// Get the latest position of the spacecraft being tracked	
	var newPosn = API.getNewSCPosition(sc_id);	
	
	// Ensure the correct models are being rendered
	displayModels();
	
	// Find the latest position of the tracked spacecraft and fix the distance (on the same vector) so that the object size remains consistent during gameplay
	var startPos = Vector3(0,0,0);
	var distance = 100;
	newPosn = (newPosn - startPos).normalized * distance + startPos;

	// Move models to match this location (if different from current location)
	if(newPosn != scModel.transform.position) {
		scModel.transform.position = Vector3.Lerp(scModel.transform.position, newPosn, Time.time);
	}
}

// Renders game models in the scene
function displayModels() {

	// If game level has started...
	//if(ARScript.getGameStatus()) {
	if(gameScript.getGameStatus()) {
		
		// Do nothing if the game is over
		//if(ARScript.gameOver) {
		if(gameScript.gameOver) {
			return;
		}
		// Display the game crosshair
		crosshair.renderer.enabled = true;
	}
	// Otherwise, before level has started...
	else {	
		// ...hide the game crosshair.
		crosshair.renderer.enabled = false;
		
		// Only display the spacecraft model so the user can find it
		scModel.renderer.enabled = true;
		pod.renderer.enabled = true;
	}
}

// Changes the spacecraft being tracked 
public function changeSpacecraft() {
	
	if(sc_id == 0) {
		sc_id = 1;
	}
	else {
		sc_id = 0;
	}
}

// Starts the game
public function activateGame(id : int) {
	
	// Start reading GPS & enable compass (accelerometer started differently)
	Input.location.Start();
	Input.compass.enabled = true;
		
	// Set spacecraft id
	sc_id = id;
	
	// Display the chosen spacecraft
	showSpacecraft(sc_id);
		
	// Set game to active
	gameActive = true;
}

// Exits the game
public function exitGame() {
	
	// Reset the tags
	scModel.tag = "Enemy";
	scModel.gameObject.name = "vehicle_enermyShip";
	
	// Hide all game models
	pod.renderer.enabled = false;
	scModel.renderer.enabled = false;
	crosshair.renderer.enabled = false;
	
	// Hide AR arrow
	arrow.renderer.enabled = false;
		
	// Ensure game mode is turned off & deactive AR 
	//ARScript.setGameStatus(false);
	gameScript.setGameStatus(false);
	gameActive = false;

	// Switch off GPS & compass
	Input.location.Stop();
	Input.compass.enabled = false;
}