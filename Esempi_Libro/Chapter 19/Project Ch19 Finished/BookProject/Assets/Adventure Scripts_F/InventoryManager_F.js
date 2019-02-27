#pragma strict

var iMode = false; // local flag for whether inventory mode is off or on
internal var controlCenter : GameObject;
var fPController : GameObject;
var fPCamera : GameObject;
internal var restricted : boolean; // flag for no more inventory access ###
internal var blocked : boolean; // flag for no inventory access during start of end sequence ###

function Start () {

   camera.enabled = false;
   controlCenter = GameObject.Find("Control Center2");// access the control center
   iMode = controlCenter.GetComponent(GameManager_F).iMode;
}


function Update () {

	if (Input.GetKeyDown("i")) ToggleMode(); // call the function if i key is pressed 
}

// toggle inventory visibility
function ToggleMode () {

	if(blocked) return; // blocks during start of final sequence ###

	// if there is a menu open, inventory is not allowed
	if(controlCenter.GetComponent(MenuManager_F).menuMode) return;
 
  if (iMode) { // if you are in inventory mode, turn it off
      camera.enabled = false;// turn off the camera
      // unblock navigation
    if (!restricted) { //###
	   fPController.GetComponent(CharacterMotor).enabled = true; // turn on navigation
	   fPController.GetComponent(FPAdventurerInputController_F).enabled = true; // turn on navigation
	   fPController.GetComponent(MouseLookRestricted_F).enabled = true; // turn on navigation
	   fPCamera.GetComponent(MouseLookRestricted_F).enabled = true;
	}

      iMode = false; // change the flag
      controlCenter.GetComponent(GameManager_F).iMode = false; // inform the game manager
      controlCenter.GetComponent(MenuManager_F).iMode = false; // inform the menu manager //###
  }
  else { // else it was off so turn it on
  
  	StashViewObjects (); // put away any view objects that are showing
    camera.enabled = true;// turn on the camera
	// block navigation
	if(!restricted) { //###
	   fPController.GetComponent(CharacterMotor).enabled = false; // turn off navigation
	   fPController.GetComponent(FPAdventurerInputController_F).enabled = false; // turn off navigation
	   fPController.GetComponent(MouseLookRestricted_F).enabled = false; // turn off navigation
	   fPCamera.GetComponent(MouseLookRestricted_F).enabled = false;
    }
      iMode = true; // change the flag
      controlCenter.GetComponent(GameManager_F).iMode = true; // inform the game manager
      controlCenter.GetComponent(MenuManager_F).iMode = true; // inform the menu manager //###
  }

}

function DoTheJob () {

   if(iMode) ToggleMode ();
   // activate Camera View if it wasn't already
   gameObject.Find("Camera View").camera.enabled= true;

}

function StashViewObjects () {
   //load the actionObject array from the GameManager script
   var actionObjects : GameObject[] = controlCenter.GetComponent(GameManager_F).actionObjects;
   for (var y : int = 0; y < actionObjects.length; y++) { // iterate through the array
      if (actionObjects[y].gameObject.layer == 10 &&   // if there is a match for the In View layer
         actionObjects[y].gameObject.activeInHierarchy) { // and it is active in the scene
         // process it out of the scene with ObjectLookup, it's in state 1, picked by the default cursor
         actionObjects[y].gameObject.GetComponent(ObjectLookup_F).
            LookUpState(actionObjects[y].gameObject,1,"default"); 
      } // close the if
   } // close the for loop
}

