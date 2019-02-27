#pragma strict
var menuSkin : GUISkin; // custom skin for the menus
var simpleText : GUIStyle; // text for nav and info

internal var groupWidth = 750;  // width of the main GUI group
internal var buttnRect = Rect(0,120,130,30); //default button size, x,y location, width and height

//menu management
internal var mainMenu = false; // flag for main menu
internal var confirmDialog = false; // flag for yes/no on quit dialog
internal var creditsScreen = false; // flag for credits dialog
internal var menuMode = false; // no menus are open
internal var end : boolean = false; // flag for end sequence ###
internal var restricted : boolean; // flag for no more menu access ###
// misc menu text
internal var introText = "Welcome to the Mystery of Tizzleblat... a simple world with a solvable problem.";
internal var infoText = "Interactive objects- cursor changes color on \nmouse over, click to activate\nInventory- i key or pick on icon at lower left to \naccess\nGeneral- objects can be combined in scene or \nin inventory";
internal var navText = "Navigation:\nUP/Down, W/S to move forward/Backward\n A/D to strafe left and right\nLeft and Right arrow keys to turn/look around, \nor <shift> or right mouse button \nand move mouse to turn and look around";

//Settings menu variables
internal var walkSpeed : float; // element 0
internal var turnSpeed : float; // element 1
internal var useText : boolean; // element 2
internal var objectDescriptions : boolean; // element 3
var fXVolume : float = 1; // element 4
var ambVolume : float = 1; // element 5
var musicVolume : float = 1; // element 6
var voiceVolume : float = 1; // element 7
internal var colorElement : int; // element 8

internal var playerSettings  = new String[9]; // player settings array in string format

// cursor color
var colorSwatch : Texture;
var cursorColors = new Color[8];
internal var moColor : Color; // mouseover color

//Gain access to 
internal var gameManager : GameManager_F; //###
internal var fPCamera : GameObject;
internal var fPController: GameObject;


internal var iMode = false; // track inventory mode
internal var saving = false; // flag for message for save function


function Start () {

	//controlCenter  = GameObject.Find("Control Center");
	gameManager = GameObject.Find("Control Center2").GetComponent(GameManager_F);//###
	fPCamera= GameObject.Find("Main Camera");
	fPController  = GameObject.Find("First Person Controller");
	
	//initialize GUI 
	walkSpeed = fPController.GetComponent(CharacterMotor).movement.maxForwardSpeed;
	turnSpeed = fPController.GetComponent(FPAdventurerInputController_F).rotationSpeed;
	useText = gameManager.useText; 
	objectDescriptions = gameManager.useLongDesc;
	moColor = gameManager.mouseOverColor; // get color, since there is no element
	
	UpdateSettings(); // update the array that holds the settings	
}

function Update () {

	if (iMode) return; // the inventory screen is open

	// if in end sequence, block all other menu input, but allow player to exit ###	
	if (Input.GetKey ("escape") && restricted) {
	   Application.Quit(); // end now 
	   return;
	}
	
	if(restricted) return; // no menu functionality ###
			
	//toggle the main menu off and on
	if (Input.GetKeyDown("f1") && !menuMode) { 
	    if(mainMenu) {
	        mainMenu= false;
	        MenuMode(false);
	    }
	     else if (!menuMode ) { // if no other menus are open, open it
	        mainMenu= true;
	        MenuMode(true);
	     }
	}

	// brings up the yes/no menu when the player hits the escape key to quit
	if (Input.GetKey ("escape") && !menuMode) {
	   confirmDialog = true; // flag to bring up yes/no menu
	   MenuMode(true);
	}
	
	var pos = Input.mousePosition; //get the location of the cursor
	if (pos.y > Screen.height - 5.0 && pos.x <  5.0 && !menuMode) { 
	   mainMenu = true;
	   MenuMode(true);
	}
	
	if(creditsScreen && Input.anyKeyDown) { 
	   creditsScreen = false;
	   MenuMode(false);
	}


}

