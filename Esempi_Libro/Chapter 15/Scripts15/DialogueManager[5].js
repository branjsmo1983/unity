#pragma strict

// Player conversation topics with npc_1
var topics_1 = new String[10]; 
internal var audioOffset: int; // offset for random & sequential replies

//audio variables
var clipSource : DialogueAudioClips; // the script with the audio clip arrays

internal var currentTopicAudio = new AudioClip[2]; // array for current topics audio
internal var currentReplyAudio = new AudioClip[2]; // array for current replies audio
internal var uiAudio : int; // element number for current topic audio
internal var currentAudioReply : AudioClip; // holds the reply audio until the topic audio is finished
internal var audioElement : int; // audio clip element number for the current reply


// NPC control
internal var rootTopicList = new int[2]; 
internal var branchTopicList = new int[2];
internal var currentTopicsList = new int[20]; // this holds the currently available topics
internal var currentNpc : int = 0; // holds the current npc's id #  
internal var exitConv : boolean = false; // flag to end conversation

// Player conversation topics with npc_1
topics_1[0] = "100 null"; 
topics_1[1] = "101 Hi"; 
topics_1[2] = "102 Where am I?"; 
topics_1[3] = "003 How do  I get out of the maze?"; 
topics_1[4] = "104 What happened to me?";
topics_1[5] = "005 Why was I transported here?";
topics_1[6] = "006 Is there a way to acquire the topi fruit?";
topics_1[7] = "007 Can you help me?";
topics_1[8] = "108 Gotta go";
topics_1[9] = "109 OK, thanks";



var dialogueSkin : GUISkin;
internal var toggleBool = false; // variable for GUI toggles 

internal var uiText = new String[6]; // store the topic text for the display
internal var uiReply = new int[6]; // store the reply element number for future processing
internal var topics = new String[20]; // store the topic strings here for processing
internal var replies = new String[20]; // store the reply strings here for processing

// replies_1 character reply text
internal var replies_1 = new String[10];
replies_1[0] = "0000 0 0000 You shouldn't see this";
replies_1[1] = "0001 0 0001^I've been expecting you^You again?^You still around?^reset";
replies_1[2] = "3030 0 0004 You are in the Maze"; 
replies_1[3] = "0009 0 0005^Trial and error?^I cannot help you with that^You can't stay, I can't get out^No idea, I've been stuck here for ages^I wish I knew";
replies_1[4] = "2050 1 0010 You were transported here";
replies_1[5] = "2060 1 0011 You tried to steal the golden Topi fruit";
replies_1[6] = "2070 1 0012 Yes, you must replace it with something of similar size and weight";
replies_1[7] = "0001 1 0113^Try using this rock^The rock doesn't work?^Sorry, you're on your own now";
replies_1[8] = "9000 0 0016 Good luck with that";
replies_1[9] = "0000 0 0017 No problem";

// Player conversation topics  with npc 2
internal var topics_2 = new String[11]; 
topics_2[0] = "000 null"; 
topics_2[1] = "101 Hi"; 
topics_2[2] = "102 What happened to the other guy?";
topics_2[3] = "103 I got the topi fruit, what am I supposed to do with it?";
topics_2[4] = "004 Where do I find the Tree of Life?";
topics_2[5] = "005 How do I find the tunnels?";
topics_2[6] = "006 Can you help me?";
topics_2[7] = "I keep blacking out in the tunnels, could you help me out with a light source?";
topics_2[8] = "108 The maze looks different, how do I get out of it?";
topics_2[9] = "109 See you around";
topics_2[10] = "110 Thanks for your help";

