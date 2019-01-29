using UnityEngine;

public class TransformExample : MonoBehaviour {

    [SerializeField]
    private Transform[] myTrasforms;
    [SerializeField]
    private float distance;
    [SerializeField]
    private int a, b;

    void Start () {
        AlignGrid ();
        //Prima calcolo la distanza normale
        float performedDistance = CalculateDistance (a , b);
        Debug.Log ("Distanza calcolata = " + performedDistance);
        if ( performedDistance > distance) {
            Debug.Log ("Maggiore");
        } else {
            Debug.Log ("Minore");
        }
        //Poi calcolo la distanza al quadrato
        performedDistance = CalculateSquareDistance (a , b);
        Debug.Log ("Distanza calcolata = " + performedDistance);
        if (CalculateSquareDistance (a,b) > distance * distance) {
            Debug.Log ("Maggiore");
        } else {
            Debug.Log ("Minore");
        }
    }

    /// <summary>
    /// Allinea orizontalmente la griglia.
    /// </summary>
    private void AlignGrid () {
        Vector3 supportPosition = new Vector3 ();
        int indexTransform = 0;
        supportPosition.z = 0;
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 5; j++) {
                supportPosition.x = j * 2;
                supportPosition.y = i * 2;
                myTrasforms[indexTransform].position = supportPosition;
                indexTransform++;
            }
        }
    }

    /// <summary>
    /// Calcola la distanza tra due vettori a un determinato indice
    /// </summary>
    /// <param name="a">Indice primo vettore</param>
    /// <param name="b">Indice secondo vettore</param>
    private float CalculateDistance (int a, int b) { 
        return Vector3.Distance (myTrasforms[a].localPosition , myTrasforms[b].localPosition);
    }

    private float CalculateSquareDistance (int a, int b) {
        return (myTrasforms[a].localPosition - myTrasforms[b].localPosition).sqrMagnitude;
    }

}
