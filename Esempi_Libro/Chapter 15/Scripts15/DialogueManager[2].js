#pragma strict

// Player conversation topics with npc_1
internal var topics_1 = new String[10]; 
internal var audioOffset: int; // offset for random & sequential replies

// NPC control
internal var rootTopicList = new int[2]; 
internal var branchTopicList = new int[2];
internal var currentTopicsList = new int[20]; // this holds the currently available topics
internal var currentNpc : int = 0; // holds the current npc's id #  


topics_1[0] = "100 null"; 
topics_1[1] = "100 Hi"; 
topics_1[2] = "100 Where am I?"; 
topics_1[3] = "000 How do  I get out of the maze?"; 
topics_1[4] = "100 What happened to me?";
topics_1[5] = "000 Why was I transported here?";
topics_1[6] = "000 Is there a way to acquire the topi fruit?";
topics_1[7] = "000 Can you help me with a replacement?";
topics_1[8] = "100 Gotta go";
topics_1[9] = "100 OK, thanks";


var dialogueSkin : GUISkin;
internal var toggleBool = false; // variable for GUI toggles 

internal var uiText = new String[6]; // store the topic text for the display
internal var uiReply = new int[6]; // store the reply element number for future processing
internal var topics = new String[20]; // store the topic strings here for processing
internal var replies = new String[20]; // store the reply strings here for processing

// replies_1 character reply text
internal var replies_1 = new String[10];

replies_1[0] = "0000 0 0000 You shouldn't see this";
replies_1[1] = "0001 0 0000 ^I've been expecting you^You again?^You still around?";
replies_1[2] = "3030 0 0000  You are in the Maze"; 
replies_1[3] = "0009 0 0000 ^Trial and error?^I cannot help you with that^You can't stay, I can't get out^No idea, I've been stuck here for ages^I wish I knew";
replies_1[4] = "2050 1 0000  You were transported here";
replies_1[5] = "2060 1 0000  You tried to steal the golden Topi fruit";
replies_1[6] = "2070 1 0000  Yes, you must replace it with something of similar size and weight";
replies_1[7] = "0001 1 0000 ^Try using this rock^The rock doesn't work?^Sorry, you're on your own now";
replies_1[8] = "0000 0 0000  Good luck with that";
replies_1[9] = "0000 0 0000  No problem";


internal var fullReply : String; // stores the current full reply string (including codes)
internal var textReply : String; // stores the current reply text part of the fullReply string
internal var currentReplyNum : int; // stores the element number of the current reply 

// timers and dialogue control
internal var playerTalking : boolean = false; // topic list visibility flag, player can talk/choose topic
internal var npcResponding : boolean = false; // character reply visibility flag, npc is talking
internal var pauseForReply : boolean = false; // flag to check pause timer before replying
internal var pauseForReplyLimit : float; // var to hold the target system time for the timer
internal var timerR : boolean = false; // flag to check the timer 
internal var timeLimitR : float; // 
internal var pauseForTopic : boolean = false;// flag to check the timer before showing topics
internal var pauseForTopicLimit : float; // var to hold the target system time for the timer


function Start () {

SetNPC(1); // set the current NPC's ID number by calling the SetNPC function

}

function Update () {

	//timer for the reply
	if(pauseForReply && Time.time > pauseForReplyLimit) {
	   pauseForReply = false; // turn off the timer
	
	   //show the reply
	   playerTalking = false; // turn off player topics
	   npcResponding = true; // turn on reply text
			
	   // start the timer for when to show the topics again
	   pauseForTopicLimit =  Time.time + 2.0; // give player time to read reply
	   pauseForTopic = true;
			
	   //load the next list of topics in preparation for showing them
	   LoadTopic ();
	}

	//timer to start the topics again
	if(pauseForTopic && Time.time > pauseForTopicLimit) {
	   npcResponding = false; // hide the reply
	   playerTalking = true; // show the topics
	   pauseForTopic = false; // turn off the timer
	}

}

