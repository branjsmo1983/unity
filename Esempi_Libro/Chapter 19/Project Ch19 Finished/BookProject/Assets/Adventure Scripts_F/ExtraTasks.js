#pragma strict
var lightMaterial : Material; // standing stone material for light
var tree : GameObject; // the tree
var audioMessage : GameObject;

function ExtraTasks () {

   // hide trunk topi
   GameObject.Find("Topi Fruit Trunk").SetActive (false);

   // hide blasted trunk
   GameObject.Find("Trunk Blasted").SetActive (false);

   // turn on tree
   tree.SetActive (true);
   tree.GetComponent(Interactor_F).SendMessage("ProcessObject",1);

   //Change the material on the Standing Stones' element 0 material
   GameObject.Find ("Standing Stones").renderer.materials[1] = lightMaterial;

   //Play the voice audio message
   audioMessage.audio.Play();
   gameObject.Find("Control Center2").GetComponent(GameManager_F).actionMsg ="Heralded by a clap of thunder, the Tree of Life is reincarnated" ;//
}
