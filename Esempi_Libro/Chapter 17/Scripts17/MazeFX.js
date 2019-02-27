#pragma strict

function OnTriggerEnter () {
   audio.Play(); // start the sound
   GameObject.Find("Point Light FX2").animation.Play();
}

function OnTriggerExit() {
   audio.Stop(); // stop the sound
}

