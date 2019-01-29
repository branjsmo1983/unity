using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerNearMoving : GenericHammerLike
{

   
    private Transform player;

    private float playerDistance;

    protected override void Awake () {
        base.Awake ();
        player = GameObject.FindGameObjectWithTag ("Player").transform;
    }

    public override void OnInitializeScene (MyEventArgs e) {
        InitializeMe (e.myLevelData.WaitingTime_HammerLike , e.myLevelData.RiseTime_HammerLike , e.myLevelData.PlayerDistance_HammerLike);
    }

    public void InitializeMe (float waitingTime , float riseTime, float playerDistance) {
        base.InitializeMe (waitingTime , riseTime);
        this.playerDistance = playerDistance * playerDistance;
        startPosition = upPosition;
    }

    protected override void Update () {
        CheckPlayerPosition ();
        base.Update (); 
    }

    private void CheckPlayerPosition () {
        Vector3 myProjectionPosition = new Vector3 (transform.position.x, player.position.y, transform.position.z);
        canFall = (myProjectionPosition - player.position).sqrMagnitude < playerDistance;
    }

}
