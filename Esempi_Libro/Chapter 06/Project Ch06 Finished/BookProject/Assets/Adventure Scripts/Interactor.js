#pragma strict

// Gain access to these objects
var gamePointer : GameObject;
internal var cam : GameObject;


// Pick and Mouseover Info
internal var triggerDistance : float = 7.0; // distance the camera must be to the object before mouse over


function Start () {

   // search the scene for an object named “Main Camera” and assign it to the cam var
   cam = GameObject.Find("Main Camera");

}


function OnMouseEnter () {

	//print (DistanceFromCamera());
	if (DistanceFromCamera() > triggerDistance) return;
	gamePointer.SendMessage("CursorColorChange", true); // colorize the pointer
}

function OnMouseExit () {

	gamePointer.SendMessage("CursorColorChange", false); // turn the pointer white

}

function DistanceFromCamera () {

   // get the direction the camera is heading so you only process stuff in the line of sight
   var heading : Vector3 = transform.position - cam.transform.position;
   //calculate the distance from the camera to the object 
   var distance : float = Vector3.Dot(heading, cam.transform.forward);
   return distance;
}
