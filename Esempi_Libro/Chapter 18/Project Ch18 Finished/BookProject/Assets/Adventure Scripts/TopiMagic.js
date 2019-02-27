#pragma strict
var crossfade : GameObject; // the crossfade prefab
var mazeCharacter : GameObject; // want character not parent
var fpc : GameObject; // the first person controller
var fpcWaypoint : GameObject; // the placeholder 
internal var maze : GameObject; // object with MazeManager script, MazeWalls
var mazeSound : AudioClip;
var terrain : GameObject; // to be able to reset the terrain
internal var terrainPosY : float; // original y position


function Start () {

   terrain = GameObject.Find("Terrain"); 
   terrainPosY = terrain.transform.position.y ; // get terrain y pos
   
   

}

function DoTheJob () {

	// activate and update character's state
	mazeCharacter.SetActive(true);
	mazeCharacter.GetComponent(Interactor).currentState = 1;
	
	// reset maze
	var mazeManager : MazeManager = GameObject.Find("MazeWalls").GetComponent(MazeManager);
	mazeManager.ResetMaze(); // scramble the maze
	// make sure the way out from Gimbok character is open, 
	// 100 value is trapLimit to clear more than 4 wall trap
	mazeManager.CheckDropPoint(mazeCharacter,100.00); 
	
	// raise terrain
	terrain.transform.position.y = terrainPosY; 
	
	// cue the twirl effect
	GameObject.Find("Camera Weapon").camera.animation.Play();
	
	AudioSource.PlayClipAtPoint(mazeSound,fpc.transform.position); // play at character's position
	
	Instantiate(crossfade);
	yield new WaitForSeconds(1.5);
	
	// move player
	fpc.transform.position = fpcWaypoint.transform.position;
	fpc.transform.rotation.eulerAngles.y = 134.28; // hard code to face npc


}