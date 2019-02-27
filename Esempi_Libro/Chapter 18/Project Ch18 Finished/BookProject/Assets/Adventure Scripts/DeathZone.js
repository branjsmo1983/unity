#pragma strict
var spawnPoint: Transform[]; // array of possible positions
var blackout : GameObject;


function Start () {

}

function Update () {

}


function OnTriggerEnter () {

   Instantiate(blackout); // trigger a fade in/out
   var element : int = Random.Range(0, spawnPoint.length); // choose a position from the array
   yield new WaitForSeconds(1.5);
   gameObject.Find("First Person Controller").transform.position = spawnPoint[element].position;
}
