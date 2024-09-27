using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] private Transform pivot;
    [SerializeField] private Sprite[] gradeImages;
    [SerializeField] private Image grade;
    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider mpBar;
    [SerializeField] private Slider shieldBar;
    [SerializeField] private GameObject[] items;
    [SerializeField] private Image[] itemIcon;

    public void SetUnit(Unit target)
    {
        unit = target;
        unit.OnGradeChanged += UpdateGrade;
        unit.OnCurHpChanged += UpdateCurHp;
        unit.OnMaxHpChanged += UpdateMaxHp;
        unit.OnCurMpChanged += UpdateCurMp;
        unit.OnMaxMpChanged += UpdateMaxMp;
        unit.OnCurShieldChanged += UpdateCurShield;
        unit.OnMaxShieldChanged += UpdateMaxShield;
        unit.OnDead += SetActiveStatus;
        unit.OnIdleReturn += SetActiveStatus;
        unit.OnItemEquiped += UpdateItemList;
        unit.OnBeSold += Delete;
    }
    private void Delete()
    {
        Destroy(gameObject);
    }
    private void UpdateGrade()
    {
        grade.sprite = gradeImages[unit.Grade - 1];
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
    private void UpdateItemList()
    {
        items[unit.itemList.Count - 1].SetActive(true);
        itemIcon[unit.itemList.Count - 1].sprite = unit.info.itemIcon[unit.itemList[unit.itemList.Count - 1]];
    }
    private void SetActiveStatus()
    {
        if (unit.IsDead)
            pivot.gameObject.SetActive(false);
        else
        {
            pivot.gameObject.SetActive(true);
        }
    }
    private void FixedUpdate()
    {
        Vector2 pos = Camera.main.WorldToScreenPoint(unit.transform.position);
        pos.y += 90f;
        transform.position = pos;
    }
    private void OnDisable()
    {
        unit.OnGradeChanged -= UpdateGrade;
        unit.OnCurHpChanged -= UpdateCurHp;
        unit.OnMaxHpChanged -= UpdateMaxHp;
        unit.OnCurMpChanged -= UpdateCurMp;
        unit.OnMaxMpChanged -= UpdateMaxMp;
        unit.OnCurShieldChanged -= UpdateCurShield;
        unit.OnMaxShieldChanged -= UpdateMaxShield;
        unit.OnDead -= SetActiveStatus;
        unit.OnIdleReturn -= SetActiveStatus;
        unit.OnItemEquiped -= UpdateItemList;
        unit.OnBeSold -= Delete;
    }
}
