#pragma strict
var labelTexture : Texture;
var startColor : Color;
var endColor : Color;
var duration : float; // fade time
internal var currentColor : Color;
internal var currentTime : float; 




function Start () {

	currentTime = Time.time;
	currentColor = startColor;
	Destroy(gameObject, duration + 0.1);
	

}

function OnGUI () {


	GUI.depth = 0; // make sure it is on top of text

	GUI.color = currentColor;

	GUI.DrawTexture(Rect(0,0,2048,2048), labelTexture, ScaleMode.ScaleToFit, true, 3/2);

}

function FixedUpdate() {

	//animate the color/opacity
    currentColor = Color.Lerp(startColor, endColor, (Time.time - currentTime)/duration);
     
}


