#pragma strict
var upRange : float =0.0; 
var downRange : float =0.0;
var speed : float = 0.2;

internal var zPos : float; // starting position
internal var upPos : float;
internal var downPos : float;

function Start () {

	zPos = transform.position.z;
}

function Update () {

}

function FixedUpdate () {

	upPos = zPos + upRange; // calculate the target up position
	downPos = zPos - downRange; // calculate the target down position
	// use cosine to get smooth ease in/ease out motion
	var weight = Mathf.Cos((Time.time) * speed * 2 * Mathf.PI) * 0.5 + 0.5;
	// applz the new z position
	transform.position.z = upPos * weight  + downPos * (1-weight);

}