// replies_2 character reply text
internal var replies_2 = new String[11];
replies_2[0] = "0000 0 0000 You shouldn't see this"; 
replies_2[1] = "0000 0 0001 Greetings, adventurer";
replies_2[2] = "0000 0 0002 Oh, he's probably still stuck in the maze";
replies_2[3] = "2040 1 0003 You must replace the dying Tree of Life by planting the new fruit";
replies_2[4] = "2050 1 0004 You will find a way to it in the tunnels";
replies_2[5] = "2060 1 0005 The tunnels are in the rock dome, there used to be a map of them in the temple";
replies_2[6] = "0001 1 0206^I have marked a trail to the top for you and cleared the entrance^There's nothing more I can do for you";
replies_2[7] = "0000 0 0008 The flower of a local, but rare plant is said to be quite illuminating";
replies_2[8] = "0009 0 0009^Trial and error^I cannot help you with that^It is ever changing, there is no map^No idea, I've been stuck here for ages";
replies_2[9] = "9009 0 0013^I wish you success^Come back any time^Good luck^May your quest bear fruit";
replies_2[10] = "0000 0 0017 Any time";

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
clipSource  = gameObject.Find("DialogueAudioClips").GetComponent(DialogueAudioClips);
}

function Update () {

	//timer for the reply
	if(pauseForReply && Time.time > pauseForReplyLimit) {
	   pauseForReply = false; // turn off the timer
	
	   //show the reply
	   playerTalking = false; // turn off player topics
	   npcResponding = true; // turn on reply text
	   
	   // reply audio
		audio.clip = currentAudioReply; // load the processed reply
		audio.Play(); // play the reply audio

			
	   // start the timer for when to show the topics again
	   pauseForTopicLimit =  Time.time + currentAudioReply.length + 0.5; // give player time to read reply
	   pauseForTopic = true;
			
	   //load the next list of topics in preparation for showing them
	   LoadTopic ();
	}

	//timer to start the topics again
	if(pauseForTopic && Time.time > pauseForTopicLimit) {
	   npcResponding = false; // hide the reply
	   if (!exitConv) playerTalking = true;// show the topics if the conversation has not finished
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
	   GUI.Label (Rect (25, 25, 800, 35), textReply);
	}

	

}


// load topics into GUI
function LoadTopic () { // start at 1 since 0 is null

	switch (currentNpc) {
		
	   case 1 :
	      topics = new String [topics_1.length];// re-initialize array	
	      topics = topics_1; // load it
	      break;
			
	   case 2 :
	      topics = new String [topics_2.length]; // re-initialize array		
	      topics = topics_2; // load it
	      break;		
	
	} // end switch
	
		
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

	// locate and load the topic audio clip for the topic/reply element 
	var tempAudioElement : int = parseInt(topics[replyElement].Substring(1,2));
	audio.clip = currentTopicAudio[tempAudioElement];
	audio.Play(); // play the topic audio

	// start timer before showing reply
	pauseForReplyLimit = Time.time + audio.clip.length + 0.5; 
	pauseForReply = true; // start the timer

	switch (currentNpc) {
		
	   case 1 :
	      replies = new String [replies_1.length];
	      replies = replies_1;
	      break;
			
	   case 2 :
	      replies = new String [replies_2.length];
	      replies = replies_2;
	      break;		
	} // end switch

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

// leaving the conversation 
if (instructions == "9" ) {
   exitConv = true; // set the flag to suppress GUI
   //currentGO.SendMessage("Conversation", false, SendMessageOptions.DontRequireReceiver);
   return;
}


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

//update the topic states as needed 

	switch (currentNpc) {
	   case 1 :
	      if (newState != "null") topics_1[currentReplyNum] =  newState + topics_1[currentReplyNum].Substring(1);
	      if (auxNewState != "null") topics_1[auxTopicNum] =  auxNewState + topics_1[auxTopicNum].Substring(1);
	      break;
	 		
	   case 2 :
	      if (newState != "null") topics_2[currentReplyNum] =  newState + topics_2[currentReplyNum].Substring(1);
	      if (auxNewState != "null") topics_2[auxTopicNum] =  auxNewState + topics_2[auxTopicNum].Substring(1);
	      break;		
	
	} // end switch

}


