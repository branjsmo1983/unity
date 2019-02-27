#pragma strict
var life : float = 5.0; // time before destroying the object

function Start () {
	// wait for life seconds before destroying the object the script is on
	yield new WaitForSeconds(life); 
	Destroy(this.gameObject);

}

function Update () {

}