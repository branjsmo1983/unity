#pragma strict

function Start () {

}

function Update () {
   // gets the current cursor position as a Vector2 type variable
   var pos = Input.mousePosition;

   // feed its x and y positions back into the GUI Texture object’s parameters 
   guiTexture.pixelInset.x = pos.x; 
   guiTexture.pixelInset.y = pos.y - 32; // offset to top
}
