using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canasta : MonoBehaviour
{

	internal List<Card> cards;

	public bool IsBurraco { get; set; }
	public bool IsBurracoClean { get; set; }
	public bool IsBurracoHalfClean { get; set; }
	public bool IsFull { get; set; }

	internal bool IsAddable(Card card)
	{
		bool result = false;

		// TO DO: logica per vedere se una carta è attaccabile


		return result;
	}

	internal bool Areaddable(List<Card> cards)
	{
		bool result = true;
		foreach(Card card in cards)
		{
			if (!IsAddable(card))
			{
				result = false;
				break;
			}
		}
		return result;
	}


	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
