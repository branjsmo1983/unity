#pragma strict
// make a Unity array for the three possible states of the object, each will have:
// picking cursor, new state, other object, its state, another object, its state, etc...
// use 'default' for the default cursor name  
var lookupState1 : String[];  //declare an array of string type content 
var lookupState2 : String[];  //declare an array of string type content 
var lookupState3 : String[];  //declare an array of string type content 

// arrays of replies for each state and corresponding element
var repliesState1 : String[];
var repliesState2 : String[];
var repliesState3 : String[];
// generic reply in case a match for the cursor was not found
var genericReplies : String[]; // Add one reply for each state


var state : int; // a variable to hold the state to process
internal var currentStateArray : String[]; // var to hold the current array to process
internal var currentReplyArray : String[]; // var to hold the current reply to process

var controlCenter : GameObject; // where the actionObject array lives

var prefabs : GameObject[]; // holder array for prefab objects

function Start () {

	controlCenter = GameObject.Find("Control Center");
	
}


//look up the state of the object to see what needs to happen
function LookUpState (object : GameObject, currentState: int, picker : String) {

	state = currentState; //assign current state to the state variable 
	
	// handle temp cursor texture name
	var matchCursor : String = picker; 

	switch (state) { // use the state to assign the corresponding array

  	   	   
	   case 1:
	   	  // redefine the current arrays for the corresponding number of element
	      currentStateArray = new String [lookupState1.length]; 
	      currentReplyArray = new String [repliesState1.length]; 
	      // assign the array	   	  
	      currentStateArray = lookupState1; 
	      currentReplyArray = repliesState1; 
	   break;
	      
	   case 2:
	   	  // redefine the current arrays for the corresponding number of element
	      currentStateArray = new String [lookupState2.length]; 
	      currentReplyArray = new String [repliesState2.length]; 
	      // assign the array	   
	      currentStateArray = lookupState2;
	      currentReplyArray = repliesState2;  
	   break;   
	   
	   case 3:
	   	  // redefine the current arrays for the corresponding number of element
	      currentStateArray = new String [lookupState3.length]; 
	      currentReplyArray = new String [repliesState3.length]; 
	      // assign the array	   
	      currentStateArray = lookupState3;
	      currentReplyArray = repliesState3;  
	   break;  
	}
   //print ("Results for state " + state ); 

	var element : int = 0; // variable to track the element number for a match 
	var match : boolean = false; // flag to check for cursor match
	
   // go through the array by element
   for (var contents : String in currentStateArray) {

	   	   //split the contents of the current element into a temporary string array 
	   var readString : String[] = contents.Split(",".Chars[0]);

		// check for a cursor match with element 0 of the split
		if (readString[0] == matchCursor) { // if there is a match... 
		
			match = true; // there is a match
			//send the correct text message off to the GameManager
			controlCenter.GetComponent(GameManager).actionMsg = currentReplyArray[element];
	 
		
			//get the new state, element 1 of the split, then convert the string to an int
			var nextState : int = parseInt(readString [1]);
			//transition the object into the new state over in the Interactor script
			if (nextState >= 0) SendMessage("ProcessObject",nextState);//call if the new state # is valid 
	
	
		   //now read through the remainder in pairs
		   //iterate through the array starting at element 2 and incrementing by 2 
		   //as long as the counting variable i is less than the length of the array
		   for (var i = 2; i < readString.length; i= i + 2) {
	
		      //assign the first piece of data in the pair to a temp var for processing
			  var tempS : String = readString[i];
			  
			  //check for special cases here- fill this out later
			if (tempS.Substring(2,1) == "_") { // if there is a special case
			   var s : String = tempS.Substring(0,1); 
			   var s2 : int = parseInt(tempS.Substring(1,1)); // convert the second character to an integer
			   var auxObject : GameObject = CheckForActive(tempS.Substring(3)); // find the object by name & activate
			   //print (auxObject);
			   var bypass : boolean = false; //set a flag if the object shouldn't be transitioned into its new state 
				// look for the matching case
				switch (s) {
				   case "a":  // trigger animation only on the auxiliary object
					// add guts here
					bypass = true; // skip the regular processing
					break;
				   case "b": // change the state on the object only- no animations
					auxObject.GetComponent(Interactor).currentState= parseInt(readString[i+1]);
					bypass = true; // skip the regular processing
					break;
				   case "c": // send a message to "DoCameraMatch" function a script on the auxiliary object
					auxObject.SendMessage("DoCameraMatch");
					bypass = true; // skip the regular processing
					break;
				   case "s": // send a message to the "DoTheJob" function on a script on the auxiliary object 
				   print (auxObject + " "   + s2);
					auxObject.SendMessage("DoTheJob");
					if (s2 == 1) bypass = true; // skip the regular processing
					break;
				   case "p": // instantiate a prefab
					// use the s2 number to specify which element of the prefabs array to instantiate
					Instantiate (prefabs[s2]);
					bypass = true; // skip the regular processing
					break;
				   case "d": // send a message to a script on the object/character to the "SendID" function
				    print (auxObject);
				    auxObject.SendMessage("SendID"); // start the conversation with the character/NPC
				    GetComponent(Interactor).processing = true; // turn on flag to block picks;
				    bypass = true; // skip the regular processing
				    break; 
	    
				} // end switch
			} // end special case
			// if no special cases, find and activate the object using its name  
			else auxObject = CheckForActive(tempS);
			
			// convert the new state from a string value to an integer for use
			var newState : int = parseInt(readString[i+1]);
	  
			// process the axiliary object into the new state
			if(!bypass) {
			 	auxObject.SendMessage( "ProcessObject",newState, SendMessageOptions.DontRequireReceiver);
			 	}
			// reset bypass
			bypass = false;
	
		  } // read the next pair
		  
		} // close the matchCursor block 
		
	element ++; //increment the element counter by 1  
	} // close the for/in block
	
	// if the picked object is not an inventory object, build a no match reply for it
	if (gameObject.tag != "InventoryObject" && !match) HandleNoMatchReplies(picker);
 	else if (matchCursor != "default" && !match){  //swap out the cursor with the object it picked

		//change the picked object's state to 10,special handling
		GetComponent(Interactor).currentState = 10;
		//change the cursor object's state to 11, special handling
		GameObject.Find(picker).GetComponent(Interactor).currentState = 11;
		//store the picked object's current inventory element number on the GameManager
		controlCenter.GetComponent(GameManager).replaceElement = GetComponent(Interactor).iElement;

	   
	   //put the old cursor in inventory
	   GameObject.Find(picker).SendMessage("ProcessObject",1);
	   //pick up the new cursor
	   SendMessage("ProcessObject",2);
	   
	   HandleCursorSwapReplies (picker); // build a reply for the swapped cursor
    }

} // close the LookUpState function


