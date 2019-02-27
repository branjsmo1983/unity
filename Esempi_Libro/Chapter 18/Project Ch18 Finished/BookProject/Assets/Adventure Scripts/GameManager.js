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
// array to hold the Inventory objects for processing
internal var inventoryObjects = new GameObject[0]; 


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

internal var conversing  : boolean = false; // flag to suppress text & cursor color change 

internal var iMode = false; // flag for whether inventory mode is off or on

// array of objects currently in inventory
internal var currentInventoryObjects = new GameObject[0];// nothing in it yet
internal var iLength : int; // var to store the array's length

var gridPosition = 0; // the default position of the inventory grid

// inventory layout
internal var startPos = 140;
internal var iconSize = 90;
internal var replaceElement : int; // var to store the current inventory object's element


function Awake () {

    Screen.SetResolution (1280, 800, false);
   
    //get a list of the gameObjects with the ActionObject tag
	var aObjects = GameObject.FindGameObjectsWithTag("ActionObject");
	// redefine the actionObject array with the number of elements in the list
	actionObjects = new GameObject[aObjects.length];
	//save the action objects into the array for easy access when deactivated
	for (var i : int = 0;i < aObjects.length;i++) {
		actionObjects[i] = aObjects[i];
		//print (actionObjects[i].name);
	}

	//get a list of the gameObjects with the Inventory tag
	var iObjects = GameObject.FindGameObjectsWithTag("InventoryObject");
	// redefine the inventoryObject array with the number of elements in the list
	inventoryObjects = new GameObject[iObjects.length];
	//save the inventory objects into the array for easy access when deactivated
	for (var k : int = 0;k < iObjects.length;k++) {
		inventoryObjects[k] = iObjects[k];
		//print (inventoryObjects[k].name);
	}
}

function Start () {

	Screen.showCursor = false; // hide the operating system cursor
	currentCursor = defaultCursor; // assign the default as the current cursor
	currentCursorColor = Color.white; // start color to white
	
	//get a list of the gameObjects with the InventoryObject tag
	var iObjects = GameObject.FindGameObjectsWithTag("InventoryObject");
	
	var element = 0; // initialize a counter for ordering active inventory objects
	
	//assign objects a position number for the current inventory array
	for (var i : int = 0;i < iObjects.length;i++) {
	//only if its current state is 1 ( in inventory)
	   if(iObjects[i].GetComponent(Interactor).initialState == 1) {
	   // assign the element number to the current object
	   iObjects[i].GetComponent(Interactor).iElement = element; 
	   element ++; //increment the element
	   }
	   //else it's initial state was 2, as cursor, assign 100, state 0 has already been deactivated 
	   else iObjects[i].GetComponent(Interactor).iElement = 100;
	   
	   //print (iObjects[i].GetComponent(Interactor).name + "  "  + iObjects[i].GetComponent(Interactor).iElement);
	}
	iLength = element;  // save the number of elements, the number of state 1 objects
	OrderInventoryArray (); // load the currentInventoryObjects array
	InventoryGrid (); // update layout
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


	// handle dropped cursors
	// if the left button, 0, is pressed and cursor is not default and not in inventory mode...
	if (Input.GetMouseButtonDown(0) && currentCursor != defaultCursor && !iMode) { 
	
	var ray : Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	var hit : RaycastHit; 
	var didHit : boolean = Physics.Raycast(ray,hit); // did it hit anything?
	var cursor : GameObject = gameObject.Find(currentCursor.name);// find the inventory object from its name
	
		if (didHit) { // you did hit something
		   if (hit.transform.tag == "ActionObject")  return; // do nothing
		   else {
		      AddToInventory(cursor); //add the current cursor
		      ResetCursor();  // reset to default
		      cursor.GetComponent(Interactor).currentState = 1; // update current state
		      audio.Play();
		   }
		}
	
	} // close handle dropped cursors


}

