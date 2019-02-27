#pragma strict
var fpc : Transform; // the First Person Controller's transform (Player tag)
var startCollider : Transform; // place holder object for start location (Player tag)
internal var target : Transform; // place holder for the target
internal var startDistance : float = 18.0; // the distance from the target to start walking at
internal var stopDistance : float = 3.0; // the distance from the target to stop at
internal var distance : float; // the current distance between the target and this object
internal var progress : int = 0; //flag for character's progress, 0 = not started

var animator : Animator; // this object's animator component
internal var tempTarget : Vector3;// var for calculating look at

internal var timer : float = 0; // hold the timer's target time
internal var timerOn : boolean = false; // timer flag


function Start () {

	target = fpc; //assign the first  target
	// the starting point/base
	startCollider.position = Vector3(transform.position.x,transform.position.y,transform.position.z);
	animator = GetComponent(Animator); // assign the animator component

}

function Update () {

	if (Time.time >= timer && timerOn) {
	   timerOn = false;
	   BackToStart (); // send the character back to the starting place
	}

	if (progress == -2) return; // in conversation

	distance = Vector3.Distance(target.position, transform.position);
	//print (distance);
	var hit : RaycastHit;// holds information about the hit
	var fwd : Vector3 = transform.TransformDirection (Vector3.forward); //forward direction 
	var offset : Vector3 = transform.position; // var to hold offset position
	offset.y += 1.5; // add the offset to the y element to get eye level
	if (Physics.Raycast (offset, fwd, hit, 30)) { // check for a hit within 30 meters
	   Debug.DrawLine (offset, hit.point); // draw a line to check the position in Scene view
	   //print ("There is a " + hit.collider.transform.name + " ahead");
	   var hitTag = hit.collider.tag; // shorten the tag to a variable
	}
	else hitTag = ""; // assign a value in case it was null
	
	if (hitTag != "Player" && progress == 1) {
	   StartTimer(3.0); // start timer in case player doesn't return to view
	   progress = -1; // paused
	   animator.SetBool("Walk", false); // pause the walk
	} 
			
	if(distance <  startDistance && progress <= 0 && hitTag == "Player") {
	   timerOn = false; //cancel the timer in case it was running
	   progress = 1; // set the flag to walking toward the target
	   animator.SetBool("Walk", true); // trigger the walk
	}
	
	if(distance <  stopDistance && progress == 1) {
	   StartTimer(5.0); // give the player a few seconds to initialize conversation
	   progress = 2; // set the flag to waiting
	   animator.SetBool("Walk", false); // trigger the walk
	}
	
	
	if(progress >= 3 &&  distance <= 0.1) {
	   Reset(); // get the character back to the starting state
	}

		
	// look at code
	tempTarget.x = target.position.x;// use target's x and z
	tempTarget.z = target.position.z;	
	tempTarget.y = transform.position.y;// use this object's y value

	if(animator.GetBool("Walk") == false || progress == 3){ // if the character is not walking...
	   // dampened lookAt
	   transform.rotation = Quaternion.Lerp(transform.rotation, 
	      Quaternion.LookRotation(tempTarget - transform.position), Time.deltaTime * 2.0);
	}
	else transform.LookAt(tempTarget); // use regular LookAt when walking

}

function BackToStart () {

	progress = 3; // heading back
	target = startCollider; // set the target back to the start object
	TurnAni (); // trigger the turn state
	yield new WaitForSeconds(1.1); // give the lookat some turnaround time
	animator.SetBool("Walk", true); // trigger the walk	

}

function StartTimer (seconds : float) { // pass in the time for the timer

   timer = Time.time + seconds;
   timerOn = true;//start the timer 
}

function TurnAbout() {
   animator.SetBool("Walk", false); // stop the walk
   target = fpc; // set the target back to the fpc
   TurnAni (); // trigger the turn state
   yield new WaitForSeconds(2.1); // allow character to settle
}

function Reset() {
   TurnAbout(); // do the turn
   progress = 4; // disabled
   yield new WaitForSeconds(10); // wait before allowing it to start out again 
   progress = 0; // ready to go again
}

function TurnAni () {

	var oldTurn : float = transform.eulerAngles.y;// get the current y rotation
	yield; // allow time to have started turning
	var deltaTurn : float = transform.eulerAngles.y - oldTurn; // get the difference
	if (deltaTurn > 180) deltaTurn -= 360; // degree correction
	if (deltaTurn < -180) deltaTurn += 360; // degree correction
	//print(deltaTurn);
	
	if (deltaTurn > 0 ) animator.SetInteger("Turning", 1); // trigger the turn right
	else animator.SetInteger("Turning", -1); // trigger the turn left
	yield new WaitForSeconds(0.75); // allow turn time
	animator.SetInteger("Turning", 0); // reset the turn flag


}
