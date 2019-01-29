using UnityEditor;
using UnityEngine;

[CreateAssetMenu (fileName = "LevelData" , menuName = "Custom/LevelData" , order = 2)]
public class LevelData : ScriptableObject {

    [SerializeField]
    private int _numberOfEnemies;
    [SerializeField]
    private int _enemiesDamage;
    [SerializeField]
    private int _playerHealth;
    [SerializeField]
    private float _scoreMultiplier;
    [SerializeField]
    private AudioClip _hitByEnemy;


    public int NumberOfEnemies
    {
        get { return _numberOfEnemies; }
    }

    public int EnemiesDamage
    {
        get { return _enemiesDamage; }
    }

    public int PlayerHealth
    {
        get { return _playerHealth; }
    }

    public float ScoreMultiplier
    {
        get { return _scoreMultiplier; }
    }

    public AudioClip HitByEnemny
    {
        get { return _hitByEnemy; }
    }
}
