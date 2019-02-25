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
		if (numberOfJolly > 1)
		{
			print("numero di jolly maggiore di 1");
			return false;
		}

		int numberOfPin = card.Count(c => c.CanBePin);
		print("numero di pinelle : " + numberOfPin);
		if (numberOfPin > 2)
		{
			print("numero di pinelle maggiore di 2");
			return false;
		}

		if (numberOfJolly > 1 && numberOfPin > 1)
		{
			print("più di un jolly e più di una pinella");
			return false;
		}

		//check sui tris
		if (card.Select(c => c.Value).Distinct().Count() == 1)		//tris pulito senza jolly
		{
			return true;
		}

		int myValues = card.FirstOrDefault(c => !c.CanBeJolly).CurrentValue;	//tris con jolly
		print(" il valore che ho trovato è : " + myValues);
		if (card.All(c => c.PossibleValues.Contains(myValues)))
		{
			return true;       //il controllo lo faccio su PossibleValues e non sul current così becco anche i jolly o pinelle
		}

		//check sulla scala
		Card.MySuits mySuits = card.FirstOrDefault(c => !c.CanBeJolly).Suit;
		if((card.All(c => (c.Suit == mySuits) || c.CanBeJolly)))				//controllo che siano tutte dello stesso seme (a meno che non sia un jolly)
		{
			print("sono tutte dello stesso seme (a meno che non ci sia un jolly)");
			//IEnumerable<Card> query = card.OrderByDescending(c => c, new Specialcomparer());
			IEnumerable<Card> query = card.OrderByDescending(c => c.CurrentValue);
			card = query.ToList<Card>();
			bool jollyAlreadyUsed = false;              // check su quando uso il jolly (se c'è)
			bool pinAlreadyUsed = false;				// check su quando uso la pin (se c'è)
			bool jollyFounded = false;					// controllo se c'è un jolly
			bool pinFounded = false;					// controllo se c'è una pinella
			for(int index = 0; index < query.Count() -1; index++)		// dati 'n' elementi devo fare 'n-1' controlli
			{
				print("entro nel ciclo for in cui controllo le cards");
				jollyFounded = (card[index].CanBeJolly)&&(!card[index].CanBePin);
				pinFounded = card[index].CanBePin;
			}
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
			print("Sto comparando :" + x.Name);
			print("");
			if (x.PossibleValues.Max() > y.PossibleValues.Max())
			{
				return 1;
			}
			else return -1;
		}
	}

}
