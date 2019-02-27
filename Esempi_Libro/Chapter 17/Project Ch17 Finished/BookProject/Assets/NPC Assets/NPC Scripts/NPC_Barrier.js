#pragma strict
var target : GameObject; // character to block
internal var simpleAI : SimpleAI; // script with the character AI

function Start () {
   simpleAI = target.GetComponent(SimpleAI);
}


function OnTriggerEnter () {
   simpleAI.progress = 2; // set progress to waiting
   simpleAI.animator.SetBool("Walk", false); // stop the walk
   simpleAI.StartTimer(5.0); //start the timer in the SimpleAI script to send the character back
}
