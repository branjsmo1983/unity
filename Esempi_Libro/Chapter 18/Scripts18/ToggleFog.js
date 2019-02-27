#pragma strict
var darkState : boolean; // which state to invoke
internal var controlCenter : GameObject; // keeper of fog state

var fpc : GameObject; // first person controller
var location : Transform; // transport location
var blackout : GameObject; // blackout crossfade prefab
var lightIcon : GameObject; // the light source icon
var newLevel : boolean; // flag to send player to the next level


function Start () {
  controlCenter = GameObject.Find("Control Center");
}

function OnTriggerEnter() {

   controlCenter.SendMessage("InTheDark", darkState);
   
	//if player doesn't have a light source, blackout and relocate
	if(lightIcon.GetComponent(Interactor).currentState == 0) { //check inventory for light source
	   yield new WaitForSeconds(5); // give him a few seconds to look around
	   Instantiate(blackout); // start the blackout
	   yield new WaitForSeconds(1); // make sure its dark before changing anything
	   controlCenter.SendMessage("InTheDark", false); // update the fog state
	   fpc.transform.position = location.position; // relocate the player	
	   
		// activate the topic about light in the tunnels since he has now been there
		var dm : DialogueManager = gameObject.Find("Dialogue Manager").GetComponent(DialogueManager);
		dm.topics_2[7] = "107" +  dm.topics_2[7].Substring(4);
	}

	else { 
	   //turn off light & send player to the final level
	   if(newLevel) { // flag set in the Inspector
	      Instantiate(blackout);
	      yield new WaitForSeconds(1);
	      Application.LoadLevel ("FinalLevel"); // load the new level
	      controlCenter.SendMessage("InTheDark", false); // temporary for testing
	   }
	}



}
