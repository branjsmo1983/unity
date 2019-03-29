using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
	internal List<Canasta> canaste;
	internal bool existBurraco;


	internal bool IsAddable(Canasta canasta)
	{
		return false;
	}

	internal bool IsAddable(Card card)
	{
		if (canaste.Count == 0)
		{
			return false;
		}

		foreach (Canasta c in canaste)
		{
			if (c.IsAddable(card))
			{
				print("ho trovato una canasta in cui la carta " + card.name + " è aggiungibile");
				return true;
			}
			else
			{
				print("ho trovato una canasta in cui la carta " + card.name + "NON è aggiungibile!!");
				continue;
			}
		}
		print("non ho trovato nessuna canasta a cui attaccare ");
		return false;
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
