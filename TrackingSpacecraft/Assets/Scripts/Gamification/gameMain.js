#pragma strict

// Game object, script & prefab references
private var crosshair : GameObject;
private var hatch : GameObject;
private var lifePod : GameObject;
public var Packet : GameObject;							
public var playerExplosion : GameObject;
public var shipExplosion : GameObject;
private var cachingSpacecraftObject : GameObject;
private var cachingSpacecraftScript : Caching_Spacecraft;
private var showCraftRef : showCraft;
private var AboutRef : About;
private var cam_mainCam : Camera;

// Textures
public var progressBarEmpty : Texture2D;
public var progressBarFull : Texture2D;
public var progressRed : Texture2D;
public var progressGreen : Texture2D;
public var gameOverImg : Texture2D;
public var findArrowImg : Texture2D;
public var endLevelImg : Texture2D;

// Dataflow booleans - to prevent infinite loops
private var packetWait : boolean = false;
private var audioWait : boolean = false;
private var powerWait : boolean;
private var endLevelWait : boolean = false;

// Game status booleans
private var tractorActive : boolean = false;
public var gameOver : boolean = false;
private var animationOver : boolean = false;
private var recordTime : boolean = false;
private var levelOver : boolean = false;
public var gameActive : boolean = false;
private var crosshairHit : boolean = false;
private var powerUp : boolean = false;
private var ARactive = false;

// Energy bar variables
private var barDisplay : float = 0;
private var pos : Vector2 = new Vector2(20,100);
private var size : Vector2 = new Vector2(150,60);
private var barFill : float = 1.0;

// Current game information
private var level : int = 0;
private var timer : float = 0.0;
private var packetCount : int  = 0;
private var maxPackets :int = 10;
private var rescueCount : int = 0;
private var leftInfoBox : String = '';
private var rightInfoBox : String = '';
private var score : int = 0;
private var duration : float = 0.1;

// Audio
var audio1: AudioSource;
var audio2: AudioSource;
var audio3: AudioSource;
var audio4: AudioSource;
var audio5: AudioSource;

// Called once at start of the program
function Start() {
	
	// Activate the mobile device compass
	Input.compass.enabled = true;					
	
	// Find all required script, game object and camera references
	cachingSpacecraftObject = GameObject.Find("CachingObject");
	cachingSpacecraftScript = cachingSpacecraftObject.GetComponent("Caching_Spacecraft");
	cam_mainCam = GameObject.Find("Main Camera").camera;
	showCraftRef = cam_mainCam.GetComponent(showCraft);	
	AboutRef = cam_mainCam.GetComponent(About);
	
	
	lifePod = GameObject.FindGameObjectWithTag("Pod");
	crosshair = GameObject.FindGameObjectWithTag("Crosshair");
	hatch = GameObject.FindGameObjectWithTag("Hatch");
	
	// Initialise all audio sources for sound FX 
	var aSources = GetComponents(AudioSource);
    audio1 = aSources[0];
    audio2 = aSources[1];
    audio3 = aSources[2];
    audio4 = aSources[3];
    audio5 = aSources[4];
}

// Called every fixed framerate frame
function FixedUpdate ()
{
	if(gameActive) {
		
		// Send a raycast from centre of the crosshair and save all collider hits in an array
		var hit : RaycastHit;
	    var fwd : Vector3 = crosshair.transform.TransformDirection(Vector3.down);
		var hits : RaycastHit[];
		hits = Physics.RaycastAll(crosshair.transform.position,fwd);
		
		// Reset game status variables
		crosshairHit = false;
		recordTime = false;
		powerUp = false;
		tractorActive = false;
		
		// Action appropriate raycast hits
		for(var i=0; i<hits.Length;i++) {
			hit = hits[i];
    	  	// If crosshair is over the download area turn on the "stopwatch" to track potential download time
    	  	if (hit.collider.gameObject.tag == "Download") {    	
				recordTime = true;			
    	    }
    	    // If crosshair is over a packet activate the tractor beam
    	    if (hit.collider.gameObject.tag == "Packet") {	
    	    	activateTractor(hit);
    	    }
    	    // Action any hits when crosshair is over the life pod	    
    	    if (hit.collider.gameObject.tag == "Pod") {    	
				energisePod();							// Energy bar goes up when crosshair is over the life pod
				rescuePacket();							// Any packets on the tractor beam are rescued 
    	    }
		}
		// Monitor download time
		if(recordTime) {
			updateTimer();
		}
		// Change crosshair colour back to white if not active
		if(!crosshairHit) {
			crosshair.renderer.material.color = Color.white;
		}
		// Update the energy bar 
		displayPowerBar();
		
		// Add packets to the scene
	   	if(!packetWait) {
	   		addPacket();
	   	}
	   	// End game if power bar runs out
	   	powerWait = !powerWait;
	   	if(barFill <= 0.00 && !gameOver) {
	   		endGame();
	   	}
	   	// Check to see if player has completed the level
	   	if(packetCount == maxPackets) {
	   		var alivePacket = GameObject.FindWithTag("Packet");
	   		if(alivePacket == null) {
	   			endLevel();
	   		}
	   	}
    }		
}

