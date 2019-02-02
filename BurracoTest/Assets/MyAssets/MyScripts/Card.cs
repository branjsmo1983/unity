using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

	[SerializeField]
	private Sprite cardFace, cardBack;

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	[SerializeField]
	private MySuits suit;

	[SerializeField]
	private MyColors colors;

	[SerializeField]
	private MyValues value;

	private bool canBeJolly, canBePin;

	public int Cost { get ; private set; }
	public bool CanBeJolly { get => canBeJolly; set => canBeJolly = value; }
	public bool CanBePin { get => canBePin; set => canBePin = value; }

	private void CalculateProp()
	{
		switch ((int)value)
		{
			case 0:
				Cost = 30;
				canBePin = false;
				canBeJolly = true;
				break;
			case 1:
				Cost = 15;
				canBePin = false;
				canBeJolly = false;
				break;
			case 2:
				Cost = 20;
				canBePin = true;
				canBeJolly = true;
				break;
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
				Cost = 5;
				canBePin = false;
				canBeJolly = false;
				break;
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
			case 13:
				Cost = 10;
				canBePin = false;
				canBeJolly = false;
				break;
			default:
				Cost = -1;
				break;
		}
	}

	// Start is called before the first frame update
	void Start()
    {
		CalculateProp();

	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public enum MySuits
	{
		clubs = 0,
		diamonds = 1,
		hearts = 2,
		spades = 3,
		none = 4,
	}

	public enum MyColors
	{
		red = 0,
		blue = 1,
	}

	public enum MyValues
	{
		jolly = 0,
		A = 1,
		due = 2,
		tre = 3,
		quattro = 4,
		cinque = 5,
		sei = 6,
		sette = 7,
		otto = 8,
		nove = 9,
		dieci = 10,
		J = 11,
		Q = 12,
		K = 13,
	}
}
