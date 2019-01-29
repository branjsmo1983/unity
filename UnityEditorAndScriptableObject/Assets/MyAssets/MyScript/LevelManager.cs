using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int debugIndexLevel;

    [SerializeField]
    private LevelData[] myLevelsData;
    [SerializeField]
    private PlayerData[] myPlayersData;
    [SerializeField]
    private MyFirstPersonController player;

    private int currentLevel;
    private LevelData currentLevelData;

    void Start () {
        InitializeMe (debugIndexLevel);
    }

    public void InitializeMe (int levelNumber) {
        currentLevel = levelNumber;
        currentLevelData = myLevelsData[currentLevel];
        Debug.Log ("Numero di nemici del livello: " + currentLevelData.NumberOfEnemies + ", danno dei nemici: " + currentLevelData.EnemiesDamage + ", salute del giocatore: " + currentLevelData.PlayerHealth);
        player.InitializeMe (myPlayersData[levelNumber]);
    }

}
