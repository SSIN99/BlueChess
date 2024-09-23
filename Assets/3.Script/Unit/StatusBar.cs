using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    private Unit unit;
    [SerializeField] private Transform pivot;
    [SerializeField] private Sprite[] gradeImages;
    [SerializeField] private Image grade;
    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider mpBar;

    private void Awake()
    {
        unit = transform.parent.GetComponent<Unit>();
    }
    private void OnEnable()
    {
        unit.OnGradeChanged += UpdateGrade;
        unit.OnCurHpChanged += UpdateCurHp;
        unit.OnMaxHpChanged += UpdateMaxHp;
        unit.OnCurMpChanged += UpdateCurMp;
        unit.OnMaxMpChanged += UpdateMaxMp;
        unit.OnIsDeadChanged += SetActiveStatus;
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
        pivot.transform.forward = Camera.main.transform.forward;
    }
    private void OnDisable()
    {
        unit.OnGradeChanged -= UpdateGrade;
        unit.OnCurHpChanged -= UpdateCurHp;
        unit.OnMaxHpChanged -= UpdateMaxHp;
        unit.OnCurMpChanged -= UpdateCurMp;
        unit.OnMaxMpChanged -= UpdateMaxMp;
        unit.OnIsDeadChanged -= SetActiveStatus;
    }
}
