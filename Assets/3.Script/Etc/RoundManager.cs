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
    [SerializeField] private Info info;
    [SerializeField] private Player player;
    [SerializeField] private UnitManager unitManager;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private ShopManager shop;
    [SerializeField] private ItemShop itemShop;
    [SerializeField] private ResultManager result;

    [SerializeField] private Text roundText;
    [SerializeField] private Text stepText;
    [SerializeField] private Volume postprocessing;
    [SerializeField] private GameObject confetti;
    [SerializeField] private GameObject battleStartBtn;

    private int unitCount;
    private int enemyCount;
    public int curRound;
    private int benefit;
    private StepType step;
    public event Action OnStepChange;
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

    private void Start()
    {
        curRound = 1;
        benefit = 5;
        step = StepType.Prepare;
    }
    public void BattleStart()
    {
        battleStartBtn.SetActive(false);
        stepText.text = "전투 단계";

        for (int i = 0; i < unitManager.fieldList.Count; i++)
        {
            unitManager.fieldList[i].OnDead += UnitDead;
        }
        unitManager.BattleStart();

        for (int i = 0; i < enemySpawner.enemyList.Count; i++)
        {
            enemySpawner.enemyList[i].OnDead += EnemyDead;
        }
        enemySpawner.BattleStart();

        Step = StepType.Battle;
    }
    public void PrepareStart()
    {
        confetti.SetActive(false);
        battleStartBtn.SetActive(true);
        stepText.text = "준비 단계";
        curRound += 1;
        roundText.text = $"ROUND {curRound}";

        if ((curRound % 5) == 1)
        {
            benefit += 5;
        }
        player.GetExp(2);
        player.UpdateGold(benefit);
        shop.RoundReroll();

        for (int i = 0; i < unitManager.fieldList.Count; i++)
        {
            unitManager.fieldList[i].OnDead -= UnitDead;
        }
        unitManager.PrepareStart();

        enemySpawner.EnemySpawn();

        Step = StepType.Prepare;
    }
  
    private void EnemyDead()
    {
        enemyCount -= 1;
        if (enemyCount <= 0)
        {
            confetti.SetActive(true);
            if (curRound == 25)
            {
                result.ShowResult();
            }
            else
            {
                Invoke("PrepareStart", 3.0f);
            }
        }
    }
    private void UnitDead()
    {
        unitCount -= 1;
        if (unitCount <= 0)
        {
            postprocessing.weight = 1f;
            result.ShowResult();
        }
    }
}
