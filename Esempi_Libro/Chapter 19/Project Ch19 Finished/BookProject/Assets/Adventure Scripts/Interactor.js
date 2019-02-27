#pragma strict

@script RequireComponent(AudioSource)
@script RequireComponent(ObjectLookup)
var cursor : String = "default"; // temporary cursor for testing LookupState 

//state dependent variables
var initialState : int = 1; // this is the state the object starts in
var currentState : int = 1; // this will get updated in Start
var iElement = 100; // holds the object's element in inventory number, 100 = not state 1
internal var currentLocation : int;  // see notes
internal var currentVisibility : int; //see notes 
internal var currentObjectName : String; // short description
internal var currentObjectDescription : String; // full description
internal var currentSound : AudioClip;
internal var currentAudioDelay : float = 0.0;
internal var currentAnimationClip : AnimationClip;
internal var currentAnimationDelay : float = 0.0;
internal var previousState :int; // need for 2D objects
internal var element = 0; // element number when in inventory



//Object metadata
var objectIs3D = true; //flag to identify GUI Texture objects
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
internal var iMode : boolean; // inventory mode flag
var messageRead : float = 2.5; // time allowed for reading action message
internal var cursorReturn : float; // time before cursor returns after pick
internal var loading = false; // flag for alternate processing of object state

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

internal var menuMode : boolean; // menu mode flag

var saveTransforms : boolean = false; // flag to save current transform for save/load

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


   if (pickTimer && Time.time > pickTimerTime -cursorReturn ) { // if time is up and flag is on...
      // turn off the flag to suppress the cursor
      controlCenter.GetComponent(GameManager).suppressPointer = false;
	}
	
   if (pickTimer && Time.time > pickTimerTime) { // if time is up and flag is on...
      pickTimer = false; // turn off the flag 
      //tell the GameManager to start watching for player mouse movement
	  controlCenter.GetComponent(GameManager).StartWatching ();
	  processing = false;//
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


function OnMouseOver () {

	if(location[currentState] == 2) return; // object is not pickable in scene
	
	// if there is an active camera match in process, exit the function now 
	if (controlCenter.GetComponent(GameManager).camMatch) return;

	// exit if you are not within range and it is a 3d object
	if (objectIs3D && DistanceFromCamera() > triggerDistance + moOffset) return;
	
	if (processing) return; // object is being processed, leave the function
	
	menuMode = controlCenter.GetComponent(MenuManager).menuMode; // check for active menus
	if (menuMode) return;
	
	iMode = controlCenter.GetComponent(GameManager).iMode; // get current iMode
	if (iMode  && gameObject.layer != 9) return; // return if the mouse was not over and inventory object and you are in inventory mode
	
	if (!objectIs3D)  guiTexture.color = Color(0.75, 0.75, 0.75,1); // brighten the 2D icon a little

	//activate the text visibility on mouseover
	controlCenter.GetComponent(GameManager).showText = true;
	//send the correct text to the GameManager for display
	controlCenter.GetComponent(GameManager). shortDesc = objectName[currentState];
	controlCenter.GetComponent(GameManager).longDesc = description[currentState];

	// automatic bypass flag
	if(!objectIs3D || DistanceFromCamera() <= triggerDistance) {
	        controlCenter.GetComponent(GameManager).inRange = true;}
	else  controlCenter.GetComponent(GameManager).inRange = false;
	
	

    controlCenter.SendMessage("CursorColorChange", true); // colorize the pointer
}


function OnMouseExit () {

	if (processing) return; // object is being processed, leave the function

	if (!objectIs3D) guiTexture.color = Color.grey;	
			
	//deactivate the text visibility on mouseover
	controlCenter.GetComponent(GameManager).showText = false;

	//deactivate the text visibility on mouseover
	controlCenter.GetComponent(GameManager).showText = false;

   controlCenter.SendMessage("CursorColorChange", false); // turn the pointer white

}


function OnMouseDown () {

    if (processing) return; // object is being processed, leave the function
    if (menuMode) return;
    if (iMode  && gameObject.layer != 9) return; // currently in inventory mode 
    
	if(location[currentState] == 2) return; // object is not pickable in scene

	// if there is an active camera match in process, exit the function now 
	if (controlCenter.GetComponent(GameManager).camMatch) return;

	// exit if you are not within range and it is a 3D object
	if (objectIs3D && DistanceFromCamera() > triggerDistance + moOffset) return;
    //print ("we have mouse down on " + this); 
	// if the player is within mouseover  but not picking distance...
	if (objectIs3D && DistanceFromCamera()  > triggerDistance ) {
	   var tempMsg : String = "You are too far from the " + objectName[currentState].ToLower() + " to interact with it";
	   //send the GameManager the action text
	   controlCenter.GetComponent(GameManager).actionMsg = tempMsg;
	   //tell the GameManager to show the action text
	   controlCenter.GetComponent(GameManager).showActionMsg = true;
	   //wait two seconds then turn off the action text and then leave the function
	   yield new WaitForSeconds(2.0);
	   controlCenter.GetComponent(GameManager).showActionMsg = false;
	   return;
	}
    
	// restore the 2D object color
	if (!objectIs3D)  guiTexture.color = Color.grey;    
      
	// *** send a message turn the cursor color back to original 
	controlCenter.SendMessage("CursorColorChange", false); // turn the pointer white; 
	
	// get and assign the current cursor from the GameManager 
	cursor = controlCenter.GetComponent(GameManager).currentCursor.name;
	if (cursor == controlCenter.GetComponent(GameManager).defaultCursor.name) cursor = "default";
	 
	//send the picked object, its current state, and the cursor [its texture name] 
	//that picked it directly to the LookUpState function for processing
	GetComponent(ObjectLookup).LookUpState(this.gameObject,currentState,cursor); 

}

function ProcessObject (newState : int) {

	processing = true; // turn on flag to block picks
	
	//handle replace
	if(newState == 10) {
	   newState = 1;
	   currentState = 10;
	}
	 if (newState == 11) {
	   newState = 2;
	   currentState = 11; 
	}


   // tell the GameManager to suppress the cursor
   controlCenter.GetComponent(GameManager).suppressPointer = true;
    
   //deactivate the text messages
   controlCenter.GetComponent(GameManager).showText = false;
   
	//tell the GameManager to show the action text
	controlCenter.GetComponent(GameManager).showActionMsg = true;

    
   // start timer
   if (objectIs3D) cursorReturn = 1.5; // longer pause for in 3d scene
   else cursorReturn = 2.0; // shorter pause for in inventory
   pickTimerTime = Time.time + messageRead; // set the timer to go for 2.5 seconds
   pickTimer = true; // turn on the flag to check the timer
   //tell the GameManager which object just started the pickTimer
   //controlCenter.GetComponent(GameManager).actionObject  = this.name;

	previousState = currentState; // store the previous state before updating
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
	if (objectIs3D) HandleVisibility();
	else Handle2D(); // send 2D objects off for processing
	
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
		if (loading) { 
		    // set the animate time to 0, play the animation, then set it back to normal
		    aniObject.animation[currentAnimationClip.name].normalizedTime = 0.0;
		    aniObject.animation.Play(currentAnimationClip.name);
		    aniObject.animation[currentAnimationClip.name].normalizedTime = 1.0;
		}
		else { // else it is not loading so process as usual 
   			aniObject.animation.Play(currentAnimationClip.name);
   		}

	
   		if (!loading) ProcessAudio (currentSound); // send audio clip off for processing
   		
		// wait the length of primary animation clip
		yield new WaitForSeconds(currentAnimationClip.length);
		
		
   		// check for a looping animation to follow the primary animation
		if(postLoop) { // if postLoop is checked/ true, there is a looping animation to trigger
 		   aniObject.animation.Play(loopAnimation[currentState].name); // play the looping animation
		   ProcessAudio (loopSoundFX[currentState]); // send loop audio clip off for processing
		}
		//processing = false; // turn off flag to block picks
   	}
   	else  {
   		
   		if (!loading) ProcessAudio (currentSound); // send loop audio clip off for processing
   		//print (currentSound ); 
   		yield new WaitForSeconds(1.0);// give a short pause if there was no animation
		//processing = false; // turn off flag to block picks
   	}	

	loading = false;   	
}


