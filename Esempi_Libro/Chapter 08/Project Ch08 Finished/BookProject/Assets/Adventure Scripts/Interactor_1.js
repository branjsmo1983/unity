#pragma strict

// Pick and Mouseover Info
internal var triggerDistance : float = 7.0; // distance the camera must be to the object before mouse over


// Gain access to these objects
internal var controlCenter : GameObject; // the Control Center object
internal var cam : GameObject; // the main camera on the first person controller

function Start () {

	controlCenter = GameObject.Find("Control Center"); // locate and assign the object to the var
    cam = GameObject.Find("Main Camera"); // find “Main Camera” and assign it to the cam var

}

function OnMouseEnter () {
	//print (DistanceFromCamera());
	if (DistanceFromCamera() > triggerDistance) return;
    controlCenter.SendMessage("CursorColorChange", true); // colorize the pointer
}

function OnMouseExit () {

   controlCenter.SendMessage("CursorColorChange", false); // turn the pointer white

}

function DistanceFromCamera () {

   // get the direction the camera is heading so you only process stuff in the line of sight
   var heading : Vector3 = transform.position - cam.transform.position;
   //calculate the distance from the camera to the object 
   var distance : float = Vector3.Dot(heading, cam.transform.forward);
   return distance;
}
