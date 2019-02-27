#pragma strict

@script RequireComponent(AudioSource)
@script RequireComponent(ObjectLookup)
var cursor : String = "default"; // temporary cursor for testing LookupState 

//state dependent variables
var initialState : int = 1; // this is the state the object starts in
var currentState : int = 1; // this will get updated in Start
internal var currentLocation : int;  // see notes
internal var currentVisibility : int; //see notes 
internal var currentObjectName : String; // short description
internal var currentObjectDescription : String; // full description
internal var currentSound : AudioClip;
internal var currentAudioDelay : float = 0.0;
internal var currentAnimationClip : AnimationClip;
internal var currentAnimationDelay : float = 0.0;


//Object metadata
var location : int[];	 // see notes
var visibility : int[];   // see notes
var objectName : String[];  // name/label of the object in this state 
var description : String[];  // description of the object in this state
var animationClip : AnimationClip[];    // the animation that will play when picked
var animationDelay : float[];   // the time delay before the animation plays
var soundClip : AudioClip[];    // the sound that gets played when picked
var audioDelay : float[]; // the time delay before the audio plays
var loopAnimation : AnimationClip[]; //animation that loops after main animation
var loopSoundFX : AudioClip[]; // sound that goes with it
var postLoop : boolean = false;  // flag to know if it has a looping animation to follow
var animates : boolean = true; // var to know if it animates at all
var aniObject : GameObject; // object that holds the animation component for this object



// Pick and Mouseover Info
internal var triggerDistance : float = 7.0; // distance the camera must be to the object before mouse over
internal var picked : boolean = false;  // so you can temporarily prevent mouse over action 
internal var mousedown : boolean; // so you know when this is true
internal var processing : boolean = false; //so you can suspend mouse over actions, etc
var moOffset : float = 8.0;  // additional distance to allow mouse over to be seen

// Gain access to these objects
internal var controlCenter : GameObject; // the Control Center object
internal var cam : GameObject; // the main camera on the first person controller

//Misc vars
internal var pickTimer : boolean; // flag to check timer
internal var pickTimerTime : float; // new time to check for


function Start () {

	controlCenter = GameObject.Find("Control Center"); // locate and assign the object to the var
    cam = GameObject.Find("Main Camera"); // find “Main Camera” and assign it to the cam var

	// load the initial values 
	currentState = initialState; // this allows override of starting state only
	currentObjectName = objectName[currentState];
	currentObjectDescription = description[currentState];  
	currentLocation = location[currentState];
	currentVisibility = visibility[currentState];


}

function Update () {

   if (pickTimer && Time.time > pickTimerTime) { // if time is up and flag is on...
      pickTimer = false; // turn off the flag 
      // turn off the flag to suppress the cursor
      controlCenter.GetComponent(GameManager).suppressPointer = false;
   } 
}


function OnMouseEnter () {
	//print (DistanceFromCamera());
	// exit if you are not within range 
	if (DistanceFromCamera() > triggerDistance + moOffset) return;
	
	if (processing) return; // object is being processed, leave the function

    controlCenter.SendMessage("CursorColorChange", true); // colorize the pointer
}

function OnMouseExit () {

	if (processing) return; // object is being processed, leave the function

   controlCenter.SendMessage("CursorColorChange", false); // turn the pointer white

}


function OnMouseDown () {

	// exit if you are not within range
	if (DistanceFromCamera() > triggerDistance) return;
    print ("we have mouse down on " + this); 
    
    if (processing) return; // object is being processed, leave the function
     
	// *** send a message turn the cursor color back to original 
	controlCenter.SendMessage("CursorColorChange", false); // turn the pointer white; 
	 
	//send the picked object, its current state, and the cursor [its texture name] 
	//that picked it directly to the LookUpState function for processing
	GetComponent(ObjectLookup).LookUpState(this.gameObject,currentState,cursor); 

}

function ProcessObject (newState : int) {

	processing = true; // turn on flag to block picks
   // tell the GameManager to suppress the cursor
   controlCenter.GetComponent(GameManager).suppressPointer = true;
   // start timer
   pickTimerTime = Time.time + 0.5; // set the timer to go for 0.5 seconds
   pickTimer = true; // turn on the flag to check the timer

	currentState = newState; // update the object's current state

   // update more of the data with the new state 
   currentObjectName = objectName[currentState];
   currentObjectDescription = description[currentState];
   currentLocation = location[currentState];
   currentVisibility = visibility[currentState];
   
      // assign the current clip and delay and audio for the new state
   if (animates) currentAnimationClip = animationClip[currentState];
   if (animates) currentAnimationDelay = animationDelay[currentState];
   currentSound = soundClip[currentState];
   currentAudioDelay = audioDelay[currentState];

   if(animates && animationClip[currentState] != null) {
		// find out if a alternate animation object was assigned, if not, assign the object itself
		if (aniObject == null) aniObject = gameObject;
		//pause before playing the animation 
		yield new WaitForSeconds(currentAnimationDelay);
		// play the animation	
		aniObject.animation.Play(currentAnimationClip.name);
	
   		ProcessAudio (currentSound); // send audio clip off for processing
   		
		// wait the length of primary animation clip
		yield new WaitForSeconds(currentAnimationClip.length);
		
		
   		// check for a looping animation to follow the primary animation
		if(postLoop) { // if postLoop is checked/ true, there is a looping animation to trigger
 		   aniObject.animation.Play(loopAnimation[currentState].name); // play the looping animation
		   ProcessAudio (loopSoundFX[currentState]); // send loop audio clip off for processing
		}
		processing = false; // turn off flag to block picks
   	}
   	else  {
   		ProcessAudio (currentSound); // send loop audio clip off for processing
   		yield new WaitForSeconds(1.0);// give a short pause if there was no animation
		processing = false; // turn off flag to block picks
   	}	
}


function DistanceFromCamera () {

   // get the direction the camera is heading so you only process stuff in the line of sight
   var heading : Vector3 = transform.position - cam.transform.position;
   //calculate the distance from the camera to the object 
   var distance : float = Vector3.Dot(heading, cam.transform.forward);
   return distance;
}


function ProcessAudio (theClip : AudioClip) {

   if(theClip)  { // if there is a sound clip
	//check to make sure an Audio Source component exists before playing
	if (GetComponent(AudioSource)) {
		audio.clip = theClip; // change the audio component's assigned sound file
		audio. 
		audio.PlayDelayed (currentAudioDelay); // delay before playing it	 
	}
   }
}
