using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoUIHandler : MonoBehaviour
{
    #region UI
    [SerializeField] Image portrait;
    [SerializeField] Image grade;
    [SerializeField] Sprite[] gradeIcons;
    [SerializeField] Image costColor;
    private Color[] colorList =
   {
        new Color(1f, 1f, 1f),
        new Color(0.35f, 1f, 0.7f),
        new Color(0.45f, 1f, 1f),
        new Color(1f, 0.35f, 1f),
        new Color(1f, 0.81f, 0.25f)
    };
    [SerializeField] Image origin;
    [SerializeField] Image jobClass;
    [SerializeField] Text charName;
    [SerializeField] Text cost;
    [SerializeField] Slider hpBar;
    [SerializeField] Text hpText;
    [SerializeField] Slider mpBar;
    [SerializeField] Text mpText;
    [SerializeField] Image skillIcon;
    [SerializeField] Text skillName;
    [SerializeField] Text skillScript;
    [SerializeField] Text ad;
    [SerializeField] Text ap;
    [SerializeField] Text armor;
    [SerializeField] Text resist;
    [SerializeField] Text critRatio;
    [SerializeField] Text critDamage;
    [SerializeField] Text attackSpeed;
    [SerializeField] Text range;
    #endregion

    [SerializeField] private Info info;
    [SerializeField] private Transform highlight;
    [SerializeField] private GameObject panel;
    public GameObject target;
    private Unit unit;
    private void InitUI()
    {
        portrait.sprite = info.portraits[unit.No];
        grade.sprite = gradeIcons[unit.Grade - 1];
        costColor.color = colorList[unit.Cost - 1];
        origin.sprite = info.traits[unit.Origin];
        jobClass.sprite = info.traits[unit.Class];
        charName.text = unit.Name;
        switch (unit.Grade)
        {
            case 1:
                cost.text = unit.Cost.ToString();
                break;
            case 2:
                cost.text = (unit.Cost * 3 - 1).ToString();
                break;
            case 3:
                cost.text = (unit.Cost * 9 - 1).ToString();
                break;
        }
        hpBar.maxValue = unit.MaxHp;
        hpBar.value = unit.CurHp;
        hpText.text = $"{hpBar.value}/{hpBar.maxValue}";
        mpBar.maxValue = unit.MaxMp;
        mpBar.value = unit.CurMp;
        mpText.text = $"{mpBar.value}/{mpBar.maxValue}";
        skillIcon.sprite = info.skills[unit.No];
        //스킬이름
        //스킬설명 추가
        ad.text = unit.AD.ToString();
        ap.text = unit.AP.ToString();
        armor.text = unit.Armor.ToString();
        resist.text = unit.Resist.ToString();
        critRatio.text = $"{unit.CritRatio}%";
        critDamage.text = $"{unit.CritDamage}%";
        attackSpeed.text = unit.AS.ToString();
        range.text = unit.Range.ToString();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (panel.activeSelf == true)
            {
                TurnOffUI();
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if(target != null)
            {
                TurnOnUI();
            }
        }
    }
    private void TurnOnUI()
    {
        unit = target.GetComponent<Unit>();
        InitUI();
        unit.OnCurHpChanged += UpdateCurHp;
        unit.OnCurMpChanged += UpdateCurMp;
        panel.SetActive(true);
        highlight.gameObject.SetActive(true);
        highlight.position = target.transform.position;
        highlight.parent = target.transform;
    }
    private void TurnOffUI()
    {
        unit.OnCurHpChanged -= UpdateCurHp;
        unit.OnCurMpChanged -= UpdateCurMp;
        panel.SetActive(false);
        highlight.gameObject.SetActive(false);
        highlight.parent = transform;
    }
    private void UpdateCurHp()
    {
        hpBar.value = unit.CurHp;
        hpText.text = $"{hpBar.value}/{hpBar.maxValue}";
    }
    private void UpdateCurMp()
    {
        mpBar.value = unit.CurMp;
        mpText.text = $"{mpBar.value}/{mpBar.maxValue}";
    }
   
}
