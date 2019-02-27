#pragma strict

internal var controlCenter : GameObject;
internal var defaultCursor : Texture;
internal var currentCursor : Texture;

function Start () {

   controlCenter = GameObject.Find("Control Center2");
   defaultCursor = controlCenter.GetComponent(GameManager_F). defaultCursor;

}


function OnMouseDown () {

	// check the current cursor against the default cursor
	
	currentCursor = controlCenter.GetComponent(GameManager_F). currentCursor;
	
	if (currentCursor == defaultCursor) return; // take no action—it was the default cursor
	
	else { // there is an action icon as cursor, so process it
	
	   // use the cursor texture's name to find the GUI Texture object of the same name   
	   var addObject = GameObject.Find(currentCursor.name);
	
	  // update the icon's current state to in inventory, 1, in the Interactor script
	  addObject.GetComponent(Interactor_F).currentState = 1;
	
	  //after you store the cursor's texture, reset the cursor to default
	   controlCenter.SendMessage("ResetCursor");
	
	  // and add the new object to inventory
	  controlCenter.SendMessage("AddToInventory", addObject); 
	
	}


}



