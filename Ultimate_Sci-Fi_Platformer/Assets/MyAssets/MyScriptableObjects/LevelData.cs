using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "LevelData" , menuName = "Custom/LevelData" , order = 1)]
public class LevelData : ScriptableObject
{

    [SerializeField]
    private float _movingPlatform_timeToReachCheckPoint;
    public float TimeToReachCheckPoint_MovingPlatform
    {
        get { return _movingPlatform_timeToReachCheckPoint; }
    }
    [SerializeField]
    private float _movingPlatform_timeOfWaiting;
    public float TimeOfWaiting_MovingPlatform
    {
        get { return _movingPlatform_timeOfWaiting; }
    }
    [SerializeField]
    private float _fallingPlatform_timeToVanish;
    public float TimeToVanish_FallingPlatform
    {
        get { return _fallingPlatform_timeToVanish; }
    }
    [SerializeField]
    private float _hammerLike_waitingTime;
    public float WaitingTime_HammerLike
    {
        get { return _hammerLike_waitingTime; }
    }
    [SerializeField]
    private float _hammerLike_riseTime;
    public float RiseTime_HammerLike
    {
        get { return _hammerLike_riseTime; }
    }
    [SerializeField]
    private float _hammerLike_playerDistance;
    public float PlayerDistance_HammerLike
    {
        get { return _hammerLike_playerDistance; }
    }
}
