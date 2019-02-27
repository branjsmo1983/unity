#pragma strict

function Start () {

   // set the GUI Texture  to match the screen size on startup
   guiTexture.pixelInset = Rect (0, 0, Screen.width, Screen.height);

}

function OnMouseDown () {

   yield new WaitForSeconds(0.25); // allow time for mouse down evaluation before toggling mode
   GameObject.Find("Camera Inventory").SendMessage("ToggleMode");

}
