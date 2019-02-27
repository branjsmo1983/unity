#pragma strict

var myDegrees = 50;

function Update () {

   transform.Rotate(0,myDegrees * Time.deltaTime,0);

}

