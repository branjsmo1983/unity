#pragma strict

var defaultCursor : Texture; // the regular cursor texture, the arrow
var mouseOverColor : Color = Color.green;  // mouse over color for the cursor
internal var currentCursorColor : Color; // current tint color of cursor
internal var currentCursor : Texture; // the current cursor texture 
internal var navigating : boolean = false;   //flag for navigation state
internal var suppressPointer : boolean = false; // flag to suppress cursor after pick

function Awake () {

   Screen.SetResolution (1280, 800, false);

}

function Start () {

	Screen.showCursor = false; // hide the operating system cursor
	currentCursor = defaultCursor; // assign the default as the current cursor
	currentCursorColor = Color.white; // start color to white
}

function Update () {

   if (Input.GetAxis("Horizontal") || Input. GetAxis ("Vertical") ||
      Input. GetButton ("Turn") || Input.GetButton("ML Enable") ){
     // a navigation key is being pressed 
    navigating = true; // player is moving
    }
   else {
      navigating = false; // player is stationary
   }

}

function OnGUI () {
	
	if (!navigating && !suppressPointer) {
		var pos : Vector2 = Input.mousePosition; //get the location of the cursor
		GUI.color = currentCursorColor; // set the cursor color to current
		GUI.DrawTexture (Rect(pos.x, Screen.height - pos.y,64,64), currentCursor);// draw the cursor there
		GUI.color = Color.white; // set the cursor color back to default
	}
	
}

function CursorColorChange (colorize: boolean) {

   if (colorize)  currentCursorColor = mouseOverColor; 

   else  currentCursorColor = Color.white; 
}