function CheckForActive (name : String) {

   // check to see if the object is active before assigning it to auxObject
   if(gameObject.Find(name)) var auxObject = gameObject.Find(name);
      else { // if no match was found, it must need to be activated
	//load the actionObject array from the GameManager script
	var actionObjects : GameObject[] = controlCenter.GetComponent(GameManager).actionObjects;
	for (var y : int = 0; y < actionObjects.length; y++) { // iterate through the array
		if (actionObjects[y].gameObject.name == name) {  // if there is a match for the name
		      actionObjects[y].gameObject.SetActive(true); // activate the matched object from the array
		      auxObject = gameObject.Find(name); // assign the newly activated object
		} // close the if
	
	} // close the for loop
 
	//load the inventoryObject array from the GameManager script
	var inventoryObjects : GameObject[] = controlCenter.GetComponent(GameManager).inventoryObjects;
	for (var x : int = 0; x < inventoryObjects.length; x++) { // iterate through the array
	//print ("inventory object: " + inventoryObjects[x].gameObject.name + "  " +  name);
		if (inventoryObjects[x].gameObject.name == name) {  // if there is a match for the name
		      inventoryObjects[x].gameObject.SetActive(true); // activate the matched object from the array
		      auxObject = gameObject.Find(name); // assign the newly activated object
		} // close the if
	
	} // close the for loop
     
   } // close the else
   
   return auxObject; // return the gameObject to where the function was called

} 


function HandleNoMatchReplies (picker : String) {

   picker = picker.ToLower(); // make it lower case
   picker = picker.Substring(0,picker.length - 5); // strip off the icon part of the name
   var tempObjectName : String = GetComponent(Interactor).currentObjectName.ToLower();

   var tempMsg : String = "The " + picker + " does not seem to affect the " + tempObjectName ;
   //send the GameManager the action text
   controlCenter.GetComponent(GameManager).actionMsg=tempMsg;
   //tell the GameManager to show the action text
   controlCenter.GetComponent(GameManager).showActionMsg=true;
   //wait two seconds then turn off the action text and leave the function
   yield new WaitForSeconds(2.0);
   controlCenter.GetComponent(GameManager).showActionMsg= false;
   return;
} 


function HandleCursorSwapReplies (picker : String) {

   picker = picker.ToLower(); // make it lower case
   picker = picker.Substring(0,picker.length - 5); // strip off the icon part of the name
   var tempObjectName : String = GetComponent(Interactor).currentObjectName.ToLower();

   var tempMsg = "You exchange the " + picker + " for the " + tempObjectName ; 
   //send the GameManager the action text
   controlCenter.GetComponent(GameManager).actionMsg=tempMsg;
   //tell the GameManager to show the action text
   controlCenter.GetComponent(GameManager).showActionMsg=true;
   //wait two seconds then turn off the action text and leave the function
   yield new WaitForSeconds(2.0);
   controlCenter.GetComponent(GameManager).showActionMsg= false;
   return;
}