#pragma strict
internal var animator : Animator; // var to store the animator component
var h : float; // variable to hold user horizontal input, turns
var v : float; // variable to hold user vertical input, forward/backward
var rotVSpeed  : float  = 90.0; //rotation speed

function Start () {

	animator = GetComponent(Animator); // assign the Animator component
	
	if(animator.layerCount > 1){
		animator.SetLayerWeight(1,1.0); // set layer 1's weight to 1
	}

}

function Update () {

	// Get Input each frame and assign it to the variables
	h = Input.GetAxis("Horizontal");
	v = Input.GetAxis("Vertical");	
	
	if(animator) { // if there is an animator compomonent
	
		// if the fire button was pressed, set the Wave parameter to true
		if(Input.GetButtonDown("Fire1")) animator.SetBool("Wave", true );
		else animator.SetBool("Wave", false );
			
	}

}

function FixedUpdate () {

   // Set V Input and Direction Parameters to H and V axes
   animator.SetFloat ("V Input", v);
   animator.SetFloat("Direction", h); 
   //animator.SetFloat("Direction", Mathf.Abs(h)); // scripted single input turn

   
   // rotate the character according to input and rotation speed
   if (animator.GetFloat("V Input") > 0.1)transform.Rotate(new Vector3 (0,h*Time.deltaTime*rotVSpeed,0)); 

}
