#pragma strict

// Player conversation topics with npc_1
internal var topics_1 = new String[6]; 
internal var audioOffset: int; // offset for random & sequential replies

topics_1[0] = "100 null"; 
topics_1[1] = "100 Hi"; 
topics_1[2] = "100 Where am I?"; 
topics_1[3] = "000 How do I get out of the maze?"; 
topics_1[4] = "100 What happened to me?";
topics_1[5] = "100 Gotta go";

var dialogueSkin : GUISkin;
internal var toggleBool = false; // variable for GUI toggles 

internal var uiText = new String[6]; // store the topic text for the display
internal var uiReply = new int[6]; // store the reply element number for future processing
internal var topics = new String[20]; // store the topic strings here for processing
internal var replies = new String[20]; // store the reply strings here for processing

// replies_1 character reply text
internal var replies_1 = new String[6];

replies_1[0] = "0000 You shouldn't see this";
replies_1[1] = "0001^I've been expecting you^You again?^You still around?";
replies_1[2] = "3030 You are in the Maze";
replies_1[3] = "0009^Trial and error?^I cannot help you with that^You can't stay, I can't get out^No idea, I've been stuck here for ages^I wish I knew";
replies_1[4] = "0000 You were transported here";
replies_1[5] = "0000 Good luck";

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

	LoadTopic ();
	playerTalking = true; // topic list visible
	npcResponding = false; // reply hidden

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
	   GUI.Label (Rect (25, 25, 500, 35), textReply);
	}

	

}


// load topics into GUI
function LoadTopic () { // start at 1 since 0 is null

	topics = new String [topics_1.length]; // re-initialize the array	
	topics = topics_1; // assign the existing array to it for now
	
	// generate the next topic list from the current reply  
	var j : int = 1; // initialize the topics array to start at the first real reply
	for (var i : int = 1; i < 6; i++){ // only allow 5 topics to be displayed
	   uiText[i] = ""; // clear the current text
	   if (j < topics.length) { // check to see if there are more topics 
	      // if first char is not zero/naught, topic is active, so process the string
	      if (topics[j].Substring(0,1) != "0") {
	         //use the string from the 4th character onwards
	         uiText[i] = topics[j].Substring(4);
	         uiReply[i] = j; // save the topic number to get its reply later on
	         }
	         else i--;  // else it wasn't active, so adjust the topic list number so you don't have a blank spot 
	         j++; // increment the topics element number
	      }
	}// end the for loop


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
   textReply = fullReply.Substring(5);
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


