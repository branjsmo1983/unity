using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look Restricted")]
public class MouseLookRestricted : MonoBehaviour { 


	public enum RotationAxes { MouseX = 0, MouseY = 1  }
	public RotationAxes axes = RotationAxes.MouseX;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationY = 0F;

	void Update ()
	{
		   // only do mouse look if right mouse button is down
  		if (Input. GetButton ("ML Enable") || Input.GetButton("Horizontal") || Input.GetButton("Vertical") || (Input.GetButton("Turn") && axes == RotationAxes.MouseY) ) {


			if (axes == RotationAxes.MouseX)// horizontal mouse for First Person Controller y axis turns
			{
				transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
			}
			else // vertical mouse for main camera x axis rotation
			{
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				//print(rotationY);
				transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
			}
		}
	}
	
	void Start ()
	{
		// Make the rigid body not change rotation
		if (rigidbody)
			rigidbody.freezeRotation = true;
	}
	
	
	public void ResetRotationY()  
	{
		// get the current rotation for the x axis
		//print(transform.rotation.eulerAngles.x);
		float tempY =  transform.rotation.eulerAngles.x ;
		if (tempY <= 360 && tempY >= 300) rotationY = 360 - tempY;
		else rotationY = - tempY;
	}
	
	
}