// Draws all GUI components
function OnGUI() {
	
	if(ARactive) {	
		leftInfoBox = "\nDOWNLOAD TIME: \n"+timer;
		rightInfoBox = "\nSPACECRAFT NAME: \n";
				
		// Once level has started draw score in top bar and information boxes at the bottom of the screen
		GuiBuilder.drawScNameBar("SCORE: "+score);
		if(gameActive) {
			leftInfoBox = GUI.TextArea(Rect(10, Screen.height - 290, 140, 100), leftInfoBox);
			rightInfoBox = GUI.TextArea(Rect(Screen.width - 150, Screen.height - 290, 140, 100), rightInfoBox);
		}
		// Draw a quit button for easy exit of game
		if(!gameOver) {
			if (GuiBuilder.drawBackBtn("Quit")) {	
				StopAR();
				destroyPackets();
				GuiBuilder.changeToScreen(Screens.index);
				return;
			}
		}	
		// Draws a bar at the bottom of the screen - start level button/level indicator
		if(!gameActive) {
			GUI.DrawTexture (Rect (50,Screen.height/10, Screen.width-100,Screen.height/5),findArrowImg, ScaleMode.StretchToFill,true,10.0f);
			if(GuiBuilder.drawGameBtn(gameActive,level)){
				endLevelWait = false;
				level++;
				gameActive = !gameActive; 
				return;	
			}
		}
		else {
			GuiBuilder.drawGameBtn(gameActive,level);
		}
		// Display the tractor beam energy bar
		if(gameActive) {
	    	GUI.BeginGroup (new Rect (pos.x, pos.y, size.x, size.y));
	 			GUI.DrawTexture (Rect (0,0, size.x, size.y),progressBarEmpty, ScaleMode.StretchToFill,true,10.0f);
	    	    GUI.BeginGroup (new Rect (0, 0, size.x * barDisplay, size.y));
	    	        GUI.DrawTexture (Rect (0,0, size.x, size.y),progressBarFull, ScaleMode.StretchToFill,true,10.0f);
	    	    GUI.EndGroup ();
			GUI.EndGroup ();			
	    }
	    // For display at end of the game
		if(gameOver) {
    			GuiBuilder.drawLogos();	
    			if(animationOver) {
    				// Shut off explosion warning sound and display game over image
    				audio4.mute = true;
 	   				GUI.DrawTexture (Rect (0.5,0.5, Screen.width,Screen.height),gameOverImg, ScaleMode.StretchToFill,true,10.0f);
 		   			
 		   			// If continue button is pressed - disable game screen and go to game stats
 		   			if (GuiBuilder.drawBackBtn("Continue")) {
						StopAR();
						//resetGame();
						GuiBuilder.changeToScreen(Screens.about);		
						return;
					}
				}
    		}
    	else if(levelOver) {
    		GUI.DrawTexture (Rect (0.5,0.5, Screen.width,Screen.height),endLevelImg, ScaleMode.StretchToFill,true,10.0f);
    	}
		else {
			GuiBuilder.drawLogos();
		}
	}
}

