#pragma strict
var goldenSleeve : GameObject;
var trayClothFloating : GameObject;

function Start () {

   goldenSleeve = GameObject.Find("Golden Sleeve");
   trayClothFloating = GameObject.Find("TrayCloth Floating");
}


function OnTriggerEnter () {
   audio.Play(); // start the sound
   GameObject.Find("Point Light FX2").animation.Play();
}

function OnTriggerExit() {
   audio.Stop(); // stop the sound
   // if the golden sleeve has been acquired, deactivate the tray
	if (goldenSleeve.GetComponent(Interactor).currentState == 0) {
	   trayClothFloating.SetActive(false);
	}

}

