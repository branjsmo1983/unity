#pragma strict

var myDegrees = 100;

function Update () {
   transform.Rotate(0,myDegrees * Time.deltaTime,0);
}

