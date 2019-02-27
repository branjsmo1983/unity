#pragma strict
internal var rotAngles = new int[7];
rotAngles[0] = 0;
rotAngles[1] = 90;
rotAngles[2] = 90;
rotAngles[3] = 180;
rotAngles[4] = 180;
rotAngles[5] = 270;
rotAngles[6] = 270;

//function Start () {
//   Scramble();
//} 


function Scramble () {
   // get a random element number in the array's range
   var element = Random.Range(0, 6); //remember arrays start at element 0
   //rotate the object on its local Z the number of degrees represented by that element
   transform.localEulerAngles.z = rotAngles[element];
}
