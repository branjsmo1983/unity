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
				if (hit.collider.CompareTag("myCard"))											//sto selezionando una carta ddella mia mano
				{
					//card in my hand selected 
					Card newCardSelected = hit.collider.GetComponent<Card>();
					MyEventManager.instance.CastEvent(MyIndexEvent.cardSelect, new MyEventArgs(this.gameObject,newCardSelected));
				}
				else if (hit.collider.CompareTag("ourTable"))									//sto cercando di calare carta/canasta sul tavolo
				{
					//try to add one or more cards into table
					MyEventManager.instance.CastEvent(MyIndexEvent.cardsHang, new MyEventArgs());
				}
				else if (hit.collider.CompareTag("ourCanasta"))
				{
					Card cardOnTable = hit.collider.GetComponent<Card>();
					print("ho trovato la carta : " + cardOnTable.name);
					print("numero di canaste sul tavolo = " + burraco.ourTable.canaste.Count);
					int numOfCan = 0;
					foreach(Canasta c in burraco.ourTable.canaste)
					{
						numOfCan++;
						print(" La canasta numero " + numOfCan + " ha " + c.cards.Count + " carte");
					}
					Canasta selected = burraco.ourTable.canaste.Find(canasta => canasta.cards.Find(c => c == cardOnTable));
					//Canasta selected = new Canasta();
					//foreach(Canasta c in burraco.ourTable.canaste)
					//{
					//	if (c.cards.Contains(cardOnTable))
					//	{
					//		print("ho trovato la canasta");
					//		selected = c;
					//		foreach(Card card in selected.cards)
					//		{
					//			print(" " + card.name);
					//		}
					//	}
					//	else
					//	{
					//		print("non ho trovato la canasta");
					//	}
					//}
					if(selected.cards != null)
					{
						print(" ho trovato la canasta con carte : ");
						foreach (Card card in selected.cards)
						{
							print(" " + card.name);
						}
					}
					else
					{
						print("non ho trovato nessuna canasta!");
					}
					
					MyEventManager.instance.CastEvent(MyIndexEvent.cardsAddToCanasta, new MyEventArgs());
				}
				else if (hit.collider.CompareTag("card"))										//sto cercando di pescare
				{
					
					//draw a card from deck
					if (burraco.me.HasCollected)
					{
						print(" hai già raccolto");
					}else if (burraco.me.HasFished)
					{
						print(" hai già pescato");
					}
					else
					{
						print("scateno l'evento di pescare una carta dal mazzo");
						Card cardFished = hit.collider.GetComponent<Card>();
						cardFished.tag = "myCard";
						print("la carta è : " + cardFished.name);
						Vector3 lastCardPosition = new Vector3(burraco.me.myHand[burraco.me.myHand.Count - 1].transform.position.x, burraco.me.myHand[burraco.me.myHand.Count - 1].transform.position.y, burraco.me.myHand[burraco.me.myHand.Count - 1].transform.position.z);
						MyEventManager.instance.CastEvent(MyIndexEvent.deckDraw, new MyEventArgs(this.gameObject,lastCardPosition,burraco.me.myHand,cardFished));
						burraco.me.HasFished = true;
						
					}
					
				}
				else if (hit.collider.CompareTag("refuse"))											//sto cercando di raccogliere
				{
					print("scateno l'evento di pescare una carta dal mazzo");
					MyEventManager.instance.CastEvent(MyIndexEvent.scrapsCollect, new MyEventArgs());
				}
			}

		}
	}
}
