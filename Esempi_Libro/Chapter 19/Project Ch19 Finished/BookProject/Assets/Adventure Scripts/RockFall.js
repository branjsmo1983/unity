#pragma strict

// Instantiates prefab when any rigid body enters the trigger.
// It preserves the prefab's original position and rotation.
var rocks : GameObject[];  // create an array to hold the rocks
internal var arrayLength : int; // var to hold number of elements in the array
var prize : GameObject; // the crystal
var pileTime : float = 10.0; // time to let rocks drop
var startTimer : boolean = false; // flag for timer after first rock drop



function Start () {
   arrayLength = rocks.length; // number of elements in the array
}


function OnTriggerEnter (object : Collider) {

   if( object == prize.collider) Destroy(this.gameObject); //prevent rock fall

   
	// get a random number between 0 and the length of the array
	var num = Random.Range(0, arrayLength); 
	//instanciate that element
	Instantiate (rocks[num]);
	if(!startTimer) DropPrize (); // start the timer function

}

function DropPrize () {

	startTimer = true; // timer running
	yield new WaitForSeconds(pileTime + 0.5); // allow extra to let rocks settle
	//activate prize
	prize.SetActive(true);
	prize.GetComponent(Interactor).currentState = 1; // manually change its state
	yield; // wait a frame
	Destroy(this.gameObject); // terminate the Rock Zone object 

}
