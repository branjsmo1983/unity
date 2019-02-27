private var motor : CharacterMotor;
internal var blockInput : boolean = false; // flag to prevent user navigation
internal var blockerTimer : boolean; // flag to check timer
internal var blockerTimerTime : float; // new time to check for

//add these for arrow turn
var rotationSpeed : float = 20.0;
internal var rotationSensitivity = 0.1 ; // This makes rotationSpeed more managable 


// Use this for initialization
function Awake () {
	motor = GetComponent(CharacterMotor);
}

// Update is called once per frame
function Update () {

	// block navigation input
	if(blockInput) Input.ResetInputAxes();
	// timer for blocking navigation
	if (blockerTimer && Time.time > blockerTimerTime) { // if time is up and flag is on...
		blockerTimer = false; // turn off the flag 
		// turn off the flag to block user input
		blockInput = false; // turn off the blocker
	} 

	// Get the input vector from kayboard or analog stick
	var directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
	//if(Input.GetAxis("Turn")) print ("turning " + Input.GetAxis("Turn"));   
	if (directionVector != Vector3.zero) {
		// Get the length of the directon vector and then normalize it
		// Dividing by the length is cheaper than normalizing when we already have the length anyway
		var directionLength = directionVector.magnitude;
		directionVector = directionVector / directionLength;
		
		// Make sure the length is no bigger than 1
		directionLength = Mathf.Min(1, directionLength);
		
		// Make the input vector more sensitive towards the extremes and less sensitive in the middle
		// This makes it easier to control slow speeds when using analog sticks
		directionLength = directionLength * directionLength;
		
		// Multiply the normalized direction vector by the modified length
		directionVector = directionVector * directionLength;
	}
	
	// Apply the direction to the CharacterMotor
	motor.inputMoveDirection = transform.rotation * directionVector;
	motor.inputJump = Input.GetButton("Jump");
}

function ToggleInput (block : boolean) {

	blockInput = block;
}


//this is for the arrow turn functionality
function FixedUpdate () {
	 // block navigation input
	if(blockInput) Input.ResetInputAxes();

   if (Input.GetAxis("Turn")) {    // the left or right arrow key is being pressed
      // the rotation = direction * speed * sensitivity
      var rotation : float = ( Input.GetAxis("Turn") ) * rotationSpeed * rotationSensitivity ;
      // add the rotation to the current orientation amount
      rotation = rotation + transform.eulerAngles.y ;
      // convert degrees to quaternion for the up axis, Y
      transform.localRotation = Quaternion.AngleAxis ( rotation, Vector3.up ) ;

   }
}

function ManageInput (aniTime: float) {
   // add the aniTime to the current time
   aniTime = Time.time + aniTime/2;
   // check to see if it is greater than the current time
   // if it is, update the timer target time
   if (aniTime > blockerTimerTime) blockerTimerTime = aniTime;
   if (blockerTimerTime > Time.time) { 
	// turn on the flag to block user input
	blockInput = true; // turn on the blocker if it isn't already on
	blockerTimer = true; // turn on timer if it isn't already on
   }
}


// Require a character controller to be attached to the same game object
@script RequireComponent (CharacterMotor)
@script AddComponentMenu ("Character/FPS Input Controller")