// Displays the energy bar in the appropriate colour according to power level.
function displayPowerBar() {

	// Energy bar drops if crosshair is not over it
	if(!powerUp && barFill > 0.00) {
		if(!powerWait) {
			barFill = barFill - 0.005;
		}
	}
	//Energy bar changes to red if it drops below 20% and turn on warning sound
   	if(barFill < 0.20) {
	  	progressBarFull = progressRed;
	   	if(!audio4.isPlaying) {
		   	audio4.mute = false;
		   	audio4.Play();
		}
	}
	// If power have be chnarged over 20% change colour back to green and stop warning sound
	else {
	   	progressBarFull = progressGreen;
	   	if(audio4.isPlaying) {
	   		audio4.mute = true;
	   		audio4.Stop();
	   	}
	}
	barDisplay = barFill;
}

// Increases the energy bar when the crosshair is over the life pod.
function energisePod() {

	if(lifePod.renderer.enabled) {
		
		// Crosshair alternates red & white to visually signify energy power up
		var lerp : float = Mathf.PingPong (Time.time, duration) / duration;
		crosshair.renderer.material.color = Color.Lerp (Color.red, Color.white, lerp);
		crosshairHit = true;
	
		// Play energise sound effect if not already playing			
		if(!audioWait) {
			energiseAudio();
		}
	
		// Steadily increase the energy level until full			
		powerUp = true;
		if(barFill < 1.00) {
			if(!powerWait) {
				barFill = barFill + 0.005;
			}
		}
	}
}

// Plays sound FX when pod is being energised.
function energiseAudio() {
	
	// Set audioWait boolean so sound effects don't overlap
	audioWait = true;
	
	// Play tractor beam sounds
	yield WaitForSeconds(0.25);
	audio2.Play();
	yield WaitForSeconds(0.25);
	audio3.Play();
	yield WaitForSeconds(0.25);
	
	// Reset audioWait to allow audio to be played again
	audioWait = false;
}

// Activates the in game tractor beam used to move packets around the scene.
function activateTractor(hit : RaycastHit) {
	
	// Only allow one packet on beam at one time, destroy all if more than one
	if(tractorActive && hit.collider.gameObject.name != "onBeam") {
    	Instantiate(playerExplosion, GameObject.Find("onBeam").transform.position, GameObject.Find("onBeam").transform.rotation);
    	Destroy(hit.collider.gameObject);												// Destroy other packet
    	Destroy(GameObject.Find("onBeam"));												// Destroy packet already on beam
    	audio5.Play();																	// Turn on collision sound effect
    	tractorActive = false;															// Turn off the tractor beam
    }
    // Otherwise attach the packet to the tractor beam
    else {	
    	if(hit.collider.gameObject.name != "onBeam") {		
	    	hit.collider.gameObject.transform.parent = crosshair.transform;				// Make packet a child of the crosshair
	    	var startPos = crosshair.transform.position;
			var direction = startPos.normalized;
	    	hit.collider.gameObject.transform.position = startPos + direction * 50;		// Set the distance from crosshair (prevents erratic size changes)
	    	hit.collider.gameObject.name = "onBeam";									// Mark the packet as on beam
   			tractorActive = true;														// Set the beam as active													
    	}
    }
}

// Removes packets from the scene if they are placed in the life pod and adds to player score.
function rescuePacket() {
	
	if(lifePod.renderer.enabled) {
		
		// Check to see that packet is on the tractor beam
		var onBeam = GameObject.Find("onBeam"); 
		
		if(onBeam != null) {
			Destroy(onBeam);					// Remove found packet from the game screen
			tractorActive = false;				// Turn off the tractor beam
			audio1.Play();						// Play rescue sound effect
			score += 10;						// Increment player score
			rescueCount++;						// Increment rescue count for game stats
		}
	}
}

// Adds a new packet to the scene and adds a force to make it move
function addPacket() {
	
	if(!gameOver && packetCount < maxPackets) {
		
		// Set wait and instanciate a packet character in the same position & rotation as the escape hatch
		packetWait = true;
		packetCount++;
		var dude = GameObject.Instantiate(Packet, Vector3(hatch.transform.position.x,hatch.transform.position.y,hatch.transform.position.z), Quaternion.identity);
		
		// Push it forwards away from the escape hatch
		var fwd = hatch.transform.TransformDirection(Vector3.up);
		dude.rigidbody.AddForce(fwd * 25);
		
		// Deactivate rigidbody to save CPU power
		dude.rigidbody.detectCollisions = false;
	
		// Do not allow any more packets to be instanciated until scheduled wait time is complete
		yield WaitForSeconds(3);
		packetWait = false;
	}
}

