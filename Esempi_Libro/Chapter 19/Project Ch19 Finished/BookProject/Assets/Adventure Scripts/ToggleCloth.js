#pragma strict
var delay : float = 3; // time to allow cloth to get into draped configuration

function Start () {

   yield new WaitForSeconds(delay); // let cloth fall
   GetComponent(InteractiveCloth).enabled = false; // freeze it
}


function DoTheJob () { // drop cloth to crumpled state

   GetComponent(InteractiveCloth).enabled = true;
   yield new WaitForSeconds(5);
   GetComponent(InteractiveCloth).enabled = false;	
}
