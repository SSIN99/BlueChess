using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarUI : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [SerializeField] private Image grade;
    [SerializeField] private Sprite[] gradeSprite;
    [SerializeField] private Slider hpBar;
    [SerializeField] private Image hpBarColor;
    [SerializeField] private Slider mpBar;
    [SerializeField] private Slider shieldBar;
    [SerializeField] private GameObject[] items;
    [SerializeField] private Image[] itemIcon;
    private Unit unit;
    private Info info;
    public void InitInfo(Unit target)
    {
        unit = target;
        unit.OnMaxHpChanged += UpdateMaxHp;
        unit.OnCurHpChanged += UpdateCurHp;
        unit.OnMaxMpChanged += UpdateMaxMp;
        unit.OnCurMpChanged += UpdateCurMp;
        unit.OnMaxShieldChanged += UpdateMaxShield;
        unit.OnCurShieldChanged += UpdateCurShield;
        unit.OnDead += SetActiveStatus;
        unit.OnGradeUp += UpdateGrade;
        if (!unit.isEnemy)
        {
            unit.OnIdleReturn += SetActiveStatus;
            unit.OnItemEquiped += UpdateItem;
            unit.OnBeSold += Delete;
            return;
        }
        hpBarColor.color = Color.red;
    }
    private void Delete()
    {
        Destroy(gameObject);
    }
    private void UpdateGrade()
    {
        grade.sprite = gradeSprite[unit.Grade - 1];
    }
    private void UpdateCurHp()
    {
        hpBar.value = unit.CurHp;
    }
    private void UpdateMaxHp()
    {
        hpBar.maxValue = unit.MaxHp;
    }
    private void UpdateCurMp()
    {
        mpBar.value = unit.CurMp;
    }
    private void UpdateMaxMp()
    {
        mpBar.maxValue = unit.MaxMp;
    }

    private void UpdateCurShield()
    {
        shieldBar.value = unit.CurShield;
    }
    private void UpdateMaxShield()
    {
        shieldBar.maxValue = unit.MaxShield;
        shieldBar.value = shieldBar.maxValue;
    }
    private void UpdateItem()
    {
        items[unit.itemList.Count - 1].SetActive(true);
        itemIcon[unit.itemList.Count - 1].sprite = info.itemIcon[unit.itemList[unit.itemList.Count - 1]];
    }
    private void SetActiveStatus()
    {
        if (unit.isEnemy)
        {
            Destroy(gameObject);
        }
        else
        {
            if (unit.IsDead)
                pivot.gameObject.SetActive(false);
            else
                pivot.gameObject.SetActive(true);
        }
    }
    private void Awake()
    {
        info = GameObject.FindGameObjectWithTag("Info").GetComponent<Info>();
    }
    private void FixedUpdate()
    {
        Vector2 pos = Camera.main.WorldToScreenPoint(unit.transform.position);
        pos.y += 130f;
        transform.position = pos;
    }
}
