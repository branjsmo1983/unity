using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilizeMyMathClass : MonoBehaviour
{




    // Start is called before the first frame update
    void Start() {
        MyMathClass myObject;
        myObject = new MyMathClass (10 , 10);
        int sumOfElements = myObject.SumOfElements ();
        Debug.Log (sumOfElements);
        MyMathClass myObject2;
        myObject2 = new MyMathClass (5 , 50);
        sumOfElements = myObject2.SumOfElements ();
        Debug.Log (sumOfElements);
        int[] myArray = new int[25];
        for (int i = 0; i < myArray.Length; i++) {
            myArray[i] = i;
        }
        int max = MyMathClass.GetMax (myArray);
        Debug.Log (max);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
