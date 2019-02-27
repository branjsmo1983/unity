#pragma strict

var defaultCursor : Texture; // the regular cursor texture, the arrow
var mouseOverColor : Color = Color.green;  // mouse over color for the cursor
internal var currentCursorColor : Color; // current tint color of cursor
internal var currentCursor : Texture; // the current cursor texture 
internal var navigating : boolean = false;   //flag for navigation state
internal var suppressPointer : boolean = false; // flag to suppress cursor after pick
internal var camMatch : boolean = false; // flag to suspend all during camera match
// array to hold the action objects for processing
internal var actionObjects = new GameObject[0]; 

var customSkin : GUISkin; 
var customGUIStyle : GUIStyle; // override the GUISkin
var boxStyleLabel : GUIStyle; // make a label that looks like a box


var useText : boolean = true; // flag to suppress or allow all text
var showText : boolean= false; // flag to toggle text during play
var useLongDesc : boolean = true;// flag to suppress or allow long description
internal var showShortDesc : boolean = true; // flag to toggle short description during play
internal var showLongDesc : boolean = true; // flag to toggle long description during play
internal var shortDesc : String = "";
internal var longDesc : String = "";
internal var actionMsg : String = "";
internal var showActionMsg : boolean = false; 
internal var inRange : boolean; // distance flag for long desc
//internal var actionObject : String; // the name of the last action object to turn on the actionMsg 
internal var pickTimer : boolean; // flag to check timer
internal var pickTimerTime : float; // new time to check for
//internal var tempActionObject : String; // var to hold the emergency reset object name
internal var watchForInput : boolean = false; // flag to watch for mouse movement after action message
internal var lastMousePos : Vector3; // last mouse position 


function Awake () {

    Screen.SetResolution (1280, 800, false);
   
    //get a list of the gameObjects with the ActionObject tag
	var aObjects = GameObject.FindGameObjectsWithTag("ActionObject");
	// redefine the ActionObject array with the number of elements in the list
	actionObjects = new GameObject[aObjects.length];
	//save the action objects into the array for easy access when deactivated
	for (var i : int = 0;i < aObjects.length;i++) {
		actionObjects[i] = aObjects[i];
		//print (actionObjects[i].name);
	}


}

function Start () {

	Screen.showCursor = false; // hide the operating system cursor
	currentCursor = defaultCursor; // assign the default as the current cursor
	currentCursorColor = Color.white; // start color to white
}

function Update () {

	if (watchForInput) { // if the flag is on,
	   if (Input.mousePosition.x != lastMousePos.x) { // if the cursor has moved,
		  //player has moved the mouse, so hide the action text
		  showActionMsg = false;
		  //turn off the flag
		  watchForInput = false;
	   }
	}

   if (pickTimer && Time.time > pickTimerTime -1.5) { // if time is up and flag is on...
      // turn off the flag to suppress the cursor
      suppressPointer = false;
	}
	
   if (pickTimer && Time.time > pickTimerTime) { // if time is up and flag is on...
      pickTimer = false; // turn off the flag 
      // start watching for player mouse movement
	  StartWatching ();
   }

   if (Input.GetAxis("Horizontal") || Input. GetAxis ("Vertical") ||
      Input. GetButton ("Turn") || Input.GetButton("ML Enable") ){
     // a navigation key is being pressed 
    navigating = true; // player is moving
    }
   else {
      navigating = false; // player is stationary
   }

}

function OnGUI () {

	GUI.skin = customSkin;
	
//GUI.Label (Rect (Screen.width/2 - 300, Screen.height - 47, 600, 32), "This is a really long and complicated bit of text, so we can test the text on screen size.");	
		
	if (useText){  //global toggle
	
	   if(showActionMsg) 
   			 GUI.Label (Rect (Screen.width/2 - 300, Screen.height - 75, 600, 32),actionMsg);
	   if (showText && !showActionMsg){ //local toggle
	         if (useLongDesc) {
	             if (showLongDesc && inRange)  
	                 GUI.Label (Rect (Screen.width/2 - 250, Screen.height - 37, 500,35), longDesc);
	            }
	      if (showShortDesc) 
	      GUI.Label (Rect (Screen.width/2 - 250, Screen.height - 65, 500,35), shortDesc, customGUIStyle);
	   }
	}



			
	// cursor control				
	if (!navigating && !suppressPointer && !camMatch) {
		var pos : Vector2 = Input.mousePosition; //get the location of the cursor
		GUI.color = currentCursorColor; // set the cursor color to current
		GUI.DrawTexture (Rect(pos.x, Screen.height - pos.y,64,64), currentCursor);// draw the cursor there
		GUI.color = Color.white; // set the cursor color back to default
	}
	
}

function CursorColorChange (colorize: boolean) {

   if (colorize)  currentCursorColor = mouseOverColor; 

   else  currentCursorColor = Color.white; 
}


function EmergencyTimer (dyingObject : String) {

   //tempActionObject = dyingObject; // assign the name of the dying object to the temp var
   // start the pickTimer
   pickTimerTime = Time.time + 2.5; // set the timer to go for 2.5 seconds
   pickTimer = true; // turn on the flag to check the timer   
}

function StartWatching () {

   watchForInput = true; // turn on the watching flag
   lastMousePos = Input.mousePosition; // record the latest cursor position
   
}
