#pragma strict
var target : Transform; // the target object
internal var tempTarget : Vector3; // var to hold the target's position adjusted for height

// Rotate the camera every frame so it keeps looking at the target,
// but rotates only on its Y, or up axis
function Update() { 

   tempTarget.x = target.position.x;// use target's x and z
   tempTarget.z = target.position.z;	
   tempTarget.y = transform.position.y;// use this object's y value
	   
   //face the target
   transform.LookAt(tempTarget);	
}
