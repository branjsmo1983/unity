using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{

	public GameObject slot1;
	private Burraco burraco;

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
					MyEventManager.instance.CastEvent(MyIndexEvent.cardSelect, new MyEventArgs());
				}
				else if (hit.collider.CompareTag("ourTable"))
				{
					//try to add one or more cards into table
					MyEventManager.instance.CastEvent(MyIndexEvent.cardsHang, new MyEventArgs());
				}
				else if (hit.collider.CompareTag("card"))
				{
					//draw a card from deck
					MyEventManager.instance.CastEvent(MyIndexEvent.deckDraw, new MyEventArgs());
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
