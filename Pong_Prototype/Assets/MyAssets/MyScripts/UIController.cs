using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Text leftPlayerScore;
    [SerializeField]
    private Text rightPlayerScore;

    public void UpdateScore (int leftPlayerScore, int rightPlayerScore) {
        this.leftPlayerScore.text = leftPlayerScore.ToString ();
        this.rightPlayerScore.text = rightPlayerScore.ToString ();
    }

}
