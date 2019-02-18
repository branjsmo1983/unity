using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{

	public GameObject slot1;
	internal Burraco burraco;

	// Start is called before the first frame update
	void Start()
    {
		burraco = FindObjectOfType<Burraco>();
		slot1 = this.gameObject;
	}

    // Update is called once per frame
    void Update()
    {
		GetMouseClick();

	}


	void GetMouseClick()
	{
		if(!burraco.isCanGetInput)
		{
			return;
		}

		if (Input.GetMouseButtonDown(0))
		{
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (hit)
			{
				if (hit.collider.CompareTag("myCard"))
				{
					//card in my hand selected 
					Card newCardSelected = hit.collider.GetComponent<Card>();
					MyEventManager.instance.CastEvent(MyIndexEvent.cardSelect, new MyEventArgs(this.gameObject,newCardSelected));
				}
				else if (hit.collider.CompareTag("ourTable"))
				{
					//try to add one or more cards into table
					MyEventManager.instance.CastEvent(MyIndexEvent.cardsHang, new MyEventArgs());
				}
				else if (hit.collider.CompareTag("card"))
				{
					
					//draw a card from deck
					if (burraco.alreadyCollected)
					{
						print(" hai già raccolto");
					}else if (burraco.alreadyFished)
					{
						print(" hai già pescato");
					}
					else
					{
						print("scateno l'evento di pescare una carta dal mazzo");
						hit.collider.tag = "myCard";
						Vector3 lastCardPosition = new Vector3(burraco.me.myHand[burraco.me.myHand.Count - 1].transform.position.x, burraco.me.myHand[burraco.me.myHand.Count - 1].transform.position.y, burraco.me.myHand[burraco.me.myHand.Count - 1].transform.position.z);
						MyEventManager.instance.CastEvent(MyIndexEvent.deckDraw, new MyEventArgs(this.gameObject,lastCardPosition,burraco.me.myHand));
						burraco.alreadyFished = true;
						
					}
					
				}
				else if (hit.collider.CompareTag("refuse")) //scrivere il tag da mettere
				{
					//collect from scraps
					MyEventManager.instance.CastEvent(MyIndexEvent.scrapsCollect, new MyEventArgs());
				}
			}

		}
	}
}
