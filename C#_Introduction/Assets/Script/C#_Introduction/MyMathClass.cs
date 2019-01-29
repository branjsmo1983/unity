using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMathClass
{
    private int secretNumber = 15;
    protected int onlyForChildren = 45;
    public int[] myList;
    public static int condivisoDaTutteLeIstanze = 60;
    //Costruttore
    public MyMathClass (int[] myList) {
        this.myList = myList;
    }
    //Costruttore overloading
    public MyMathClass (int numberOfElemnts, int valueOfElements) {
        myList = new int[numberOfElemnts];
        for (int i = 0; i < numberOfElemnts; i++) {
            myList[i] = valueOfElements;
        }
    }

    //Scorciatoia per fare SumOfElements (0, myList.Length) cioè ritornare la somma di tutti gli elementi della lista.
    public int SumOfElements () {
        return SumOfElements (0 , myList.Length);
    }

    //Overloading su funzione
    public int SumOfElements (int startIndex, int endIndex) {
        int tempSum = 0;
        if (endIndex >= myList.Length) {
            endIndex = myList.Length;
        }
        if (startIndex < 0) {
            startIndex = 0;
        }
        for (int i = startIndex; i < endIndex; i++) {
            tempSum += myList[i]; //tempSum = tempSum + myList[i];
        }
        return tempSum;
    }


    //Una funzione può essere normale
    public int GetMax () {
        return GetMax (myList);
    }
    //Oppure può essere statica
    public static int GetMax (int[] list) {
        if  (list.Length == 0) {
            return -1;
        } else {
            int max = list[0];
            for (int i = 1; i < list.Length; i++) {
                if (list[i] > max) {
                    max = list[i];
                }
            }
            return max;
        }
    }

}
