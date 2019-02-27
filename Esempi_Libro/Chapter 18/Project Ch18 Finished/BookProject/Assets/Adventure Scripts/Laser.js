#pragma strict

var laser : Transform; // the object with the Line Renderer component
var range : int = 30; // the distance to check within
var hitLight : Light; // light at the end of the laser
var hitParticles : GameObject;// the sparks prefab

var target : GameObject; // target for the lazer to do something when it hits this
var actionObject : GameObject; // object whose animation needs to be triggered
internal var triggered : boolean = false; // flag to prevent multiple triggers

var templeBlocker : Collider; // prevent player from standing under stairs
var fpc : Transform;// first person controller


internal var hit : RaycastHit; // holds some of the properties of the object that is detected with the raycast
 
function Update () {

	// is right mouse button down?
	if(Input.GetButton("ML Enable") )  {// if player is looking around
		// Did we hit anything?
		if (Physics.Raycast (transform.position, transform.forward, hit, range)) { 
			laser.GetComponent(LineRenderer).SetPosition (1,Vector3(0, 0,hit.distance -0.45));//update end position
			if (hitParticles && hit.transform.name == target.name) {  // if particles were assigned, instantiate them at the hit point
				var temp : GameObject = Instantiate(hitParticles, hit.point,Quaternion.FromToRotation(Vector3.up, hit.normal));
				Destroy(temp, 0.3);
				if (!triggered) {
				   // call the target's LookupState function, send its current state, 1, and tell it the default cursor was the picker
				   target.GetComponent(ObjectLookup).LookUpState(target,1,"default");
				}
				triggered = true; // set the flag to prevent more triggers
			}
			hitLight.intensity = 5.0; // turn the intensity up
			hitLight.transform.position = hit.point; // move the light to the hit point
		}
	}
	else {
		//print ("There's nothing directly ahead");
		laser.GetComponent(LineRenderer).SetPosition (1,Vector3(0, 0, 0)); // shorten the laser
		hitLight.intensity = 0.0; // turn the intensity off if there was no hit
	}
}

function DoTheJob () {

    actionObject.animation.Play(); // drop the stairs
   
	templeBlocker.collider.enabled = false; // disable blocker
	// wait until the animation is almost done
	yield new WaitForSeconds(actionObject.animation.clip.length - 0.05);
	GetComponent(Laser).enabled = false; // disable the laser script
	hitLight.gameObject.SetActive(false); // deactivate laser light
	laser.gameObject.SetActive(false); // kill the line renderer
	fpc.position.y = fpc.position.y + 0.1;// "bump" the player up a bit
	animation.Play(); // kill the crystal color	

}
