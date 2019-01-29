using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPowerUp : MonoBehaviour
{

    [SerializeField]
    private Vector2 myVector;
    [SerializeField]
    private bool isForce;
    

    public Vector2 MyVector
    {
        get { return myVector; }
    }

    public bool IsForce
    {
        get { return isForce; }
    }
    

}