function OnGUI () {
   GUI.skin = dialogueSkin;
   
   if (playerTalking) {  // player can select topic
   
		// Make a group on the center of the screen
		GUI.BeginGroup (Rect (0, Screen.height / 2 , 650, 500));
		var y = 35; // this will increment the y position for each topic
		
		//text panel
		GUI.Box(Rect (0, 20 , 400, 230),"");	
	
		//Topic 1 
		if (uiText[1] != "") { 
			if (GUI.Button (Rect (0, y, 500, 35), "")) {
			ProcessReply(uiReply[1]); 
			}
			toggleBool = GUI.Toggle (Rect (25, y, 600, 35), false, uiText[1]);
			y += 35; // increment the next position by 35
		}
	
		//Topic 2 
		if (uiText[2] != "") { 
			if (GUI.Button (Rect (0, y, 500, 35), "")) {
			ProcessReply(uiReply[2]); 
			}
			toggleBool = GUI.Toggle (Rect (25, y, 600, 35), false, uiText[2]);
			y += 35; // increment the next position by 35
		}
		
		//Topic 3 
		if (uiText[3] != "") { 
			if (GUI.Button (Rect (0, y, 500, 35), "")) {
			ProcessReply(uiReply[3]); 
			}
			toggleBool = GUI.Toggle (Rect (25, y, 600, 35), false, uiText[3]);
			y += 35; // increment the next position by 35
		}
	
		//Topic 4 
		if (uiText[4] != "") { 
			if (GUI.Button (Rect (0, y, 500, 35), "")) {
			ProcessReply(uiReply[4]); 
			}
			toggleBool = GUI.Toggle (Rect (25, y, 600, 35), false, uiText[4]);
			y += 35; // increment the next position by 35
		}
		
		//Topic 5 
		if (uiText[5] != "") { 
			if (GUI.Button (Rect (0, y, 500, 35), "")) {
			ProcessReply(uiReply[5]); 
			}
			toggleBool = GUI.Toggle (Rect (25, y, 600, 35), false, uiText[5]);
			y += 35; // increment the next position by 35
		}
		
	
		GUI.EndGroup ();
	}
	
	if (npcResponding) {  // npc is talking
	   GUI.Label (Rect (25, 25, 700, 35), textReply);
	}

	

}


// load topics into GUI
function LoadTopic () { // start at 1 since 0 is null

	topics = new String [topics_1.length]; // re-initialize the array	
	topics = topics_1; // assign the existing array to it for now
	
// generate the next topic list from the current reply  
var j : int = 0; // this will go through the currentTopicsList array
for (var i : int = 1; i < 6; i++){ // only allow 5 topics to be displayed, i is for GUI 
   uiText[i] = ""; // clear the current text
   //check for a valid topic
   if (i <= currentTopicsList.length && j < currentTopicsList.length) {//new
      // if first char is not 0, topic is active, so process
      if (topics[currentTopicsList[j]].Substring(0,1) != "0") {
         //use the string from the 4th character onward as the currentReply
         uiText[i] = topics[currentTopicsList[j]].Substring(4);// new
         // store the element number of the topic for later use
         uiReply[i] = currentTopicsList[j];// new
      }
   else i--; // else it wasn't active, so adjust the topic list number so you don't have a blank spot	
   j++; // increment the topics element number
   }
} // end the for loop



}

// process the reply by its element number
function ProcessReply (replyElement : int) {

	// start timer before showing reply
	pauseForReplyLimit = Time.time  + 0.5; 
	pauseForReply = true; // start the timer

	replies = new String [replies_1.length]; // re-initialize the array	
	replies = replies_1; // assign the existing array to it for now
	currentReplyNum = replyElement; // assign the newly found element number to the var
	fullReply = replies[currentReplyNum]; // get the full reply for that element

	//check to see if any topics need to change their state, first character
	var stateInstructions : String = fullReply.Substring(0,1); // read the first character
	if (stateInstructions != "0") ActivationHandler(stateInstructions);// special handling
	
	ProcessReplyString (); // process the reply string
	
	//get the topics list for this reply
	var useList : String = replies[currentReplyNum].Substring(5,1);
	
	if (useList == "0") {
	   currentTopicsList = new int[rootTopicList.length];
	   currentTopicsList = rootTopicList;
	}
	if (useList == "1") {		
	   currentTopicsList = new int[branchTopicList.length];
	   currentTopicsList = branchTopicList;
	}


}

