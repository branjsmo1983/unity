#pragma strict
var twoStates : boolean = false; // default for single state objects
var aniObject : GameObject;
var aniClipA : AnimationClip;
var aniClipB : AnimationClip;
var audioClipA : AudioClip;
var audioClipB : AudioClip;
var audioDelayA : float = 0.0;
var audioDelayB : float = 0.0;
var aniLoopClip : AnimationClip; // if not null, this will be called after the first clip is finished

internal var aniClip : AnimationClip;
internal var fXClip : AudioClip; 
internal var audioDelay : float;
internal var objState : boolean = true;  // true is the beginning state, false is the second state 


function Start () {
	// if no parent was assigned, assume it is the object this script is on
	if (aniObject == null) aniObject = this.gameObject; 
}


function OnMouseDown () {

	if (twoStates == false) objState = true; // if twoStates is false, set objectState to true

	if (objState) {    // if objState is true/ use A
	   aniClip = aniClipA;  // set the new animation clip
	   fXClip = audioClipA; // set the new audio clip
	   audioDelay = audioDelayA; // set the new delay
	   objState = false;  // change its state to false
	}
	 else  {  // the objState must be false / use B
	   aniClip = aniClipB;  // set the new animation clip
	   fXClip = audioClipB;  // set the new audio clip
	   audioDelay = audioDelayB; // set the new delay
	   objState = true;  // change its state to true
	}

   print(name + " picked using " + aniClip.name);
   aniObject.animation.Play(aniClip.name);
	//check to make sure an Audio Source component exists before playing
	if (GetComponent(AudioSource)) {
		audio.clip = fXClip; // change the audio component's assigned sound file 
		//audio.PlayDelayed (audioDelay); // delay before playing it	 
	}
	
	if (aniLoopClip) {
		// wait the length of the first animation before you play the second
		yield new WaitForSeconds (aniClipA.length); 
		aniObject.animation.Play(aniLoopClip.name); // this one needs to be set to loop
	}


}