function OnGUI () {

	if (iMode) return; // the inventory screen is open
	
	 GUI.depth = 1;
	 GUI.skin = menuSkin; 
	 
	// ********  credits screen  *************
	if(creditsScreen) {
	
	   // Make a group on the center of the screen
	   GUI.BeginGroup (Rect (Screen.width / 2 - 250 , Screen.height  / 2 - 250, 500, 500)); 
	
	   // make a box so you  can see where the group is on-screen
	   GUI.Box (Rect (0,0,500,500), "Credits");
	 
	   // add labels here
	   var creditsY : int = 5;
	   var creditsX : int = 150;

	   GUI.Label( Rect (0,creditsY,500,100), "Developer - Your name here!");
	   
	   creditsY += 40;
	   GUI.Label( Rect (0,creditsY,500,100), "Beginning 3D Game Development with Unity 4.0");
	   
	   
	   creditsY += 40;
	   GUI.Label( Rect (0,creditsY,500,100), "The Adventurer - Zack Sarachman");

	   
	   creditsY += 20;
	   GUI.Label( Rect (0,creditsY,500,100), "Gimbok - Rosendo Rosas");
	   
	   
	   creditsY += 20;
	   GUI.Label( Rect (0,creditsY,500,100), "Kahmi - Evelyn Hernandez");
   
	   creditsY += 20;	   
	   GUI.Label( Rect (0,creditsY,500,100), "Disembodied Voice - Daniel Butler"); 

	   creditsY += 20;
	   GUI.Label( Rect (0,creditsY,500,100), "Voice Recording- Pablo Ortega");	   	   
	   	   	   	   	   	   
	   creditsY += 40;
	   GUI.Label( Rect (0,creditsY,500,100), "The Temple theme - BryanTaylor, Binary Sonata Studios");

	   creditsY += 40;
	   GUI.Label( Rect (0,creditsY,500,100), "The Game, Design - Sue Blackman, Jenny Wang, Gabriel Acosta");	   
	   	   
	   creditsY += 20;
	   GUI.Label( Rect (0,creditsY,500,100), "The Game, Art Assets - Sue Blackman");

	   creditsY += 20;
	   GUI.Label( Rect (0,creditsY,500,100), "The Game, Engine - Unity");	   

	   creditsY += 40;
	   GUI.Label( Rect (0,creditsY,500,100), "The Book, Author - Sue Blackman");
	   
	      	   	   	      	   	   	   
	   creditsY += 20;
	   GUI.Label( Rect (0,creditsY,500,100), "The Book, Publisher - Apress, 2013");

	   creditsY += 40;
	   GUI.Label( Rect (0,creditsY,500,100), "The adventure continues...");

	   // End the credits menu group 
   	   GUI.EndGroup ();
	   
	} // end the credits screen conditional
	
	
	if (end || restricted) return; // block any other menu GUI during end sequence

	
   // *****  main menu  ******
   if(mainMenu) {
      // Make a master group on the center of the screen
      GUI.BeginGroup (Rect (Screen.width / 2 - 375 , Screen.height  / 2 - 270, 750, 500));      

      //*** title and intro
      GUI.Box (Rect (0,0,750,80), "Main Menu");
      GUI.Label( Rect (30,0,650,100), introText);

      //*** navigation and instructions
      GUI.BeginGroup (Rect (0,90,370,340));
      GUI.Box (Rect (0,0,370,340), "General Information and Navigation");
	  GUI.Label( Rect (20,50,350,120), infoText,simpleText);
	  GUI.Label( Rect (20,160,350,350), navText,simpleText);
      if (GUI.Button (Rect (20,280,150,40), "Credits")) {
		mainMenu = false;
		creditsScreen = true;
		MenuMode(true);
	  }
      GUI.EndGroup (); // end navigation & instructions group

   //*** settings
   GUI.BeginGroup (Rect (380,90,370,340));
   GUI.Box (Rect (0,0,370,340), "Settings");
   
	// fpc speeds 
	GUI.Label (Rect (25,35, 100, 30), "Walk Speed");
	walkSpeed = GUI.HorizontalSlider (Rect (150,40, 100, 20), walkSpeed, 0.0, 20.0);
	GUI.Label (Rect (25,60, 100, 30), "Turn Speed");
	turnSpeed = GUI.HorizontalSlider (Rect (150,65, 100, 20), turnSpeed, 0.0, 40.0);           

	// text 
	var textY : int = 90;
	useText = GUI.Toggle (Rect (30,textY, 120, 30), useText, "  Use Text");
	objectDescriptions = GUI.Toggle (Rect (30,textY + 30, 120, 30), objectDescriptions, "  Use Descriptions");      
		      
	// audio 
	var audioY : int = 120;
	audioY += 30;
	GUI.Label (Rect (25, audioY, 100, 30), "FX Volume");
	audioY += 10;
	fXVolume = GUI.HorizontalSlider (Rect (150,audioY, 100, 20), fXVolume, 0.0, 1.0);
	audioY += 14;
	GUI.Label (Rect (25, audioY, 100, 30), "Ambient Volume");
	audioY += 10;
	ambVolume = GUI.HorizontalSlider (Rect (150,audioY, 100, 20), ambVolume, 0.0, 1.0);
	audioY += 14;
	GUI.Label (Rect (25, audioY, 100, 30), "Music Volume");
	audioY += 10;
	musicVolume = GUI.HorizontalSlider (Rect (150,audioY, 100, 20), musicVolume, 0.0, 1.0);
	audioY += 14;
	GUI.Label (Rect (25, audioY, 100, 30), "Dialog Volume");
	audioY += 10;
	voiceVolume = GUI.HorizontalSlider (Rect (150,audioY, 100, 20), voiceVolume, 0.0, 1.0);

	// cursor 
	var cursorY = 260;
	GUI.Label (Rect (30, cursorY, 140, 30), "Current Mouseover Color");
	GUI.contentColor = moColor;
	GUI.Box (Rect (180, cursorY, 20, 20),GUIContent (colorSwatch));
	GUI.contentColor = Color.white;
	
	cursorY += 25;
	GUI.Label (Rect (20, cursorY , 120, 30),  "Mouseover Colors");	
	cursorY += 20;
	var cursorX : int = 160;
	// display color swatches
	for (var i : int = 0;i < 8;i++) {
	   GUI.color = cursorColors[i];
	   if (GUI.Button (Rect (cursorX, cursorY - 15, 20, 40),colorSwatch,simpleText)) {
	      moColor = cursorColors[i];
	      colorElement = i;
	   }
	   cursorX += 25;
	}
	GUI.color = Color.white;
	
	// track changes
	if (GUI.changed) {
	
	   UpdateSettings(); // process the changes
	
	}

		
    GUI.EndGroup (); // end settings group
      
   //*** button options
   GUI.Box (Rect (0,440,750,40), "");
   //this is a local variable that gets changed after each button is added
	var buttnRectTemp : Rect = Rect (20,445,buttnRect.width,buttnRect.height);
	
	if (GUI.Button (buttnRectTemp, "New Game")) {
		Application.LoadLevel("MainLevel"); // Start the Main Level 
	}
	
	buttnRectTemp.x += buttnRect.width + 15; //shift the starting position over for the next one
	if (GUI.Button (buttnRectTemp, "Save Game")) {
	  // save the current game
	  MenuMode(false); // turn off menu mode 
	  SaveGame(false); // save the current game
	}
	
	buttnRectTemp.x += buttnRect.width + 15; //shift the starting position over for the next one
	if (GUI.Button (buttnRectTemp, "Load Game")) {
	   mainMenu = false;
	   LoadGame(); // load the saved game
	   MenuMode(false); // turn off menu mode 
	}
	
	buttnRectTemp.x += buttnRect.width + 15; //shift the starting position over for the next one
	if (GUI.Button (buttnRectTemp, "Quit")) {
	  confirmDialog = true; // turn on confirm menu
	  mainMenu = false; // turn off the menu 
	}
	
	buttnRectTemp.x += buttnRect.width + 15; //shift the starting position over for the next one
	if (GUI.Button (buttnRectTemp, "Resume")) {
	   mainMenu = false; // turn off the menu
	   MenuMode(false); // turn off menu mode
	}

      // End the main group 
      GUI.EndGroup ();

  } // end the main menu if conditional

  
      
	// *******   confirmDialog dialog  *******
	if (confirmDialog) {
	
	   // Make a group on the center of the screen
	   GUI.BeginGroup (Rect (Screen.width / 2 - 100, Screen.height / 2 - 75, 200, 150));
	
	   // make a box so you can see where the group is on-screen.
	   GUI.Box (Rect (0,0,200,150), "Do you really want to quit?");
	
	   // reset the  buttnRectTemp.y value
	   buttnRectTemp = Rect (25,30,150,buttnRect .height);
	
	   if (GUI.Button (buttnRectTemp, "No, resume game")) {
	      // turn off the menu
	      confirmDialog = false;
	      MenuMode(false); // turn off menu mode
	   }
	
	   buttnRectTemp.y += 40;

	   if (GUI.Button (buttnRectTemp, " Yes, quit without saving")) {
	      // quit the game without saving
	      confirmDialog = false;
	      MenuMode(false); // turn off menu mode
	      print ("closing");
	      Application.Quit();
	   }
	
	   buttnRectTemp.y += 40;
	
	   if (GUI.Button (buttnRectTemp, " Yes, but Save first")) {
	      // turn off the menu, save the game, then quit
	      confirmDialog = false;
	      MenuMode(false); // turn off menu mode
	      SaveGame(true); // quit after saving 
	   }
	
	   // End the confirmDialog group 
	   GUI.EndGroup ();
	
	} // end confirm

	
	// saving message
	if (saving) GUI.Label( Rect (20,20,250,100), "Saving game");	 
		
} // end the OnGui function


