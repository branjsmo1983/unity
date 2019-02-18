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

	public bool IsVisible { get; set; } = false;
	public bool IsSelected { get; set; } = false;
	public int Cost { get ; private set; }
	public bool CanBeJolly { get; set; }
	public bool CanBePin { get; set; }
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
				break;
			case 1:
				Cost = 15;
				CanBePin = false;
				CanBeJolly = false;
				break;
			case 2:
				Cost = 20;
				CanBePin = true;
				CanBeJolly = true;
				break;
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
				Cost = 5;
				CanBePin = false;
				CanBeJolly = false;
				break;
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
			case 13:
				Cost = 10;
				CanBePin = false;
				CanBeJolly = false;
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
