#pragma strict
internal var mazeManager : MazeManager; // var to hold MazeWalls' MazeManager component
var object : GameObject; // object to be transported
var yPos : float; // the y position destination in the maze
var transportAtStart : boolean = false;

function Start () {

	mazeManager = GameObject.Find("MazeWalls").GetComponent(MazeManager);// find & assign it
	if (transportAtStart == true) BeamMeUpScotty();

}


//function OnMouseDown () {
//   mazeManager.ResetMaze(); // trigger the maze reset
//   yield; // make sure it is in place
//   mazeManager.FindDropPoint(object,object.transform.position.y); // find a drop point
//}

function BeamMeUpScotty () {

   if (yPos == 0) yPos = this.transform.position.y; // use the object's current y if none assigned
   // send this object off to be moved to a random position in the maze
   mazeManager.FindDropPoint(this.gameObject,yPos);
} 
