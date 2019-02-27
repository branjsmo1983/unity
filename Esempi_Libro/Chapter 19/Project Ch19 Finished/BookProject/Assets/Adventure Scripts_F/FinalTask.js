#pragma strict

var music : GameObject;
internal var menuManager : MenuManager_F;

function Start () {

   menuManager = GameObject.Find("Control Center2").GetComponent(MenuManager_F);

}

function Update () {

  
}

function DoTheJob () {
 
    //turn off cursor and inventory icon
	GameObject.Find("Control Center2").GetComponent(GameManager_F).end = true;
	Screen.lockCursor = true;


	 //trigger the music clip
	 yield new WaitForSeconds(2);
	music.audio.Play();
	
	//roll credits
	yield new WaitForSeconds(1);
 	menuManager.creditsScreen = true; 




}
