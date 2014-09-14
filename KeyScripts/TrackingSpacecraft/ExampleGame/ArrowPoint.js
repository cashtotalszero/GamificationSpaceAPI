#pragma strict

private var spacecraft : GameObject;

// Called once per frame
function Update() {
	
	// Point the arrow at the game object with the tag 'selectedCraft'
	if (GameObject.FindWithTag('selectedCraft') != null) {
		spacecraft = GameObject.FindWithTag('selectedCraft');
		transform.LookAt(spacecraft.transform.position);
	}
}