#pragma strict
var iD : int; // character ID number for switch 
internal var dialogueManager : GameObject; // so it can contact the  dialogueManager
internal var animator : Animator; // the Animator component for controlling the animation


function Start () {

	animator = GetComponent(Animator);
	dialogueManager = gameObject.Find("Dialogue Manager");
	
	var layers : int = animator.layerCount;

	if (layers >= 2) {
	    for (var i : int = 1; i < layers; i++ ) { 
	          animator.SetLayerWeight(i, 1);
	          print(i);
	    }
	}
}


function Update () {

}

function SendID () {
	
	// check with the DialogueManager to see if it is already in conversation
	var conversing : boolean = dialogueManager.GetComponent(DialogueManager).conversing;

	// if the npc is picked, and not already active in conversation, start the conversation
	if (!conversing) {
		if (GetComponent(SimpleAI)) { // if there is a SimpleAI script...
		   // get the NPC turned around if walking away or, start moving if picked from the time out 
		   if(GetComponent(SimpleAI).progress > 2) { // if walking away or waiting at start
		      GetComponent(SimpleAI).TurnAbout(); // turn back if not facing the player already
		      GetComponent(SimpleAI).progress = 0;// set flag to start walking (already in range)
		      yield new WaitForSeconds(3);// allow the character to return to the player
		   }
		   // inform the SimpleAI script that the character is conversing
		   GetComponent(SimpleAI).progress = -2; 
		   // cancel the timer in case it was running
		   GetComponent(SimpleAI).timerOn = false;  
		   // turn off the walk in case she was picked before stopping
		   GetComponent(Animator).SetBool("Walk", false);
		 } 

	   dialogueManager.SendMessage("SetNPC",iD, SendMessageOptions.DontRequireReceiver);                     dialogueManager.SendMessage("SetGO",this.gameObject,SendMessageOptions.DontRequireReceiver);
	}
			
}

//function OnMouseDown () {
//
//	SendID(); // trigger the conversation
//
//}
