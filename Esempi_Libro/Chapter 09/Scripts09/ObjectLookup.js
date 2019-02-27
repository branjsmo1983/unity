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
internal var currentArray : String[]; // var to hold the current array to process
internal var currentReply : String[]; // var to hold the current reply to process



function Start () {

}

//look up the state of the object to see what needs to happen
function LookUpState (object : GameObject, currentState: int, picker : String) {

	state = currentState; //assign current state to the state variable 
	
	// handle temp cursor texture name
	var matchCursor : String = picker; 

	switch (state) { // use the state to assign the corresponding array
	   
	   case 1:
	      currentArray = lookupState1; 
	      currentReply = repliesState1; 
	   break;
	      
	   case 2:
	      currentArray = lookupState2;
	      currentReply = repliesState2;  
	   break;   
	   
	   case 3:
	      currentArray = lookupState3;
	      currentReply = repliesState3;  
	   break;  
	}
   print ("Results for state " + state ); 
   // go through the array by element
   for (var contents : String in currentArray) {
   //view the contents of the current element in currentArray
   print ("contents: " + contents); 
   //split the contents of the current element into a temporary string array 
   var readString : String[] = contents.Split(",".Chars[0]);
   //} this will get uncommented later
   // now read the first two split out pieces (elements) back out 
   print ("elements in array for state " + state + " = " + readString.length);
   print ("Cursor = " + readString[0]);
   print ("New state = " + readString[1]);

	// check for a cursor match with element 0 of the split
	if (readString[0] == matchCursor) { // if there is a match... 
		//get the new state, element 1 of the split, then convert the string to an int
		var nextState : int = parseInt(readString [1]);
		//transition the object into the new state over in the Interactor script
		SendMessage("ProcessObject",nextState);


	   //now read through the remainder in pairs
	   //iterate through the array starting at element 2 and incrementing by 2 
	   //as long as the counting variable i is less than the length of the array
	   for (var i = 2; i < readString.length; i= i + 2) {
	      print ("auxiliary object = " + readString[i]);
	      print (readString[i]  +  "'s new state = " + readString[i+1]); 
	  }
	} // close the matchCursor block  
} // close the for/in block

} // close the LookUpState function
