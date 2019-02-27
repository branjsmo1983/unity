#pragma strict
import System.IO;
internal var filePath : String;
internal var filename= "SavedGame";
internal var extension = ".txt";

function Start () {
   filePath = Application.dataPath + "/";
   //print (filePath + filename+ extension);
}


function Update () {

}

function WriteFile(filename : String) {

   var sWrite: StreamWriter = new StreamWriter(filePath + filename + extension);

// level
var level = Application.loadedLevel; // the current level number
sWrite.WriteLine(level);

//First Person Controller transforms
var fpc = GameObject.Find("First Person Controller");
sWrite.WriteLine(fpc.transform.position);
sWrite.WriteLine(fpc.transform.localEulerAngles);

//Player Settings
var ps = GameObject.Find("Control Center2").GetComponent(MenuManager_F).playerSettings;
sWrite.WriteLine(ps[0]); //walkSpeed
sWrite.WriteLine(ps[1]); //turnSpeed
sWrite.WriteLine(ps[2]); // useText
sWrite.WriteLine(ps[3]); //objectDescriptions
sWrite.WriteLine(ps[4]); //fXVolume
sWrite.WriteLine(ps[5]); //ambVolume  
sWrite.WriteLine(ps[6]); //musicVolume 
sWrite.WriteLine(ps[7]); // voiceVolume 
sWrite.WriteLine(ps[8]); // mo color

//Action Objects- get the list generated by the GameManager_F on Awake
var ao = GameObject.Find("Control Center2").GetComponent(GameManager_F).actionObjects;

for (var x : int = 0; x < ao.length; x++) { // iterate through the array of action objects
   // save its current state 
   //print (ao[x]); // so you can see what objects are being processed
   if (ao[x].activeSelf == true) {
       sWrite.WriteLine(ao[x].GetComponent(Interactor_F).currentState);
		if (ao[x].GetComponent(Interactor_F).saveTransforms == true){
		    sWrite.WriteLine(ao[x].transform.position);
		    sWrite.WriteLine(ao[x].transform.localEulerAngles);
		} 
   }
   else { // if inactive, wake it up long enough to save its current state
      ao[x].SetActive(true); // activate it
      sWrite.WriteLine(ao[x].GetComponent(Interactor_F).currentState);
		if (ao[x].GetComponent(Interactor_F).saveTransforms == true){
		    sWrite.WriteLine(ao[x].transform.position);
		    sWrite.WriteLine(ao[x].transform.localEulerAngles);
		}      
      ao[x].SetActive(false); // deactivate it
   }
}

//Inventory Objects- get the list generated by the GameManager_F on Awake
var io = GameObject.Find("Control Center2").GetComponent(GameManager_F).inventoryObjects;

for (x = 0; x < io.length; x++) { // iterate through the array of action objects
   // save its current state 
   //print (io[x]); // so you can see what objects are being processed
   if (io[x].activeSelf == true) {
       sWrite.WriteLine(io[x].GetComponent(Interactor_F).currentState);
   }
   else { // if inactive, wake it up long enough to save its current state
      io[x].SetActive(true); // activate it
      sWrite.WriteLine(io[x].GetComponent(Interactor_F).currentState);
      io[x].SetActive(false); // deactivate it
   }
}

// stop writing/saving data here if player is in FinalLevel
if (Application.loadedLevelName == "FinalLevel") {
   sWrite.Flush();
   sWrite.Close();
   return; // don't save any more data
}

//save dialogue array contents
var dm : DialogueManager = GameObject.Find("Dialogue Manager").GetComponent(DialogueManager);
var tempArray = new String[dm.topics_1.length]; 
tempArray = dm.topics_1;
for(var e : String in tempArray) sWrite.WriteLine(e);
tempArray = new String[dm.topics_2.length]; 
tempArray = dm.topics_2;
for(e in tempArray) sWrite.WriteLine(e);
tempArray = new String[dm.replies_1.length]; 
tempArray = dm.replies_1;
for(e in tempArray) sWrite.WriteLine(e);
tempArray = new String[dm.replies_2.length]; 
tempArray = dm. replies_2;
for(e in tempArray) sWrite.WriteLine(e);

// maze walls find them, save their local z rotation
var walls : Component[] = GameObject.Find("MazeWalls").GetComponentsInChildren(Transform);
for (var wall : Component in walls) 
   if (wall.gameObject.GetComponent(MeshRenderer)) sWrite.WriteLine(wall. gameObject.transform.localEulerAngles.z);

// Misc single values
sWrite.WriteLine(GameObject.Find("Terrain").transform.position.y);
sWrite.WriteLine(GameObject.Find("TempleBlocker").collider.enabled);
sWrite.WriteLine(GameObject.Find("Control Center2").GetComponent(FogManager).currentState);

   sWrite.Flush();
   sWrite.Close();
}



