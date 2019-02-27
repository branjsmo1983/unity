#pragma strict

// Script to scroll main texture and bump based on time
var materialIndex : int = 0; // in case the objects has more than one material

var animateUV = true; // flag for option to scroll texture
var scrollSpeedU1 = 0.0; // variables to scroll texture
var scrollSpeedV1 = 0.0;

var animateBump = false; // flag for option to scroll bump texture
var scrollSpeedU2 = 0.0; // variables to scroll bump texture
var scrollSpeedV2 = 0.0;

function Start () {

   //print ("shininess " + renderer.materials[materialIndex].HasProperty("_Shininess"));
   //print ("parallax " + renderer.materials[materialIndex].HasProperty("_Parallax"));

}


function FixedUpdate () {

   // texture offset variables
   var offsetU1 = Time.time  * -scrollSpeedU1;
   var offsetV1 = Time.time * -scrollSpeedV1;
   // bump texture offset variables
   var offsetU2 = Time.time * -scrollSpeedU2;
   var offsetV2 = Time.time * -scrollSpeedV2;
   if (animateUV) { // if the flag to animate the texture is true...
          renderer.materials[materialIndex].SetTextureOffset ("_MainTex",Vector2(offsetU1,offsetV1));
   }
   
   if (animateBump) { // if the flag to animate the bump texture is true...
      renderer.materials[materialIndex].SetTextureOffset ("_BumpMap", Vector2(offsetU2,offsetV2));
    }


}