function ProcessReplyString () {

	// locate the reply audio element 
	audioElement = parseInt(fullReply.Substring(9,2));

			
	//Process special instructions, the 7th and 8th char
	SpecialInstructions(fullReply.Substring(7,2));
	
	
	// get the type and convert to an int to be able to do some math on it
	var type : int = parseInt(fullReply.Substring(3,1));
	
	// normal, single reply, 4th character (element 3) = 0
	if (type == 0) {
	   textReply = fullReply.Substring(12);
	   currentAudioReply = currentReplyAudio[audioElement];
	   return;
	}
	
	
	// the rest need to be split up into a temporary array
	var readString : String[] = fullReply.Split("^".Chars[0]); 
	
	// if 4th char = 9, choose a random sub string 
	if (type == 9) {
	   var randNum : int = Random.Range(1, readString.length);
	   textReply = readString[randNum];
	   audioOffset = randNum;// offset for random & sequential reply audio clips 
	   currentAudioReply = currentReplyAudio[audioElement + audioOffset -1];
	   return;
	}
	
	// else 4th code charactor is less than 9 and greater than 0 (they've already been processed) 
	// the type/number tells you where in the sequence the current reply is at
	textReply = readString[type]; // this is the current reply in the sequence
	audioOffset = type; // it is also the audio offset
	currentAudioReply = currentReplyAudio[audioElement + audioOffset -1];
	
	// increment the type number if you haven't reached the last reply yet
	if (type < readString.length -1) {
	   type +=1; //increment type
	   //check to see if the replies need to reset/loop, "reset"
	   // as the last reply will be ignored and cause the type to reset to 1
	   if (readString[type] == "reset") type = 1; 
	}
	
	// increment or reset the 5th char (element 4)for the next time, 
	// update [rebuild] original string with the correct number
	switch (currentNpc) {
			
	   case 1 :
	      replies_1[currentReplyNum] = replies_1[currentReplyNum].Substring(0,3) + type.ToString() + replies_1[currentReplyNum].Substring(4);
	      break;
				
	   case 2 :
	      replies_2[currentReplyNum] = replies_2[currentReplyNum].Substring(0,3) + type.ToString() + replies_2[currentReplyNum].Substring(4);
	      break;		
	} // end switch


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
		currentTopicAudio = new AudioClip[clipSource.audioTopics1.length];
		currentTopicAudio = clipSource.audioTopics1;
		currentReplyAudio = new AudioClip[clipSource.audioReplies1.length];
		currentReplyAudio = clipSource.audioReplies1;
		break;

	case 2 :
		rootTopics = "1,2,3,7,9";	
		branchTopics  = "3,4,5,6,10";
		rootTopicList = ConvertToArray (rootTopics); 
		branchTopicList = ConvertToArray (branchTopics); 
		currentTopicsList = rootTopicList; 
		currentTopicAudio = new AudioClip[clipSource.audioTopics2.length];
		currentTopicAudio = clipSource.audioTopics2;
		currentReplyAudio = new AudioClip[clipSource.audioReplies2.length];
		currentReplyAudio = clipSource.audioReplies2;
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


//this lets you do extra stuff, trigger animations, special effects, just about anything
function SpecialInstructions (code : String) {
  if(code == "00") return;
	
  switch (code) {
    case "01" : // offers rock- this is a one off
       print ("npc offers player a rock");
       //clear the special instructions flag by rebuilding the string
       replies_1[7] =  replies_1[7].Substring(0,7) + "00" + replies_1[7].Substring(9);
       break;

	case "02" : // activates path
	   print ("activates path markers");
	   //clear the special instructions flag by rebuilding the string
	   replies_2[6] =  replies_2[6].Substring(0,7) + "00" + replies_2[6].Substring(9);
	   break;

			
   }// end switch
} // end SpecialInstructions

