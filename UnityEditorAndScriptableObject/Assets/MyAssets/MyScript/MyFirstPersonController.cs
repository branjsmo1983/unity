using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class MyFirstPersonController : FirstPersonController
{
    
    public void InitializeMe (PlayerData levelData) {
        m_WalkSpeed = levelData.WalkSpeed;
        m_RunSpeed = levelData.RunSpeed;
        m_JumpSpeed = levelData.JumpSpeed;
        m_StickToGroundForce = levelData.StickToGroundForce;
        m_GravityMultiplier = levelData.GravitiMutliplier;
    }

}
