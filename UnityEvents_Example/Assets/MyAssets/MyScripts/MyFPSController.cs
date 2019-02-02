using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class MyFPSController : FirstPersonController {


    public float maxWalkSpeed = 5f;
    public float maxRunSpeed = 10f;

    protected override void Start () {
        base.Start ();
        MyEventManager.instance.AddListener (MyIndexEvent.powerUpEvent , OnPowerUpTaked);
    }

    public void OnPowerUpTaked (MyEventArgs e) {
        m_WalkSpeed += e.myFloat;
        m_RunSpeed += e.myFloat;
        m_WalkSpeed = m_WalkSpeed > maxWalkSpeed ? maxWalkSpeed : m_WalkSpeed;
        m_RunSpeed = m_RunSpeed > maxRunSpeed ? maxRunSpeed : m_RunSpeed;
    }

}
