#pragma strict
private var pickNum : int = 0;

function OnMouseDown () {

	pickNum += 1; // increment the number of times the object was picked 
	print("This object was picked "); // + pickNum + " times.");

}