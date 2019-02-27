#pragma strict
var upRange : float =1.0; 
var downRange : float =1.0;
var speed : float = 0.2;

internal var yPos : float; // starting position
internal var upPos : float;
internal var downPos : float;

function Start () {

	yPos = transform.position.y;
}

function Update () {

}

function FixedUpdate () {

	upPos = yPos + upRange; // calculate the target up position
	downPos = yPos - downRange; // calculate the target down position
	// use cosine to get smooth ease in/ease out motion
	var weight = Mathf.Cos((Time.time) * speed * 2 * Mathf.PI) * 0.5 + 0.5;
	// apply the new y position
	transform.position.y = upPos * weight  + downPos * (1-weight);

}