function ReadFile(fileName : String) {
   var sRead = new File.OpenText(filePath + fileName + extension);

// level, if the level is different than the present level, load it
var level = parseInt( sRead.ReadLine());
if (level != Application.loadedLevel) {
   // more here later
   Application.LoadLevel(level);
}

// First Person Controller transforms
var fpc = GameObject.Find("First Person Controller");
ProcessTransforms(fpc,sRead.ReadLine(),"position");
ProcessTransforms(fpc,sRead.ReadLine(),"rotation");
   
// read Settings into an array 
var ps = new String[9];
for (var i : int; i<9; i++) {
   ps[i] = sRead.ReadLine();
}
// send the new settings off to menu and game managers
var controlCenter : GameObject = GameObject.Find("Control Center2");
controlCenter.GetComponent(GameManager_F).NewSettings(ps); // update array in game manager
controlCenter.GetComponent(MenuManager_F).playerSettings = ps; // update the playerSettins array
controlCenter.GetComponent(MenuManager_F).UpdateControls(); // update GUI controls
      
//Process action objects- get the list generated by the GameManager_F on Awake
var ao = GameObject.Find("Control Center2").GetComponent(GameManager_F).actionObjects;
for (var x : int = 0; x < ao.length; x++) { // iterate through the array of action objects
   // process it into the save's state  
   ao[x].SetActive(true); // activate it
   ao[x].GetComponent(Interactor_F).loading = true; // turn on the loading flag
   ao[x].SendMessage("ProcessObject",parseInt(sRead.ReadLine()));
      if (ao[x].GetComponent(Interactor_F).saveTransforms == true){
	  ProcessTransforms(ao[x],sRead.ReadLine(),"position");
	  ProcessTransforms(ao[x],sRead.ReadLine(),"rotation");
   }
}
         
//Process inventory objects- get the list generated by the GameManager_F on Awake
var io = GameObject.Find("Control Center2").GetComponent(GameManager_F).inventoryObjects;
for (x = 0; x < io.length; x++) { // iterate through the array of inventory objects
   // process it into the save's state  
   io[x].SetActive(true); // activate it
   io[x].SendMessage("ProcessObject",parseInt(sRead.ReadLine()));
}
            
// stop reading/loading data here if player is in FinalLevel
if (Application.loadedLevelName == "FinalLevel") {
   sRead.Close();
   return; // don't read any more data
}
               
//load dialogue array contents
var dm : DialogueManager = GameObject.Find("Dialogue Manager").GetComponent(DialogueManager);
for(i = 0; i < dm.topics_1.length; i++) dm.topics_1[i] = sRead.ReadLine();
for(i = 0; i < dm.topics_2.length; i++) dm.topics_2[i] = sRead.ReadLine();
for(i = 0; i < dm.replies_1.length; i++) dm.replies_1[i] = sRead.ReadLine();
for(i = 0; i < dm.replies_2.length; i++) dm.replies_2[i] = sRead.ReadLine();
                  
// maze walls find them, update their local z rotation
var walls : Component[] = GameObject.Find("MazeWalls").GetComponentsInChildren(Transform);
for (var wall : Component in walls) 
   if (wall.gameObject.GetComponent(MeshRenderer)) wall. gameObject.transform.localEulerAngles.z = parseFloat(sRead.ReadLine());	

// Misc single values
GameObject.Find("Terrain").transform.position.y = parseFloat(sRead.ReadLine());
GameObject.Find("TempleBlocker").collider.enabled = parseBool(sRead.ReadLine());
GameObject.Find("Control Center2").GetComponent(FogManager).InTheDark(parseBool(sRead.ReadLine()));                     
   sRead.Close();
}


function ProcessTransforms (object :GameObject, theValue : String, transform : String) {
   //strip off parentheses
   print(object + "  " + theValue);
   theValue = theValue.Substring(1,theValue.length -2);
   //split the string into an array using the commas
   var readString : String[] = theValue.Split(","[0]);
   // feed the new elements into a Vector3
   var nt : Vector3 = Vector3(parseFloat(readString[0]),parseFloat(readString[1]),parseFloat(readString[2])); 
   if (transform == "position") object.transform.position = nt;
   else object.transform.localEulerAngles = nt;
}


function parseBool (psValue : String) {
   if (psValue == "True") return true;
   else return false;
}