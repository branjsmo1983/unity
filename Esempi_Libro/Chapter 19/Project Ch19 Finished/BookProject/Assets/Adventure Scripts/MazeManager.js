#pragma strict
internal var dropPoint : GameObject; // this gets built and found
var dropObject : GameObject; // object to move to the drop point


function Start () {

	yield;// give the walls time to be processed
	ResetMaze ();
	FindDropPoint (dropObject,dropObject.transform.position.y); // find a random location for the drop point

}


function ResetMaze () {

   // trigger the wall rotations on all of the children
   gameObject.BroadcastMessage("Scramble");
   yield new WaitForSeconds(0.05); // give maze time to reset
}


function FindDropPoint (obj : GameObject, yPos : float) {

	dropObject = obj; // assign the new drop object
	
   //randomly generate the name of one of the 30 drop points 
   var num : int = Random.Range(1,30); 
	// parseInt changes the integer to a string
	if (num > 9) var name : String = "DropPoint" + parseInt(num); 
	else  name = "DropPoint0" + parseInt(num);

    //print (name);
	dropPoint = GameObject.Find(name); // local var to hold selected drop point
	CheckDropPoint(dropPoint, 12.0); // pass the object in to have its distance checked	
	
	//move the drop object to the dropPoint's location
	dropObject.transform.position.x = dropPoint.transform.position.x;
	dropObject.transform.position.y = yPos;// in case it is a different y location
	dropObject.transform.position.z = dropPoint.transform.position.z;

}

function CheckDropPoint (obj : GameObject, trapLimit : float) { 
 
	// get distances to surrounding walls clockwise from 12 o'clock
	var dForward : float = DistToWall(obj.transform .position, Vector3.forward , Color.blue);
	var dRight : float = DistToWall(obj.transform .position, Vector3.right , Color.yellow);
	var dBackward : float = DistToWall(obj.transform .position,-Vector3.forward , Color.red);
	var dLeft : float = DistToWall(obj.transform .position,-Vector3.right , Color.green);
	
	var total : float = dForward + dRight +  dBackward +  dLeft;
	//print (dForward + "  " + dRight +  "  " +  dBackward +  "  " + dLeft + "  " + total);
	
	// check for single square trap
	if (total < trapLimit) {
	   print ("trapped");
	   ResetMaze (); // get a new maze configuration
	   CheckDropPoint(dropPoint, trapLimit); // and check the drop point again
	}

}

function DistToWall (origin: Vector3, direction : Vector3, lineColor : Color) {
    
    // pass the source/origin and direction in to be checked
    // do raycasting
   var hit : RaycastHit; // create a variable to hold the collider that was hit
   if (Physics.Raycast(origin, direction, hit)) {
      Debug.DrawLine (origin, hit.point, lineColor,1.0); // draw a line to check the position
      return hit.distance; // send the distance back to the caller
   }
   else return 1000.0; // didn't hit anything, so assign a large number 
}


