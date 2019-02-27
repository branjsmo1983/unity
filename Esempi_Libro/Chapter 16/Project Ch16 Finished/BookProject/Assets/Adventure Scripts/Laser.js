#pragma strict

var laser : Transform; // the object with the Line Renderer component
var range : int = 30; // the distance to check within
var hitLight : Light; // light at the end of the laser
var hitParticles : GameObject;// the sparks prefab

internal var hit : RaycastHit; // holds some of the properties of the object that is detected with the raycast
 
function Update () {

	// is right mouse button down?
	if(Input.GetButton("ML Enable") )  {// if player is looking around
		// Did we hit anything?
		if (Physics.Raycast (transform.position, transform.forward, hit, range)) { 
			laser.GetComponent(LineRenderer).SetPosition (1,Vector3(0, 0,hit.distance -0.45));//update end position
			if (hitParticles && hit.collider.tag == "ActionObject") { // if particles were assigned, instantiate them at the hit point
				var temp : GameObject = Instantiate(hitParticles, hit.point,Quaternion.FromToRotation(Vector3.up, hit.normal));
				Destroy(temp, 0.3);
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
