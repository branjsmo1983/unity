#pragma strict
// camera match metadata - resides on action objects
var targetPos : Transform; // where the First Person Controller needs to ends up
var targetLook : Transform; // where the main camera needs to end up looking
var matchDelay = 0.0; // any delay time needed before the match
var duration = 1.0 ; //  the time the camera match animates over 
var addTime = 0.0; // additional time after the match before control is returned to the player

internal var controlCenter : GameObject;
internal var fPCamera: GameObject; // first person's camera, Main Camera
internal var fPController: GameObject; // the first person controller

function Start () {

   // gain access to these objects
   controlCenter = GameObject.Find("Control Center2");
   fPCamera = GameObject.Find("Main Camera");
   fPController = GameObject.Find("First Person Controller");

} 


function Update () {

}

function DoCameraMatch () {

// disable cursor visibility and mouse functionality
controlCenter.GetComponent(GameManager_F).camMatch = true;//disable mouse functions

//send off position and look-at values to First Person Controller
fPController.GetComponent(CameraMatch_F).targetPos = targetPos; // the position target
fPController.GetComponent(CameraMatch_F).targetLook = targetLook; // the lookAt target
fPController.GetComponent(CameraMatch_F).duration = duration; // the match time

fPController.GetComponent(CameraMatch_F).addTime = addTime; // extra observation time
   
  
// Wait for the delay if any
yield new WaitForSeconds(matchDelay);

// trigger the camera match
fPController.SendMessage("MatchTransforms"); // start the match

}
