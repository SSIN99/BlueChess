using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Text levelText;
    [SerializeField] private Text goldText;
    [SerializeField] private Text expText;
    [SerializeField] private Slider expSlider;
    [SerializeField] private Image lockImage;
    [SerializeField] private Sprite[] lockSprite;
    
    [SerializeField] private ShopManager shop;
    [SerializeField] private Info info;
    private int level;
    private int gold;
    private int maxExp;
    private int curExp;
    private bool isLocked;
    private int[] maxExpList = { 2, 4, 6, 10, 20, 36, 48, 76 };

    private List<GameObject> unitList;
    public event Action OnLevelUp;
    public int Level
    {
        get { return level; }
        private set
        {
            level = value;
            LevelUp();
            OnLevelUp?.Invoke();
        }
    }
    public int Gold
    {
        get { return gold; }
        private set
        {
            gold = value;
            goldText.text = gold.ToString();
        }
    }
 
    private void Start()
    {
        Level = 1;
        curExp = 0;
        Gold = 1000;
        isLocked = false;
        unitList = new List<GameObject>();
    }
    public bool PayCost(int cost)
    {
        if(gold < cost)
        {
            Debug.Log("돈 부족");
            return false;
        }
        else
        {
            Gold -= cost;
            return true;
        }
    }
    public void LevelUp()
    {
        levelText.text = $"Lv.{level.ToString()}";
        if (level >= 9)
        {
            expSlider.value = expSlider.maxValue;
            expText.text = "Max";
        }
        else
        {
            maxExp = maxExpList[level - 1];
            expSlider.maxValue = maxExp;
            expSlider.value = curExp;
            expText.text = $"{curExp}/{maxExp}";
        }
    }
    public void PurchaseExp()
    {
        if(level.Equals(9))
        {
            Debug.Log("최대 레벨");
            return;
        }

        if (PayCost(4))
        {
            curExp += 4;
            if (curExp >= maxExp)
            {
                curExp -= maxExp;
                Level++;
            }
            else
            {
                expSlider.value = curExp;
                expText.text = $"{curExp}/{maxExp}";
            }
        }
    }
    public void Recruit()
    {
        if (isLocked)
        {
            Debug.Log("잠금상태");
            return;
        }
        if (PayCost(2))
        {
            shop.SetShopItem();
        }
    }
    public void PurchaseUnit(int n, int cost)
    {
        Gold -= cost;
        GameObject newUnit = Instantiate(info.prefabs[n], Vector3.zero, Quaternion.identity);
        unitList.Add(newUnit);
        newUnit.GetComponent<Unit>().InitData(info.unitData[n]);
    }
    public void ToggleLock()
    {
        isLocked = !isLocked;
        lockImage.sprite = isLocked ? lockSprite[0] : lockSprite[1];
    }
}
