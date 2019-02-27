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
var originalMaterial : Material;// store the object's material
var originalColor : Color;// store the object's material's color
var alphaColor : Color;// store color's transparent version
internal var aniLength : float; // temp var for animation clip length

//fade variables
var useAlpha : boolean = true; // flag to fade material during visibility change
internal var fadeIn : boolean = false; // flags to assign end & out colors
internal var fadeOut : boolean = false; // 
internal var fadeTime : float = 2.0; //default time over which the fades happen
internal var startColor : Color; // variables for color/opacity transition
internal var endColor : Color; 
internal var ent : float = 1; // elapsed normalized time for fade, 1 = 100% or finished
internal var fadeTimer : boolean; // flag to check timer
internal var fadeTimerTime : float; // new time to check for


function Start () {

	controlCenter = GameObject.Find("Control Center"); // locate and assign the object to the var
    cam = GameObject.Find("Main Camera"); // find “Main Camera” and assign it to the cam var

		// Material and fades section
	if(GetComponent(MeshRenderer)) {// if the object has a MeshRenderer, get its material
		originalMaterial = GetComponent(MeshRenderer).material;
		//print (this + "  " + "Shader: " + renderer.material.shader.name + "  " + renderer.material.color.a);
		// prep for auto fade by checking shader, unless specified as false
		if (useAlpha) { // if it isn't set to false by the author
		   useAlpha = false; // set it to false, there will be only one condition to make it true
		   var tempShadername : String = renderer.material.shader.name; //get the shader name,a String
		   if (tempShadername.length > 11) { //check for short names- they aren't transparent shaders
		      // check to see if the material's shader is a Transparency shader
		      if(tempShadername.Substring (0,11) == "Transparent") { 
			  // set the flag to use alpha for changing opacity during a transitions
			  useAlpha = true;
			  // get fade colors
			  originalColor = renderer.material.color; // the material's original color
			  alphaColor = Color(originalColor.r,originalColor.g,originalColor.b,0);// alpha valued color
		      }
		   }
		}
	}

	
	
	// load the initial values 
	currentState = initialState; // this allows override of starting state only
	currentObjectName = objectName[currentState];
	currentObjectDescription = description[currentState];  
	currentLocation = location[currentState];
	currentVisibility = visibility[currentState];

	// if the object's current location is not in scene, 0, deactivate it 
	if (currentState == 0) {
		// if it is set to use fades, make it transparent before deactivating it
		if(useAlpha) renderer.material.color = alphaColor; //assign the alpha version as its color
		gameObject.SetActive (false); // deactivate the object
	}

}

function Update () {

   if (pickTimer && Time.time > pickTimerTime) { // if time is up and flag is on...
      pickTimer = false; // turn off the flag 
      // turn off the flag to suppress the cursor
      controlCenter.GetComponent(GameManager).suppressPointer = false;
   }

	// timer for visibility fades
	if (fadeTimer && Time.time > fadeTimerTime) { // if time is up and flag is on...
		fadeTimer = false; // turn off the flag 
		if(!useAlpha) gameObject.SetActive (false); // deactivate the object
		// set up for the fade out
		endColor = alphaColor;
		startColor = originalColor;
		ent = 0; // start the fade
		 	
	}

   if(!useAlpha) return; // skip the fade code if it is not being used
   else {
		if(fadeIn) {
			startColor = alphaColor;
			endColor = originalColor;
		}
		
		if(fadeOut) {
			endColor = alphaColor;
			startColor = originalColor;
		}
		if (ent < 1) {	
		   // calculate the current color using linear interpolation between start & end while it is less than 1
		   renderer.material.color = Color.Lerp(startColor, endColor, ent);
		  // increase the ent value using the rate
		   ent = ent + Time.deltaTime/fadeTime;
		   //deactivate the object if it is finished fading out
		   if (ent >= 1 && endColor == alphaColor) gameObject.SetActive (false);

		} //end the if/ent
	} // end the else
}


function OnMouseEnter () {
	//print (DistanceFromCamera());
	
	// if there is an active camera match in process, exit the function now 
	if (controlCenter.GetComponent(GameManager).camMatch) return;

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

	if(location[currentState] == 2) return; // object is not pickable in scene

	// if there is an active camera match in process, exit the function now 
	if (controlCenter.GetComponent(GameManager).camMatch) return;

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
   
   // if there is a clip, add its length and delay together
   if(currentAnimationClip) aniLength = currentAnimationClip.length + currentAnimationDelay;
   else aniLength = 0.0; // there is no animation for this state
   
   currentSound = soundClip[currentState];
   currentAudioDelay = audioDelay[currentState];
   
	// send it off for handling in case it has a visibility state that is processed at the start
	HandleVisibility();
	
	// block player input while action plays out
	if (aniLength != 0.0) {
	    gameObject.Find("First Person Controller").SendMessage("ManageInput",aniLength);
	}


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
   		//print (currentSound ); 
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
		audio.PlayDelayed (currentAudioDelay); // delay before playing it	 
	}
   }
}

function HandleVisibility () {

	switch (currentVisibility) {
	
	  case 0 : // deactivate at start 
	      if(currentLocation == 0 ) {
	        // turn off the timer for the cursor before deactivating the object
			pickTimerTime = Time.time; // set timer to now to force it to finish
			yield; // give timer a chance to stop	
			if(currentSound != null) {
			   ProcessAudio (currentSound); // send audio clip off for processing first
			   yield new WaitForSeconds(currentSound.length); // allow time to play the sound clip
			}
			gameObject.SetActive(false); // deactivate the object immediately
	      }
	      break;
	      
	  case 1: // currentVisibility is 1, Show at start
	  	      print ("here at 1");
	     if (useAlpha) {
			startColor = alphaColor; // assign the start color 
			endColor = originalColor; // assign the end color	
			ent = 0; // start the fade
		 }
	     break;

	
	  case 2: // currentVisibility is 2, Show at start, hide at end
		   if (useAlpha) {
			startColor = alphaColor; // assign the start color 
			endColor = originalColor; // assign the end color	
			ent = 0; // start the fade
		   }
		   // set up for fade out
			if (aniLength == 0.0) aniLength = 2.0; // at least let it show a couple of seconds before the fade
			fadeTimerTime = Time.time + aniLength;
			fadeTimer = true; // turn on the flag to check the timer
      	break;	

	
	  case 3: // currentVisibility is 3, hide at end
			// set up for fade out
			if (aniLength == 0.0) aniLength = 2.0; // at least let it show a couple of seconds before the fade
			fadeTimerTime = Time.time + aniLength;
			fadeTimer = true; // turn on the flag to check the timer			
		break;	

	
	}

}
