using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
	[SerializeField]
	internal List<Card> myHand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	internal int CountNumbersOfCards()
	{
		int result = 0;


#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
		return result = (myHand.Capacity != null) ? 0 : myHand.Capacity;
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
	}

}
