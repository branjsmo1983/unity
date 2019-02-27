#pragma strict

var mySpeed : float;
var someString : String = "This is a test";
internal var someSetting : boolean = true;
var someObject : GameObject;


function Update () {

   //transform.Rotate(0,50 * Time.deltaTime,0);
   transform.Translate(2 * Time.deltaTime,0,0);

}