function UpdateSettings() {  // update  array from GUI

   playerSettings[0] = walkSpeed.ToString();
   playerSettings[1] = turnSpeed.ToString(); 
   playerSettings[2] = useText.ToString(); 
   playerSettings[3] = objectDescriptions.ToString();
   playerSettings[4] = fXVolume.ToString(); 
   playerSettings[5] = ambVolume.ToString(); 
   playerSettings[6] = musicVolume.ToString(); 
   playerSettings[7] = voiceVolume.ToString(); 
   playerSettings[8] = colorElement.ToString();

   // update the settings in the GameManager
   gameManager.NewSettings(playerSettings); 
   
	//Update FX Audio volumes
	var gos = GameObject.FindGameObjectsWithTag ("ActionObject");
	for (var go in gos) {
	//print (go);	
	   if (go.activeSelf) {
	      go.audio.volume = fXVolume;
	      if(go.GetComponent(CharacterID)) go.audio.volume = voiceVolume; // readjust character's volume
	      }
	   else { // wasn't active, so turn it on long enough to adjust sound
	      go.SetActive(true);
	      go.audio.volume = fXVolume;
	      if(go.GetComponent(CharacterID)) go.audio.volume = voiceVolume; // readjust character's volume
	      go.SetActive(false);
	   }
	}
	gos = GameObject.FindGameObjectsWithTag ("Ambient");
	for (go in gos) go.audio.volume = ambVolume;
	gos = GameObject.FindGameObjectsWithTag ("Music");
	for (go in gos) go.audio.volume = musicVolume;
	gos = GameObject.FindGameObjectsWithTag ("Voice");
	for (go in gos) go.audio.volume = voiceVolume;	   
	  

 
}

