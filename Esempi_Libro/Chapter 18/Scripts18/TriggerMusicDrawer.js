#pragma strict
var dropObject : GameObject; // the tray
//var nPC : GameObject; // the Gimbok group


//function OnMouseDown () {
//	  if(animation.IsPlaying("music drawer")) return;
//   CloseDrawer();
//}

function DoTheJob() {
  //animation.Play();
  yield new WaitForSeconds(1.75);
   GameObject.Find("Point Light FX2").animation.Play();
   audio.Play();
   GameObject.Find("MazeWalls").SendMessage("ResetMaze");// reset the maze
   //check objects currently in maze
   dropObject.SendMessage("BeamMeUpScotty",SendMessageOptions.DontRequireReceiver);
   //nPC.SendMessage("RecheckPosition",SendMessageOptions.DontRequireReceiver);

}
