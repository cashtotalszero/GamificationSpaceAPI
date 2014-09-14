#pragma strict

function Update () {

	// Test whether any packets are still on tractor beam
	var hit : RaycastHit;
	var fwd : Vector3 = transform.TransformDirection(Vector3.down);
	if (Physics.Raycast (transform.position, fwd, hit)) {
		
		if (hit.collider.gameObject.tag != "Packet") {    	
			return;
		}
		else {
			resetPackets();
		}
	}
	else {
		resetPackets();
	}
}

// Reset all packets back to their original state
function resetPackets() {
	var gos : GameObject[];
	gos = GameObject.FindGameObjectsWithTag("Packet");
	var packet : GameObject;
	
	for(var i=0; i<gos.Length;i++) {
		packet = gos[i];
		packet.name = "";	
	} 
}