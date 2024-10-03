using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public enum StepType
{
    Prepare,
    Battle
}

public class RoundManager : MonoBehaviour
{
    //스타트 유닛
    [Header("Start Unit")]
    [SerializeField] private GameObject startUnitPanel;
    [SerializeField] private StartItem[] unitList;
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
            rand = Random.Range(0, 13);
            unit.SetItem(rand);
        }
    }

    //아이템 획득
    [Header("Item Shop")]
    [SerializeField] private GameObject itemShopPanel;
    [SerializeField] private Button rerollBtn;
    [SerializeField] private Text rerollBtnText;
    [SerializeField] private ItemShopUI[] itemList;
    private int rerollCount;
    public void OpenItemShop()
    {
        itemShopPanel.SetActive(true);
        rerollCount = 2;
        rerollBtnText.text = "2";
        rerollBtn.interactable = true;
        SetRandomItem();
    }
    public void SetRandomItem()
    {
        int rand = 0;
        foreach (var item in itemList)
        {
            rand = Random.Range(0, 9);
            item.SetItem(rand);
        }
        rerollCount -= 1;
        rerollBtnText.text = $"{rerollCount}";
        if(rerollCount == 0)
            rerollBtn.interactable = false;
    }
    public void GetItem()
    {
        for(int i = 0; i< itemList.Length; i++)
        {
            player.AddItem(itemList[i].No);
        }
        itemShopPanel.SetActive(false);
    }
    //결과창
    [Header("Result")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private GameObject victoryText;
    [SerializeField] private GameObject defeatText;
    [SerializeField] private Volume postprocessing;
    [SerializeField] private Text bestRoundText;
    [SerializeField] private Text curRoundText;
    [SerializeField] private UsedUnitUI[] usedList;

    public void ShowResult()
    {
        resultPanel.SetActive(true);
        if (isVictory)
            victoryText.SetActive(true);
        else
            defeatText.SetActive(true);
        bestRoundText.text = $"최고 라운드 : 0";
        curRoundText.text = $"이번 라운드 : {round - 1}";
        for (int i = 0; i < player.fieldList.Count; i++)
        {
            usedList[i].gameObject.SetActive(true);
            usedList[i].SetUI(player.fieldList[i]);
        }
    }
    public void BackLobby()
    {
        SceneManager.LoadScene("Lobby");
    }

    //기본
    [Header("Basic")]
    [SerializeField] private Info info; 
    [SerializeField] private Player player;
    [SerializeField] private ShopManager shop;
    [SerializeField] private GameObject fieldText;
    [SerializeField] private Text roundText;
    [SerializeField] private Text stepText;
    private int round;
    private int benefit;
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
    public event Action OnStepChange;
    public bool IsBattleStep => step == StepType.Battle;
    public void BattleStart()
    {
        battleStartBtn.interactable = false;
        fieldText.SetActive(false);
        stepText.text = "전투 단계";
        btnText.text = "전투 중...";
        for (int i = 0; i < player.fieldList.Count; i++)
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
        EnemySpawn();
        for (int i = 0; i < player.fieldList.Count; i++)
        {
            player.fieldList[i].OnDead -= FieldDead;
        }
        if((round % 5) == 1)
        {
            benefit += 5;
            OpenItemShop();
        }
        player.GetGold(benefit);
        Step = StepType.Prepare;
    }

    //에너미 스폰
    [Header("Enemy Spawn")]
    [SerializeField] private Button battleStartBtn;
    [SerializeField] private Text btnText;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform[] spawnPivots;
    [SerializeField] private GameObject statusBar;
    [SerializeField] private Transform canvas;
    private List<Unit> enemyList;
    public void EnemySpawn()
    {
        enemyList.Clear();
        Dictionary<string, string> data = info.Rounds[round - 1];
        int enemy1 = int.Parse(data["Enemy1"]);
        int count1 = int.Parse(data["Count1"]);
        int enemy2 = 0;
        int count2 = 0;
        if (data["Enemy2"] != string.Empty)
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
        for (int i = 0; i < count1; i++)
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
        for (int i = 0; i < enemyCount; i++)
        {
            enemyList[i].transform.position = spawnPivots[int.Parse(data[$"Pos{i + 1}"])].position;
            GameObject status = Instantiate(statusBar);
            status.GetComponent<StatusBar>().SetUnit(enemyList[i]);
            status.transform.SetParent(canvas);
            enemyList[i].InitInfo(info.Enemies[enemy1]);
            enemyList[i].OnDead += EnemyDead;
        }
    }

    //전투 관련
    [SerializeField] private GameObject confetti;
    private int enemyCount;
    private int fieldCount;
    private bool isVictory;
    private void EnemyDead()
    {
        enemyCount -= 1;
        if (enemyCount <= 0)
        {
            if(round == 25)
            {
                isVictory = true;
                confetti.SetActive(true);
                Invoke("ShowResult", 3.0f);
            }
            else
            {
                Invoke("PrepareStart", 3.0f);
            }
        }
    }
    private void FieldDead()
    {
        fieldCount -= 1;
        if (fieldCount <= 0)
        {
            postprocessing.weight = 1f;
            isVictory = false;
            Invoke("ShowResult", 3.0f);
        }
    }

    private void Start()
    {
        round = 1;
        benefit = 5;
        rerollCount = 2;
        enemyList = new List<Unit>();
        step = StepType.Prepare;
        SerStartUnit();
    }

}
