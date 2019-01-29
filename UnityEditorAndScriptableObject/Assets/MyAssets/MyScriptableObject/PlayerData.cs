using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "PlayerData" , menuName = "Custom/PlayerData" , order = 3)]
public class PlayerData : ScriptableObject
{
    [SerializeField]
    private float m_WalkSpeed;
    [SerializeField]
    private float m_RunSpeed;
    [SerializeField]
    private float m_JumpSpeed;
    [SerializeField]
    private float m_StickToGroundForce;
    [SerializeField]
    private float m_GravityMultiplier;

    public float WalkSpeed
    {
        get { return m_WalkSpeed; }
    }

    public float RunSpeed
    {
        get { return m_RunSpeed; }
    }

    public float JumpSpeed
    {
        get { return m_JumpSpeed; }
    }

    public float StickToGroundForce
    {
        get { return m_StickToGroundForce; }
    }

    public float GravitiMutliplier
    {
        get { return m_GravityMultiplier; }
    }
}
