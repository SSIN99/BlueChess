using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Info info;
    [SerializeField] private RoundManager roundManager;
    [SerializeField] private GameObject statusBar;
    [SerializeField] private Transform statusBarCanvas;
    [SerializeField] private Transform[] tile;
    public List<Unit> enemyList;

    private void Awake()
    {
        enemyList = new List<Unit>();
    }
    public void EnemySpawn()
    {
        int enemyCount = int.Parse(info.Rounds[roundManager.curRound - 1]["Count"]);

        GameObject temp = null;
        int spawnTile = 0;
        int enemyNo = 0;
        Quaternion viewDirection = Quaternion.Euler(0, -180, 0);
        Unit newEnemy = null;
        StatusBarUI newStatusBar = null;

        for(int i = 0; i < enemyCount; i++)
        {
            enemyNo = int.Parse(info.Rounds[roundManager.curRound - 1][$"Enemy{i}"]);
            spawnTile = int.Parse(info.Rounds[roundManager.curRound - 1][$"Tile{i}"]);

            temp = Instantiate(info.enemyPrefabs[enemyNo], tile[spawnTile].position , viewDirection, transform);
            temp.transform.tag = "Enemy";
            //temp.layer = LayerMask.GetMask("Enemy");

            newEnemy = temp.GetComponent<Unit>();
            newEnemy.isEnemy = true;

            temp = Instantiate(statusBar, statusBarCanvas);
            newStatusBar = temp.GetComponent<StatusBarUI>();
            newStatusBar.InitInfo(newEnemy);
            newEnemy.InitInfo(info.Enemies[enemyNo]);
            
            enemyList.Add(newEnemy);
            Debug.Log(enemyList.Count);
        }
    }
    public void BattleStart()
    {
        Debug.Log(enemyList.Count);
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].IsBattle = true;
        }
        enemyList.Clear();
    }
}
