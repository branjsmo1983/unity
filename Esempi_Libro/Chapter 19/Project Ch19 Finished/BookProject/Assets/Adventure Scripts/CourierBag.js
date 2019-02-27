#pragma strict
var fileName : String;

function Awake () {
   DontDestroyOnLoad(gameObject); // so it will persist to the next level 
}

function Start () {

   fileName = "MyNewSavedGame";
   Destroy(gameObject, 1); //allow the info 1 second to be harvested before killing courier
}