function OnGUI () {

	GUI.skin = customSkin;
	
	//GUI.Label (Rect (Screen.width/2 - 300, Screen.height - 47, 600, 32), "This is a really long and complicated bit of text, so we can test the text on screen size.");	
		
	if (useText && !conversing){  //global toggle
	
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

	if (GUI.Button (Rect(5,Screen.height - 35,32,32), "", boxStyleLabel)) {
	   // call the Toggle Mode function when this button is picked
	   GameObject.Find("Camera Inventory").SendMessage("ToggleMode");
	} 
			
	// cursor control				
	if (!navigating && !suppressPointer && !camMatch) {
		var pos : Vector2 = Input.mousePosition; //get the location of the cursor
		if(!conversing) GUI.color = currentCursorColor; // set the cursor color to current
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


function ResetCursor() {
   currentCursor = defaultCursor; // reset the cursor to the default
}


function AddToInventory (object : GameObject) {

	print ("Adding" + object.name + "  to inventory");
	
	//update the object's element to be the new last element
	object.GetComponent(Interactor).iElement = iLength;
	
	iLength = iLength +1; // increment the length of the current Inventory objects	
	
	object.guiTexture.enabled = false; // *****
	
	//update the currentInventoryObject array
	OrderInventoryArray ();
	
	
	// update the grid
	InventoryGrid(); 
	
	
	if (iLength > 9) {  
		// shift the grid to the right until you get to the end where the new object was added
		while (gridPosition * 3 + 9 < iLength) 
		   GameObject.Find("ArrowLeft").SendMessage("ShiftGrid", "left");
	}



}

function RemoveFromInventory (object : GameObject) {

	//print ("removing" + object.name + "  from inventory");
	   
	// retrieve its inventory element number
	var iRemove = object.GetComponent(Interactor).iElement;
	
	//get the list of the active gameObjects with the InventoryObject tag
	var iObjects = GameObject.FindGameObjectsWithTag("InventoryObject");
	
	// go through the list and decrement the iElement 
	// numbers for all objects past the iRemove number 
	for (var i : int = 0;i < iObjects.length;i++) {
	   //only if its current state is 1 ( in inventory)
	   if(iObjects[i].GetComponent(Interactor).iElement > iRemove) {
	   	  // decrement if the value is greater than the iRemove value
	      iObjects[i].GetComponent(Interactor).iElement --; 
	   }
	}
	
	iLength = iLength -1; // decrement the length of the current Inventory objects	
	
	//update the iRemove object's element to be out of the array
	object.GetComponent(Interactor).iElement = 100;
	
	//update the currentInventoryObject array
	OrderInventoryArray ();
	
	
	//if the third column is empty, shift the grid to the right
	if(gridPosition * 3 + 6 >= iLength  && iLength>= 9) 
	GameObject.Find("ArrowRight").SendMessage("ShiftGrid", "right");

	
	// update the grid
	InventoryGrid();
	   
}

// arrange icons in inventory
function InventoryGrid () {

	var visibility: boolean; // variable for column visibility

	var xPos = -startPos - iconSize/2;  // adjust column offset start position according to icon 
	var spacer = startPos - iconSize; // calculate the spacer size
	var iLength = currentInventoryObjects.length; // length of the array
	
	for (var k = 0; k < iLength; k = k + 3) {
	//calculate the column visibility for the top element, k, using the or, ||
	if (k < gridPosition * 3 || k > gridPosition * 3 + 8) visibility = false;
	else visibility = true; // else it was on the grid
	
	// if elements need to be hidden, do so before positioning
	if (!visibility) HideColumn(k); // send the top row element for processing
	//row 1
	var yPos = startPos - iconSize/2;
	currentInventoryObjects[k].guiTexture.pixelInset = Rect(xPos, yPos, iconSize,iconSize);

		
	//row 2
	yPos = yPos - iconSize - spacer + 3;
	if (k + 1 < iLength) 
	   currentInventoryObjects[k+1].guiTexture.pixelInset = Rect(xPos, yPos, iconSize,iconSize);

		
	//row 3
	yPos = yPos - iconSize - spacer + 6;
	if (k + 2 < iLength) 
	   currentInventoryObjects[k+2].guiTexture.pixelInset = Rect(xPos, yPos, iconSize,iconSize);

	// if elements need to be shown, do so after positioning
	if (visibility) ShowColumn(k); // send the top row element for processing

			
	   xPos = xPos + iconSize + spacer;  // update column position for the next group
	
	} // close for loop
	
	//if there are icons to the left of the grid, activate the right arrow
	if (gridPosition > 0) GameObject.Find("ArrowRight").SendMessage("ArrowState", true);
	else GameObject.Find("ArrowRight").SendMessage("ArrowState", false);
		
	//if there are icons to the right of the grid, activate the left arrow
	if (iLength > gridPosition * 3 + 9) 
	  GameObject.Find("ArrowLeft").SendMessage("ArrowState", true);
	else GameObject.Find("ArrowLeft").SendMessage("ArrowState", false);
	
} // close the function


function ShowColumn ( topElement : int) {

   if (topElement >= iLength) return; // there are no icons in the column, return
   
   // show the elements in the 3 rows for the top element's column
   currentInventoryObjects[topElement].guiTexture.enabled = true; // row 1 element
   
   if (topElement + 1 < iLength) 
         currentInventoryObjects[topElement + 1].guiTexture.enabled = true; // row 2 
   if (topElement + 2 < iLength) 
         currentInventoryObjects[topElement + 2].guiTexture.enabled = true; // row 3 
}

function HideColumn ( topElement : int) {

   if (topElement >= iLength) return; // there are no icons in the column, return

   // hide the elements in the 3 rows for the top element's column
   currentInventoryObjects[topElement].guiTexture.enabled = false; // row 1 element
   if (topElement + 1 < iLength) 
         currentInventoryObjects[topElement + 1].guiTexture.enabled = false; // row 2 
   if (topElement + 2 < iLength) 
         currentInventoryObjects[topElement + 2].guiTexture.enabled = false; // row 3 
}

function OrderInventoryArray () {

	// re-initialize array for new length
	currentInventoryObjects = new GameObject[iLength]; 

	//get a list of the gameObjects with the InventoryObject tag
	var iObjects = GameObject.FindGameObjectsWithTag("InventoryObject");
	
	// load the new array with the state 1 objects, according to their iElement numbers/order
	for (var e : int = 0; e < iLength; e++) { // currentInventoryObjects elements
		for (var i : int = 0; i < iObjects.length; i++) {
			// if there is a match
			if (iObjects[i].GetComponent(Interactor).iElement == e) {
				// add that object to the new array
				currentInventoryObjects[e] = iObjects[i];
				// tell it you're finished looking for that element number
				i = iObjects.length; // reached the end
			}
		}
	    //print (currentInventoryObjects[e]+ " * " + e); // print the new entry
} // loop back to look for next element, e		
				
}

function ToggleConversing (talking : boolean) {

	conversing = talking;
}
