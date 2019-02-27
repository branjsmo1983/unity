#pragma strict
internal var animator : Animator; // var to store the animator component
var walk : boolean; // flag to move character
var turn : boolean; // flag to turn character
var h : float; // variable to hold directional input, turns
var v : float; // variable to hold directional input, forward/backward
var rotSpeed : float  = 90.0; //rotation speed
var animSpeed: float  = 1.0;// animation clip speed


function Start () {

	animator = GetComponent(Animator); // assign the Animator component
	animator.speed = animSpeed;// set the animation speed for this character
		
//	var layers : int = animator.layerCount;
//
//	if (layers >= 2) {
//	    for (var i : int = 1; i < layers; i++ ) { 
//	          animator.SetLayerWeight(i, 1);
//	          print(i);
//	    }
//	}

}

function Update () {

}

function FixedUpdate () {
   // Set speed and Direction Parameters using the variables
   if (walk) animator.SetFloat("V Input", v);
   if (turn) {
   		animator.SetFloat("Direction", h);
	   // rotate the character according to input and rotation speed
	   transform.Rotate (new Vector3(0,h*Time.deltaTime*rotSpeed,0));
   }
}
