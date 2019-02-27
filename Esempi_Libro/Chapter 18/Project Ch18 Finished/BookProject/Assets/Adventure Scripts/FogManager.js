#pragma strict
//this script lives on control center
var lightSource : Light; // so it can be turned off & on
var lightIcon : GameObject; // to check inventory for the light source
var hasLight : boolean = false; // flag for player light source
internal var originalState : boolean; // state of fog in render settings
internal var originalColor : Color; // save these to return to
internal var originalDensity : float;

internal var darkColor : Color = Color.black;
internal var darkDensity : float = 0.06; // if player has light source
internal var darkerDensity : float = 0.2; // if player doesn't, make it darker
internal var currentState : boolean = false; // darkness off

function Start () {

	originalState = RenderSettings.fog; // is it off or on?
	originalColor = RenderSettings.fogColor;
	originalDensity = RenderSettings.fogDensity;


}

function InTheDark (state : boolean) { // pass in the fog state you want, true/on, false/off

//check for light source in inventory
if(lightIcon.GetComponent(Interactor).currentState > 0) hasLight = true;
else hasLight = false;

if(state && !currentState){ // it's off & you want it on
   RenderSettings.fog = true; // turn on fog
   RenderSettings.fogColor = darkColor;
   RenderSettings.fogDensity = darkerDensity;
   currentState = true; // dark fog is now on
   if (hasLight) {
      RenderSettings.fogDensity = darkDensity;// adjust density for light
      lightSource.enabled = true;// turn on the light
   }
}
else {
   if(!state && currentState){ // it's on & you want it off
      RenderSettings.fog = originalState;
      RenderSettings.fogColor = originalColor;
      RenderSettings.fogDensity = originalDensity;
      currentState = false; // dark fog is now off
      lightSource.enabled = false;// turn on the light 	
   }
}



}
