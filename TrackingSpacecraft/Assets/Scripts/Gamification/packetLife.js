#pragma strict

private var life : int;
public var playerExplosion : GameObject;
public var maxLifespan : int;

// Called once when prefab is first instanciated
function Awake () {
	
	// Initialise life
	life = 0;
}

// Called at each physics frame update
function FixedUpdate() {
	
	// Increment the object life counter with each frame
	if (life < maxLifespan) {
		life++;
	}
	// If the packet has reached the max lifespan and hasn't been rescued - destroy it
	else {
		Instantiate(playerExplosion, transform.position, transform.rotation);
    	Destroy(gameObject);
	}
}