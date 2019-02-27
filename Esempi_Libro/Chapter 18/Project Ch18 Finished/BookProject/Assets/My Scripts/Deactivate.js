#pragma strict

var object : GameObject;

function OnMouseDown () {

        if (object. activeSelf == false) object.SetActive(true);
        else object.SetActive(false);
}