// Removes all packets from the scene
function destroyPackets() {
	
	// Find all packets currently in the scene
	var gos : GameObject[];
	gos = GameObject.FindGameObjectsWithTag("Packet");
	var remainingPacket;
	
	// Destroy all found packets
	for(var i=0; i<gos.Length;i++) {
		remainingPacket = gos[i];
		Destroy(remainingPacket);		
	} 
}

// Makes the life pod and the ship explode (when the energy bar reaches zero)
function destroyShip() {
	
	// Display explosion in same location of life pod and turn off the renderer 
	Instantiate(shipExplosion, lifePod.transform.position, lifePod.transform.rotation);
	lifePod.renderer.enabled = false;
	
	// After 1 second wait repeat process with the main ship
	yield WaitForSeconds(1);
	var ship = GameObject.FindGameObjectWithTag("selectedCraft");
	Instantiate(shipExplosion, ship.transform.position, ship.transform.rotation);
	ship.renderer.enabled = false;
	
	// Wait for one second to allow ships to explode then set animationOver flag to true
	yield WaitForSeconds(1);
	animationOver = true;
	
	// Remove any remaining packets from the scene
	destroyPackets();	
}

// Returns the level number of the game currently in progress.
public function getCurrentLevel() {
	return level;
}

// Ends a game level and resets relevant information ready for the next level.
function endLevel() {

	// Wait half a second before changing the display
	yield WaitForSeconds(0.5);
	gameActive = false;
	levelOver = true;
	
	// Reset energy bar and packet count ready for next level
	packetCount = 0;
	barFill = 1.0;
	
	// Wait so that level end image can display 
	yield WaitForSeconds(4);
	levelOver = false;
	
	// Find an appropriate spacecraft to track for the next level
	if(!endLevelWait) {
		//ARshowCraftRef.changeSpacecraft();
		showCraftRef.changeSpacecraft();
	}
	endLevelWait = true;
}

// Ends the current game and sends final stats to the stats (About) script
function endGame() {
	
	// Shut off power low warning sound
	audio4.mute = true;
	
	// Send final game stats to stats script 		
	AboutRef.lastScore(score);
	AboutRef.lastDownload(timer);
	AboutRef.lastRescue(rescueCount,level,packetCount);   	
	
	// End the game   		
	gameOver = true;
	destroyShip();
}

// Increments a timer. Acts as a stopwatch to time how long downloads could be happening.
function updateTimer() {
	timer += 1 * Time.deltaTime;
}

// Resets all game information ready for a new game.
function resetGame() {
	barFill = 1.0;
	animationOver = false;
	timer = 0.0;
	score = 0;
	packetCount = 0;
	rescueCount = 0;
	level = 0;
	cachingSpacecraftScript.uploadComplete = false;
}

// Increases player score by specified amount
public function incrementScore(amount : int) {
	score += amount;
}

// Enable AR feature
function StartAR(sc_id : int) {
			
	// Move camera to centre of virtual space & reset the rotation
	cam_mainCam.transform.position.z = 0;
	cam_mainCam.transform.rotation = Quaternion.identity;
	
	// Ensure compass has activated then set heading to match (if no compass reading after 5 seconds set heading to 0)
	var i : int;
	for(i=0;i<6;i++) {
		if(Input.compass.enabled || i==5) {
			var startHeading = Quaternion.Euler(0.0,Input.compass.magneticHeading,0.0);
			cam_mainCam.transform.rotation *= startHeading;
			break;
		}
		else {
			Input.compass.enabled = true;
			yield WaitForSeconds(1);
		}
	}
	
	// Activate AR to track specified spacecraft	
	showCraftRef.activateGame(sc_id);	
	ARactive = true;
}

function StopAR() {
	
	// Reset camera rotation & return camera to its original location
	cam_mainCam.transform.rotation = Quaternion.identity;
	cam_mainCam.transform.position.z = -80;
	
	// Reset the player score and deactivate the AR
	ARactive = false;
	score = 0;
	showCraftRef.exitGame();
}

// Get & set functions
function getARStatus() {
	return ARactive;
}

function getGameStatus() {
	return gameActive;
}

function setGameStatus(active : boolean) {
	gameActive = active;
}

