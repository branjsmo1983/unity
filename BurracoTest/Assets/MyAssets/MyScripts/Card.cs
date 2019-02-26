using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

	[SerializeField]
	internal Sprite cardFace, cardBack;

	[SerializeField]
	internal SpriteRenderer spriteRenderer;

	[SerializeField]
	private MySuits suit;

	[SerializeField]
	private MyColors color;

	[SerializeField]
	private MyValues value;

	internal BoxCollider2D cardCollider2D;

	private string _name;

	public string Name
	{
		get
		{
			_name = value + " " + suit + " " + color;
			return _name;
		}

	}
	public bool IsVisible { get; set; } = false;
	public bool IsSelected { get; set; } = false;
	public int Cost { get ; private set; }
	public bool CanBeJolly { get; set; }
	public bool CanBePin { get; set; }
	public int CurrentValue { get; set; }
	public List<int> PossibleValues = new List<int>();

	public MySuits Suit
	{
		get
		{
			return suit;
		}
	}
	public MyColors Color
	{
		get
		{
			return color;
		}
	}
	public MyValues Value
	{
		get
		{
			return value;
		}
	}


	private void CalculateProp()
	{
		switch ((int)value)
		{
			case 0:
				Cost = 30;
				CanBePin = false;
				CanBeJolly = true;
				CurrentValue = 16;
				for(int i = 1; i < 15; i++)
				{
					PossibleValues.Add(i);
				}
				break;
			case 1:
				Cost = 15;
				CanBePin = false;
				CanBeJolly = false;
				CurrentValue = 14;
				PossibleValues.Add(1);
				PossibleValues.Add(14);
				break;
			case 2:
				Cost = 20;
				CanBePin = true;
				CanBeJolly = true;
				CurrentValue = 15;
				for (int i = 1; i < 15; i++)
				{
					PossibleValues.Add(i);
				}
				break;
			case 3:
				CurrentValue = 3;
				PossibleValues.Add(3);
				break;
			case 4:
				CurrentValue = 4;
				PossibleValues.Add(4);
				break;
			case 5:
				CurrentValue = 5;
				PossibleValues.Add(5);
				break;
			case 6:
				CurrentValue = 6;
				PossibleValues.Add(6);
				break;
			case 7:
				Cost = 5;
				CanBePin = false;
				CanBeJolly = false;
				CurrentValue = 7;
				PossibleValues.Add(7);
				break;
			case 8:
				CurrentValue = 8;
				PossibleValues.Add(8);
				break;
			case 9:
				CurrentValue = 9;
				PossibleValues.Add(9);
				break;
			case 10:
				CurrentValue = 10;
				PossibleValues.Add(10);
				break;
			case 11:
				CurrentValue = 11;
				PossibleValues.Add(11);
				break;
			case 12:
				CurrentValue = 12;
				PossibleValues.Add(12);
				break;
			case 13:
				Cost = 10;
				CanBePin = false;
				CanBeJolly = false;
				CurrentValue = 13;
				PossibleValues.Add(13);
				break;
			default:
				Cost = -1;
				break;
		}
	}

	void Awake()
	{
		CalculateProp();
	}

	// Start is called before the first frame update
	void Start()
    {
		spriteRenderer = GetComponent<SpriteRenderer>();

	}

    // Update is called once per frame
    void Update()
    {

		spriteRenderer.sprite = IsVisible ? cardFace : cardBack ;
		spriteRenderer.color = IsSelected ? UnityEngine.Color.yellow : UnityEngine.Color.white;
		if (!IsVisible && gameObject.tag != "Respawn" && gameObject.tag != "prestart")
		{
			gameObject.transform.localScale = new Vector3(0.24f, 0.24f, 1);

		}
		else if (IsVisible && gameObject.tag != "Respawn" && gameObject.tag != "prestart")
		{
			gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 1);

		}
	}

	public enum MySuits
	{
		clubs = 0,
		diamonds = 1,
		hearts = 2,
		spades = 3,
		none = 4,				//per i Jolly
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
