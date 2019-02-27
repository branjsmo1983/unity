#pragma strict

function DoTheJob () {

   gameObject.AddComponent (Rigidbody); // adds the rigidbody component
   yield new WaitForSeconds(0.5); // give it time to start falling	
   rigidbody.AddForce(-50, 0, 0); // push it away from the wall in the X direction

   yield new WaitForSeconds(10); // wait for 5 seconds to let it play out and react
   rigidbody.isKinematic = true; // disable physics

   yield;
   Destroy(this); // kill the script to prevent retriggering
}
