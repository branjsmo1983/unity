#pragma strict
var aniParent : GameObject;
var aniClip : AnimationClip;
var audioDelay : float = 0.0;

function Start () {

}

function OnMouseDown () {
   print(name + " picked using " + aniClip.name);
   aniParent.animation.Play(aniClip.name);
	//check to make sure an Audio Source component exists before playing
	if (GetComponent(AudioSource)) {
	   //yield new WaitForSeconds(audioDelay);
	   //audio.Play();
	   audio.PlayDelayed (audioDelay); // delay before playing
	}


}