function UpdateControls () { // update GUI from array 

	walkSpeed = parseFloat(playerSettings[0]);
	turnSpeed = parseFloat(playerSettings[1]);
	useText = parseBool(playerSettings[2]); 
	objectDescriptions = parseBool(playerSettings[3]);
	fXVolume = parseFloat(playerSettings[4]);
	ambVolume = parseFloat(playerSettings[5]); 
	musicVolume = parseFloat(playerSettings[6]); 
	voiceVolume = parseFloat(playerSettings[7]);
	colorElement = parseInt(playerSettings[8]);
	moColor = cursorColors[colorElement]; // update current swatch

}

function SaveGame (quitAfter : boolean) {

   //print ("saving");
   GameObject.Find("SystemIO_F").SendMessage( "WriteFile", "MyNewSavedGame");
   saving = true;
   yield new WaitForSeconds(2);
   saving = false;


   if (quitAfter) {
   	  Application.Quit();
      //print ("closing");
   }

}

function LoadGame () {

   GameObject.Find("SystemIO_F").SendMessage( "ReadFile", "MyNewSavedGame");//###
   
}


function MenuMode (state : boolean) {

   if (end) return; // don't process menu mode
   
   if (state) { // go into menuMode ###
   menuMode = true;
	fPController.GetComponent(CharacterMotor).enabled = false; // turn off navigation
	fPController.GetComponent(FPAdventurerInputController_F).enabled = false; // turn off navigation
	fPController.GetComponent(MouseLookRestricted_F).enabled = false; // turn off navigation
	fPCamera.GetComponent(MouseLookRestricted_F).enabled = false;

   }
   else { // return from menuMode ###
   menuMode = false;
	fPController.GetComponent(CharacterMotor).enabled = true; // turn on navigation
	fPController.GetComponent(FPAdventurerInputController_F).enabled = true; // turn on navigation
	fPController.GetComponent(MouseLookRestricted_F).enabled = true; // turn on navigation
	fPCamera.GetComponent(MouseLookRestricted_F).enabled = true;

   }
}

function parseBool (psValue : String) {
   if (psValue == "True") return true;
   else return false;
}