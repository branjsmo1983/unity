#pragma strict
var showTerrain = true; // true means show on enter, false means hide on enter

internal var terrain : GameObject;
internal var terrainPosY : float;

function Start () {

   terrain = GameObject.Find("Terrain"); 
   terrainPosY = terrain.transform.position.y ; // get terrain y pos
}


function ManageTerrain () {

   if (showTerrain) {
       terrain.transform.position.y = terrainPosY; // restore the terrain
   }

   else {
       terrain.transform.position.y = terrainPosY - 20; // drop the terrain
   }
}

function OnTriggerEnter () {

   ManageTerrain ();
}
