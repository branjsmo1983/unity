#pragma strict

internal var targetPos : Transform ; // destination position
internal var source : Transform ; // the position of the object the script is on, start pos
internal var targetLook : Transform; // camera's target for lookAt
internal var fPCamera : GameObject; // main camera
internal var duration : float = 1.0 ; // seconds duration of the camera match
var addTime = 0.0; // additional time after the match before control is returned to the player

function Start () {
fPCamera = GameObject.Find("Main Camera");
}

function Update () {

	if (Input.GetKeyDown("p")){
		// trigger the match
		MatchTransforms ();
	}

}

function MatchTransforms () {
	var ent : float = 0.0;// normalized chunk of time
	var startPos : Vector3 = transform.position; // this object's position
	var startRot : Quaternion = transform.rotation;// this object's rotation
	var startCamRot : Quaternion = fPCamera.transform.rotation;// the main camera's rotation
	
	while (ent <= 1.0) {
		ent = ent + Time.deltaTime/duration; // get the latest time slice 
		// set the new position
		transform.position = Vector3.Lerp(startPos, targetPos.position, Mathf.SmoothStep(0.0,1.0, Mathf.SmoothStep(0.0,1.0,ent))); 
		transform.rotation = Quaternion.Lerp(startRot, targetPos.rotation, Mathf.SmoothStep(0.0,1.0,Mathf.SmoothStep(0.0,1.0,ent)));
		//match camera if targetLook exists
	if (targetLook) {
	   var endCamRot : Quaternion = targetLook.rotation;// get the rotation from the cam target 
	   fPCamera.transform.rotation = Quaternion.Lerp(startCamRot, endCamRot, Mathf.SmoothStep(0.0,1.0,Mathf.SmoothStep(0.0,1.0,ent)));
	} 
								
	yield; // make sure the last line gets processed before looping	to the next time slice
	} // end while
	
	//align the fpc and camera Y rotation again
	this.transform.eulerAngles.y = targetLook.eulerAngles.y;
	fPCamera.transform.localEulerAngles.y = 0.0;
	
	//tell the MouseLookRestricted script the match is finished via the ResetRotationY function
	yield new WaitForSeconds(addTime); // extra pause before returning control to player
	fPCamera.SendMessage("ResetRotationY",SendMessageOptions.DontRequireReceiver); 
	// enable cursor visibility and mouse functionality
	gameObject.Find("Control Center").GetComponent(GameManager).camMatch = false;//enable mouse functions


}
