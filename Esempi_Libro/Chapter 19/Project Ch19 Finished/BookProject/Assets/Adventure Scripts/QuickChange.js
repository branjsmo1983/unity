#pragma strict


function Start () {

   yield;
   if (GetComponent(Interactor).currentState == 2)renderer.materials[1].color = Color.black;

}
