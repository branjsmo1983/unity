#pragma strict

var sendToObject  : GameObject; 
var correctObject: GameObject; //this is the only object we allow to trigger the event 

function OnTriggerEnter (object : Collider) {

	// call the function on the specified object
    if(object == correctObject.collider) sendToObject.SendMessage("ToggleTrigger "); 
	print (object.name); // this is the name of the object triggered the event
}

function OnTriggerExit (object : Collider) {

	// call the function on the specified object
    if(object == correctObject.collider) sendToObject.SendMessage("ToggleTrigger "); 
	print (object.name); // this is the name of the object triggered the event
}