function ActivationHandler (instructions : String) {

// clear the variables used to change the states
var newState : String = "null"; // the new State for the current topic
var auxNewState : String = "null"; // the new state for the auxillary topic
var auxTopicNum : int ; // auxillary topic number

if (instructions == "1" ) { // deactivate the current topic and activate a new topic
   newState = "0"; // the current topic gets a state change
   auxTopicNum =  parseInt(fullReply.Substring(1,2));
   auxNewState = "1";
}

if (instructions == "2" ) { // don't change the current topic, but activate a new topic
   auxTopicNum =  parseInt(fullReply.Substring(1,2));
   auxNewState = "1";
}

if (instructions == "3" ) { //   deactivate the current topic and deactivate a different topic
   newState = "0";// the current topic gets a state change
   auxTopicNum =  parseInt(fullReply.Substring(1,2));
   auxNewState = "1";
}

if (instructions == "4" ) { // don't change the current topic, but deactivate a different topic
   auxTopicNum =  parseInt(fullReply.Substring(1,2));
   auxNewState = "1";
}

// update the active/inactive topic state - hardcode topics_1 for now 
// update the current topic
if (newState != "null") topics_1[currentReplyNum] =  newState + topics_1[currentReplyNum].Substring(1);
// update the auxiliary topic
if (auxNewState != "null") topics_1[auxTopicNum] =  auxNewState + topics_1[auxTopicNum].Substring(1);




}


function ProcessReplyString () {

// get the type and convert to an int to be able to do some math on it
var type : int = parseInt(fullReply.Substring(3,1));

// normal, single reply, 4th character (element 3) = 0
if (type == 0) {
   textReply = fullReply.Substring(12);
   return;
}


// the rest need to be split up into a temporary array
var readString : String[] = fullReply.Split("^".Chars[0]); 

// if 4th char = 9, choose a random sub string 
if (type == 9) {
   var randNum : int = Random.Range(1, readString.length);
   textReply = readString[randNum];
   audioOffset = randNum;// offset for random & sequential reply audio clips 
   return;
}

// else 4th code charactor is less than 9 and greater than 0 (they've already been processed) 
// the type/number tells you where in the sequence the current reply is at
textReply = readString[type]; // this is the current reply in the sequence
audioOffset = type; // it is also the audio offset

// increment the type number if you haven't reached the last reply yet
if (type < readString.length -1) {
   type +=1; //increment type
   //check to see if the replies need to reset/loop, "reset"
   // as the last reply will be ignored and cause the type to reset to 1
   if (readString[type] == "reset") type = 1; 
}

// increment or reset the 4th code char (element 3) for the next time, update the original string with the correct number
replies_1[currentReplyNum] = replies_1[currentReplyNum].Substring(0,3) + type.ToString() + replies_1[currentReplyNum].Substring(4);


}

// set the conversation npc character as current
// this info is sent when the player clicks on the npc via ObjectLookup, special cases
function SetNPC (npcID : int) {

   // need to hard code these in	
   switch (npcID) {
      case 1 :
      var rootTopics : String = "1,2,3,4,8";	
      var branchTopics : String = "5,6,7,9";
      rootTopicList = ConvertToArray (rootTopics); // convert the string to an array
      branchTopicList = ConvertToArray (branchTopics); // convert the string to an array
      currentTopicsList = rootTopicList; 
      break;
			
   } // end switch
   
	currentNpc = npcID; // assign the npc that was passed into the function
	playerTalking = true; // the player is now able to "talk" to the npc - topics will display
	npcResponding = false; // reply hidden
	LoadTopic(); // load the topics that can be discussed with the current npc into the GUI
   
   
   
} // end SetNPC

function ConvertToArray (theString : String) {

   var tempTopics : String[] = theString.Split(",".Chars[0]); 
   var tempTopicsList  = new int[tempTopics.length];
   for (var i : int = 0; i < tempTopics.length; i++) {
      tempTopicsList[i] = parseInt(tempTopics[i]);
   }
   return tempTopicsList; // return the new array
}
