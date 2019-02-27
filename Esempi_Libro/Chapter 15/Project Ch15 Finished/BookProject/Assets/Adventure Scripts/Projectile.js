#pragma strict
var projectile : GameObject; // the object to instantiate
var speed : float = 20.0; // default speed
var activateRate : float = 0.5; // how often to trigger the action
internal var nextActivationTime : float; // target time


function Start () {

}

function Update () {

	// if the Fire1 button (default is left ctrl) is pressed and the alloted time has passed
	if (Input.GetButton ("Fire1") && Time.time > nextActivationTime) { 
	   nextActivationTime = Time.time + activateRate; // reset the timer
	   Activate(); // do whatever the fire button controls
	}
}

function Activate () {  
   // create a clone of the projectile at the location & orientation of the script's parent     
   var clone : GameObject = Instantiate (projectile, transform.position, transform.rotation);
   // add some force to send the projectile off in its forward direction
   clone.rigidbody.velocity = transform.TransformDirection(Vector3 (0,0,speed));
   // ignore the collider on the object the script is on
   Physics.IgnoreCollision(clone.collider, transform.collider);


}


