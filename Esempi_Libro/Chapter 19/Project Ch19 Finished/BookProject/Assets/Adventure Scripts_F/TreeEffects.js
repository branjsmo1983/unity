#pragma strict

var animationTarget : GameObject; // object with the animations
var topiFruit : GameObject; // 
var fpc : GameObject;
var cameraMain : GameObject;

function Start () {

	// hide trunk topi
	topiFruit.SetActive (false);

}

function DoTheJob  () {

	// show trunk topi
	topiFruit.SetActive (true);
	
	//disable navigation
	Destroy(fpc.GetComponent(MouseLookRestricted_F));
	Destroy(fpc.GetComponent(FPAdventurerInputController_F));
	Destroy(fpc.GetComponent(CharacterMotor));
	Destroy(cameraMain.GetComponent(MouseLookRestricted_F));
	
	//send the restricted message to the InventoryManager and menu manager
	GameObject.Find("Camera Inventory").GetComponent(InventoryManager_F).restricted = true;
	GameObject.Find("Control Center2").GetComponent(MenuManager_F).restricted = true;
	
	// block inventory while sequence runs
	GameObject.Find("Camera Inventory").GetComponent(InventoryManager_F).blocked = true;
	
	yield new WaitForSeconds(3); // pause before starting
	animationTarget.animation.Play();
	
	// unblock inventory 
	yield new WaitForSeconds(8); // allow time for sequence to finish
	GameObject.Find("Camera Inventory").GetComponent(InventoryManager_F).blocked = false;
}
