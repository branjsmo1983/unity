using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerNoScriptable : MonoBehaviour {
    [SerializeField]
    private int[] _numberOfEnemies;
    [SerializeField]
    private int[] _enemiesDamage;
    [SerializeField]
    private int[] _playerHealth;
    [SerializeField]
    private float[] _scoreMultiplier;
    [SerializeField]
    private AudioClip[] _hitByEnemy;

}