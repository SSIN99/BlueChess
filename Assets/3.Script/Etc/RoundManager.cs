using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum StepType
{
    Prepare,
    Battle
}

public class RoundManager : MonoBehaviour
{
    //UI
    [SerializeField] private GameObject startUnitPanel;
    [SerializeField] private GameObject fieldText;
    [SerializeField] private Text roundText;
    [SerializeField] private Text stepText;
    [SerializeField] private Button battleStartBtn;
    [SerializeField] private Text btnText;
    [SerializeField] private StartItem[] unitList;

    [SerializeField] private Info info;
    [SerializeField] private Player player;
    [SerializeField] private ShopManager shop;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform[] spawnPivots;
    [SerializeField] private GameObject statusBar;
    [SerializeField] private Transform canvas;
    private int round;
    private int benefit;
    private List<Unit> enemyList;
    private int enemyCount;
    private int fieldCount;
    private StepType step;
    public StepType Step
    {
        get { return step; }
        private set
        {
            step = value;
            OnStepChange?.Invoke();
        }
    }
    public bool IsBattleStep => step == StepType.Battle;

    public event Action OnStepChange;

    private void Start()
    {
        round = 1;
        benefit = 5;
        enemyList = new List<Unit>();
        step = StepType.Prepare;
        SerStartUnit();
    }
    public void BattleStart()
    {
        battleStartBtn.interactable = false;
        fieldText.SetActive(false);
        stepText.text = "전투 단계";
        btnText.text = "전투 중...";
        for(int i =0; i < player.fieldList.Count; i++)
        {
            player.fieldList[i].OnDead += FieldDead;
        }
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].IsBattle = true;
        }
        Step = StepType.Battle;
    }
    public void PrepareStart()
    {
        battleStartBtn.interactable = true;
        fieldText.SetActive(true);
        stepText.text = "준비 단계";
        btnText.text = "전투 시작";
        round += 1;
        roundText.text = $"ROUND {round}";
        player.GetExp(2);
        if (!player.isLocked)
            shop.SetShopItem();
        player.GetGold(benefit);
        EnemySpawn();
        for (int i = 0; i < player.fieldList.Count; i++)
        {
            player.fieldList[i].OnDead -= FieldDead;
        }
        Step = StepType.Prepare;
    }
    public void EnemySpawn()
    {
        enemyList.Clear();
        Dictionary<string, string> data = info.Rounds[round - 1];
        int enemy1 = int.Parse(data["Enemy1"]);
        int count1 = int.Parse(data["Count1"]);
        int enemy2 = 0;
        int count2 = 0;
        if(data["Enemy2"] != string.Empty)
        {
            enemy2 = int.Parse(data["Enemy2"]);
            count2 = int.Parse(data["Count2"]);
        }
        int enemy3 = 0;
        int count3 = 0;
        if (data["Enemy3"] != string.Empty)
        {
            enemy3 = int.Parse(data["Enemy3"]);
            count3 = int.Parse(data["Count3"]);
        }
        for(int i = 0; i < count1; i++)
        {
            GameObject temp = Instantiate(enemyPrefabs[enemy1]);
            Unit enemy = temp.GetComponent<Unit>();
            enemyList.Add(enemy);
        }
        for (int i = 0; i < count2; i++)
        {
            GameObject temp = Instantiate(enemyPrefabs[enemy2]);
            Unit enemy = temp.GetComponent<Unit>();
            enemyList.Add(enemy);
        }
        for (int i = 0; i < count3; i++)
        {
            GameObject temp = Instantiate(enemyPrefabs[enemy3]);
            Unit enemy = temp.GetComponent<Unit>();
            enemyList.Add(enemy);
        }
        enemyCount = enemyList.Count;
        for(int i = 0; i < enemyCount; i++)
        {
            enemyList[i].transform.position = spawnPivots[int.Parse(data[$"Pos{i + 1}"])].position;
            GameObject status = Instantiate(statusBar);
            status.GetComponent<StatusBar>().SetUnit(enemyList[i]);
            status.transform.SetParent(canvas);
            enemyList[i].InitInfo(info.Enemies[enemy1]);
            enemyList[i].OnDead += EnemyDead;
        }
    }
    private void EnemyDead()
    {
        enemyCount -= 1;
        if(enemyCount <= 0)
        {
            Invoke("PrepareStart", 3.0f);
        }
    }
    private void FieldDead()
    {
        fieldCount -= 1;
        if (fieldCount <= 0)
        {
            Invoke("PlayerLose", 3.0f);
        }
    }
    private void PlayerLose()
    {
        Debug.Log("playerlose");
    }
    public void GameStart()
    {
        foreach (var unit in unitList)
        {
            player.GetUnit(unit.no);
        }
        startUnitPanel.SetActive(false);
        EnemySpawn();
    }
    public void SerStartUnit()
    {
        int rand = 0;
        foreach (var unit in unitList)
        {
            rand = Random.Range(0 , 13);
            unit.SetItem(rand);
        }
    }
    
    public void SetRandomItem()
    {

    }
}
