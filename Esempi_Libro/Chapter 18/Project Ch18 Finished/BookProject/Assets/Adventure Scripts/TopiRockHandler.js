#pragma strict
var gimbok : Collider; // need for collision ignore

function Start () {
	
	// make sure the rock can drop to the tray
	Physics.IgnoreCollision(this.collider, gimbok);
}

function DoTheJob () {

	yield new WaitForSeconds(0.5); // allow the rock to drop and settle
	rigidbody.isKinematic = true; // turn off regular physics

}
