using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utility 
{
	/// <summary>
	/// Find the the previous card compared to the pivot card
	/// </summary>
	/// <param name="card">Card Pivot used to find the predecessor </param>
	/// <param name="numbers">All card in the hand</param>
	/// <returns>Card with CurrentValue lower than 1</returns>
	public static Card FindPrevious(Card card, List<Card> cards)
	{
		if (cards.Exists(i => (card.CurrentValue == i.CurrentValue + 1) && i.Suit == card.Suit))
		{
			return cards.Find(i => (card.CurrentValue == i.CurrentValue + 1)  && i.Suit == card.Suit);
		}
		else
		{
			return null;
		}
	}

	/// <summary>
	///  Find the the next card compared to the pivot card
	/// </summary>
	/// <param name="card">Card Pivot used to find the next</param>
	/// <param name="cards">All card in the hand</param>
	/// <returns>Card with CurrentValue higher than 1</returns>
	public static Card FindSuccessor(Card card, List<Card> cards)
	{
		if (cards.Exists(i => (card.CurrentValue == i.CurrentValue + 1) && i.Suit == card.Suit))
		{
			return cards.Find(i => (card.CurrentValue == i.CurrentValue + 1) && i.Suit == card.Suit);
		}
		else
		{
			return null;
		}
	}

	/// <summary>
	/// Used for: 
	/// </summary>
	/// <param name="cards"></param>
	/// <returns></returns>
	public static int GetTrisNumber(List<Card> cards)
	{
		int myValues = cards.FirstOrDefault(c => c.Value != Card.MyValues.jolly && c.Value != Card.MyValues.due).CurrentValue;

		if (cards.Count(c => c.CurrentValue == myValues) >= (cards.Count - 1))
		{
			Debug.Log(" è un tris di : " + myValues);
			return myValues;
		}
		else
		{
			Debug.Log(" non è un tris");
			return -1;
		}
	}

	public static bool PinsAreValidOnScale(ref List<Card> myCards)
	{
		Card.MySuits mySuits = myCards.FirstOrDefault(c => !c.CanBeJolly).Suit;
		Debug.Log("" + mySuits);
		int numOfPin = myCards.Count(c => c.Value == Card.MyValues.due);
		if (numOfPin < 2)
		{
			Debug.Log("ho meno di 2 pinelle");
			return true;

		}
		else
		{
			Debug.Log("ho più di 2 pinelle");
			if (!myCards.Exists(c => c.Value == Card.MyValues.due && c.Suit == mySuits))
			{
				Debug.Log(" non ho nessuna pinella con il seme della scala ");
				return false;
			}
			else
			{
				Debug.Log(" ho una pinella con il seme della scala ");
				Card pin = myCards.FirstOrDefault(c => c.Value == Card.MyValues.due && c.Suit == mySuits);
				if ((myCards.Exists(c => c.Value == Card.MyValues.A) && myCards.Exists(c => c.Value == Card.MyValues.tre)) ||
				(myCards.Exists(c => c.Value == Card.MyValues.A) && myCards.Exists(c => c.Value == Card.MyValues.quattro)) ||
				(myCards.Exists(c => c.Value == Card.MyValues.tre) || myCards.Exists(c => c.Value == Card.MyValues.quattro)))
				{
					Debug.Log(" se ho un 3, o un 4, o un asso con un 3 o un 4 ");
					pin.CurrentValue = 2;
					pin.CanBeJolly = false;
					pin.CanBePin = false;
					return true;
				}
				else
				{
					Debug.Log(" non ho ne un 3, ne un 4, ne un asso con un 3 o un 4");
					return false;
				}

			}

		}

	}

	public static void CheckAcePosition(ref List<Card> myCards)
	{
		Card ace = myCards.FirstOrDefault(c => c.Value == Card.MyValues.A);
		//Card.MySuits aceSuits = myCards.FirstOrDefault(c => c.Value == Card.MyValues.A).Suit;
		if ((myCards.Exists(c => c.Value == Card.MyValues.due) && myCards.Exists(c => c.Value == Card.MyValues.tre)) ||
			(myCards.Exists(c => c.Value == Card.MyValues.due) && myCards.Exists(c => c.Value == Card.MyValues.jolly)) ||
			(myCards.Exists(c => c.Value == Card.MyValues.jolly) && myCards.Exists(c => c.Value == Card.MyValues.tre)) ||
			(myCards.Where(c => c.Value == Card.MyValues.due).Count() == 2))
		{
			ace.CurrentValue = 1;
		}


	}

	public static bool IsCanasta(ref List<Card> card)
	{
		bool check = false;


		//check sui giolly e pinelle
		int numberOfJolly = card.Count(c => (c.CanBeJolly) && (!c.CanBePin));
		Debug.Log("numero di jolly : " + numberOfJolly);
		if (numberOfJolly > 1)                                      //se ho + di 1 Jolly NON va bene
		{
			Debug.Log("numero di jolly maggiore di 1");
			return false;
		}

		int numberOfPin = card.Count(c => c.CanBePin);
		Debug.Log("numero di pinelle : " + numberOfPin);
		if (numberOfPin > 2)                                        //se ho + di 2 pinelle NON va bene
		{
			Debug.Log("numero di pinelle maggiore di 2");
			return false;
		}

		if (numberOfJolly > 1 && numberOfPin > 1)                   //se ho + di 1 jolly e 1 pinella
		{
			Debug.Log("più di un jolly e più di una pinella");
			return false;
		}

		//check sui tris
		if (card.Select(c => c.Value).Distinct().Count() == 1)      //tris pulito senza jolly
		{
			Debug.Log("tris pulito");
			return true;
		}

		int myValues = card.FirstOrDefault(c => !c.CanBeJolly).CurrentValue;    //tris con jolly
		Debug.Log(" il valore che ho trovato è : " + myValues);
		if (card.All(c => c.PossibleValues.Contains(myValues)))                 //coi possible values 
		{
			if (card.Where(c => c.CanBeJolly).Count() < 2)
			{
				Debug.Log("tris con jolly");
				return true;
			}
			else
			{
				return false;
			}

		}

		//check sulla scala
		Card.MySuits mySuits = card.FirstOrDefault(c => !c.CanBeJolly).Suit;
		//if (card.Where(c => c.Suit == mySuits).Count() == card.Count - 1)
		//{
		//};
		if ((card.All(c => (c.Suit == mySuits) || c.CanBeJolly)))               //controllo che siano tutte dello stesso seme (a meno che non sia un jolly)
		{
			Debug.Log("sono tutte dello stesso seme (a meno che non ci sia un jolly)");
			//controllo che siano elementi distini (visto che voglio fare una scala

			if (card.Distinct(new CardEqualityComparer()).Count() != card.Count())
			{
				Debug.Log("entro nel ramo in cui gli elementi NON sono distinti");
				return false;
			}

			//IEnumerable<Card> query = card.OrderByDescending(c => c, new Specialcomparer());

			//metto a posto in caso di pinella dello stesso seme
			if (card.Exists(c => c.CanBePin))
			{
				Debug.Log("sono entrato nel metodo in cui controllo la pinella");
				if (!PinsAreValidOnScale(ref card))
				{
					Debug.Log("se il metodo che controlla le pins ritorna falso esco con il return false");
					return false;
				}
			}

			//if (card.Where(c => c.CanBePin).Count() > 1) return false;

			//metto a posto l'asso
			if (card.Exists(c => c.Value == Card.MyValues.A))
			{
				Debug.Log("sono entrato nel metodo in cui controllo l'asso");
				CheckAcePosition(ref card);
			}
			List<Card> temp = card.OrderByDescending(c => c.CurrentValue).ToList();
			card = temp;

			bool jollyAlreadyUsed = false;              // check su quando uso il jolly (se c'è)
			bool jollyFounded = false;                  // controllo se c'è un jolly
			for (int index = 0; index < card.Count() - 1; index++)     // dati 'n' elementi devo fare 'n-1' controlli
			{


				Debug.Log("entro nel ciclo for in cui controllo le cards");
				jollyFounded = (card[index].CanBeJolly);
				Debug.Log(" sono su un jolly? : " + jollyFounded);
				if (jollyFounded)
				{
					Debug.Log("ho trovato un jolly/pinella, eseguo il continue per skippare un index");
					continue;
				}
				// se ho più di una pinella allora una delle 2 deve avere la stessa suite delle altre carte
				if (card[index].CurrentValue - card[index + 1].CurrentValue == 1)
				{
					Debug.Log("la differenza tra " + card[index].name + " e " + card[index + 1].name + " è : " + card[index].CurrentValue + " - " + card[index + 1].CurrentValue + " = " + (card[index].CurrentValue - card[index + 1].CurrentValue));
					check = true;
					continue;
				}
				else if (card[index].CurrentValue - card[index + 1].CurrentValue == 2 && !jollyAlreadyUsed)
				{
					Debug.Log(" sono entrato nel ramo in cui uso il jolly ");
					card.First(c => c.CanBeJolly).CurrentValue = card[index].CurrentValue - 1;
					jollyAlreadyUsed = true;
					check = true;
					continue;
				}
				else
				{
					Debug.Log("la differenza tra il valore corrente e il prossimo è maggiore di 1 oppure ho già usato il jolly");
					Debug.Log("valore corrente = " + card[index].CurrentValue + " valore prossimo = " + card[index + 1].CurrentValue);
					return false;
				}

			}

		}


		return check;
	}


	private class CardEqualityComparer : IEqualityComparer<Card>             //compara solo il valore
	{
		public bool Equals(Card x, Card y)
		{
			//return x.Value == y.Value;
			if ((x.Value != y.Value && x.Suit != y.Suit) || (x.Value == Card.MyValues.jolly && y.Value == Card.MyValues.jolly) || (x.Value == Card.MyValues.due && y.Value == Card.MyValues.due))
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public int GetHashCode(Card obj)
		{
			return obj.CurrentValue;
		}
	}
}