function DistanceFromCamera () {

	//fake the distance for Camera View objects 
	if(gameObject.layer == 10) return 1.0; 

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
	        controlCenter.GetComponent(GameManager).EmergencyTimer(this.name);
			yield; // make sure the action is carried out	
			if(currentSound != null) {
			   ProcessAudio (currentSound); // send audio clip off for processing first
			   yield new WaitForSeconds(currentSound.length); // allow time to play the sound clip
			}
			gameObject.SetActive(false); // deactivate the object immediately
	      }
	      break;
	      
	  case 1: // currentVisibility is 1, Show at start
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

//handle 2D objects
function Handle2D () {
   
   // Not in scene -> Is Cursor
   if (previousState == 0 && currentState == 2) {
      controlCenter.GetComponent(GameManager).currentCursor = guiTexture.texture;
      gameObject.guiTexture.enabled = false;
  }

   // Not in scene -> In Inventory
   if (previousState == 0 && currentState == 1) {
      controlCenter.SendMessage("AddToInventory", gameObject);
      gameObject.guiTexture.enabled = true;
  }

   // Is Cursor -> Not in scene
   if (previousState == 2 && currentState == 0) {
      controlCenter.SendMessage("ResetCursor"); 
      yield; 
      gameObject.SetActive(false); // deactivate the object immediately 
  }
   // Is Cursor -> In Inventory
   if (previousState == 2 && currentState == 1) {
      controlCenter.SendMessage("AddToInventory", gameObject);
      gameObject.guiTexture.enabled = true;
      controlCenter.SendMessage("ResetCursor");
  }
   // In Inventory -> Not in scene
   if (previousState == 1 && currentState == 0) {
      gameObject.guiTexture.enabled = false;
      controlCenter.SendMessage("RemoveFromInventory", gameObject);
      yield;
      gameObject.SetActive(false); // deactivate the object immediately
  }

   // In Inventory -> Is Cursor
   if (previousState == 1 && currentState == 2) {
      gameObject.guiTexture.enabled = false;
      controlCenter.SendMessage("RemoveFromInventory", gameObject);
      controlCenter.GetComponent(GameManager).currentCursor = guiTexture.texture;
  }
  
	// In Inventory, will be replaced 
	if (previousState == 10) { 
	   //turn off the object
	   gameObject.guiTexture.enabled = false;
	   //turn it into the cursor
	   controlCenter.GetComponent(GameManager).currentCursor = guiTexture.texture;
	   //update its iElement to not state 1
	   iElement = 100;
	}
	
	//the new object that takes the picked object's position
	if (previousState == 11) { 
	   controlCenter.SendMessage("ResetCursor");  
	   //set its state to inventory
	   currentState = 1;
	   gameObject.guiTexture.enabled = true;
	   //get the element number in inventory that it replaces
	   iElement = controlCenter.GetComponent(GameManager).replaceElement;
	   yield; // make sure the value has time to update
	   //update the currentInventoryObjects array
	   controlCenter.GetComponent(GameManager).OrderInventoryArray(); 
	   //update the inventory grid 
	   controlCenter.GetComponent(GameManager).InventoryGrid();
	}

	if(previousState == 0 && currentState == 0) gameObject.SetActive(false);
}
