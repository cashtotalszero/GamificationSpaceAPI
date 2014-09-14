#pragma strict

private var gameScript : gameMain;
public var speed : float;
private var defaultSpeed : float;
private var gameLevel : int;

// Called once at start of the program
function Start() {
	
	// Assign referenced scripts
	gameScript = GameObject.Find("GameMain").GetComponent(gameMain);
	
	// Initialise variables
	defaultSpeed = 10;
	gameLevel = 1;
}

// Called once per frame
function Update() {
	
	// Only rotate the model when the game screen is active	
	if(gameScript.getARStatus()) {
	
		// Use the current game level to define rotation speed
		gameLevel = gameScript.getCurrentLevel();	
		if(!gameScript.gameActive) {
			gameLevel += 1;
		}
		speed = defaultSpeed * gameLevel;
			
		// Rotate the object around its X axis at defined speed...
		transform.Rotate(Time.deltaTime*speed, 0, 0);
			
		// ... at the same time as spinning it relative to the global Y axis at the same speed.
		transform.Rotate(0, Time.deltaTime*speed, 0, Space.World);
	}
}