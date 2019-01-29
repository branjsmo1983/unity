using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public BallController ball;
    public UIController myUI;

    public Transform leftPaddleTransform;
    public Transform rightPaddleTransform;

    int scoreLeftPlayer;
    int scoreRightPlayer;
    


    void Start () {
        Reset ();
    }

    public void Reset () {
        scoreLeftPlayer = 0;
        scoreRightPlayer = 0;
        myUI.UpdateScore (scoreLeftPlayer , scoreRightPlayer);
        ball.InitializeBall ();
    }

    public void PlayerScore (bool left) {
        if (left) {
            scoreLeftPlayer++;
            ball.InitializeBall (rightPaddleTransform);
        } else {
            scoreRightPlayer++;
            ball.InitializeBall (leftPaddleTransform);
        }
        myUI.UpdateScore (scoreLeftPlayer , scoreRightPlayer);
    }

}
