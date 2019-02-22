using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Canasta : MonoBehaviour
{

	internal List<Card> cards;

	public bool IsBurraco { get; set; }
	public bool IsBurracoClean { get; set; }
	public bool IsBurracoHalfClean { get; set; }
	public bool IsFull { get; set; }
	public bool IsTris { get; set; }

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

	internal bool IsCanasta()
	{
		bool check = false;

		//TO DO: controllare quando una lista di carte è una canasta

		return check;
	}

	internal static bool IsCanasta(List<Card> card)
	{
		bool check = false;
		
		//check sui giolly e pinelle
		int numberOfJolly = card.Count(c => (c.CanBeJolly) && (!c.CanBePin));
		print("numero di jolly : " + numberOfJolly);
		if (numberOfJolly > 1) return false;
		int numberOfPin = card.Count(c => c.CanBePin);
		print("numero di pinelle : " + numberOfPin);
		if (numberOfPin > 2) return false;
		if (numberOfJolly > 1 && numberOfPin > 1) return false;

		//check sui tris
		if (card.Select(c => c.Value).Distinct().Count() == 1) return true;
		int myValues = card.FirstOrDefault(c => !c.CanBeJolly).CurrentValue;
		print(" il valore che ho trovato è : " + myValues);
		if (card.All(c => c.PossibleValues.Contains(myValues))) return true;

		//check sulla scala
		Card.MySuits mySuits = card.FirstOrDefault(c => !c.CanBeJolly).Suit;
		if((card.All(c => (c.Suit == mySuits) || c.CanBeJolly)))				//controllo che siano tutte dello stesso seme (a meno che non sia un jolly)
		{
			IEnumerable<Card> query = card.OrderByDescending(c => c.CurrentValue);
		}


		return check;
	}


	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	public class Specialcomparer : IComparer<Card>
	{
		public int Compare(Card x, Card y)
		{
			if (x.PossibleValues.Max() > y.PossibleValues.Max())
			{
				return 1;
			}
			else return -1;
		}
	}

}
