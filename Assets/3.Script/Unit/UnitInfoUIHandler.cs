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
    [SerializeField] Text skillEffect;
    [SerializeField] Text ad;
    [SerializeField] Text ap;
    [SerializeField] Text armor;
    [SerializeField] Text resist;
    [SerializeField] Text critRatio;
    [SerializeField] Text critDamage;
    [SerializeField] Text attackSpeed;
    [SerializeField] Text avoid;
    [SerializeField] Text range;
    [SerializeField] Image[] items;
    #endregion

    [SerializeField] private Info info;
    [SerializeField] private Transform highlight;
    [SerializeField] private GameObject panel;
    public GameObject target;
    private Unit unit;
    private void InitUI()
    {
        int no = unit.No;
        if (unit.isEnemy)
        {
            portrait.sprite = info.enemyPortraits[no];
        }
        else
        {
            portrait.sprite = info.portraits[no];
        }
        grade.sprite = gradeIcons[unit.Grade - 1];
        costColor.color = colorList[unit.Cost - 1];
        if(unit.Origin == -1)
        {
            origin.sprite = null;
            jobClass.sprite = null;
        }
        else
        {
            origin.sprite = info.traitIcon[unit.Origin];
            jobClass.sprite = info.traitIcon[unit.Class];
        }
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
        skillIcon.sprite = info.skillIcon[unit.No];
        skillName.text = info.Skills[no]["Name"];
        skillEffect.text = info.Skills[no]["Effect"];
        ad.text = unit.AD.ToString();
        ap.text = unit.AP.ToString();
        armor.text = unit.Armor.ToString();
        resist.text = unit.Resist.ToString();
        critRatio.text = $"{unit.CritRatio}%";
        critDamage.text = $"{unit.CritDamage}%";
        attackSpeed.text = unit.AS.ToString();
        avoid.text = $"{unit.Avoid}%";
        range.text = unit.Range.ToString();
        for(int i = 0; i < items.Length; i++)
        {
            if (i < unit.ItemCount)
                items[i].sprite = info.itemIcon[unit.itemList[i]];
            else
                items[i].sprite = null;
        }
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
        panel.SetActive(true);
        highlight.gameObject.SetActive(true);
        Vector3 pos = target.transform.position;
        pos.y += 0.2f;
        highlight.position = pos;
        highlight.parent = target.transform;

        unit.OnMaxHpChanged += UpdateMaxHp;
        unit.OnCurHpChanged += UpdateCurHp;
        unit.OnCurMpChanged += UpdateCurMp;
        unit.OnADChanged += UpdateAD;
        unit.OnAPChanged += UpdateAP;
        unit.OnArmorChanged += UpdateArmor;
        unit.OnResistChanged += UpdateResist;
        unit.OnASChanged += UpdateAS;
        unit.OnAvoidChanged += UpdateAvoid;
    }
    private void TurnOffUI()
    {
        unit.OnMaxHpChanged -= UpdateMaxHp;
        unit.OnCurHpChanged -= UpdateCurHp;
        unit.OnCurMpChanged -= UpdateCurMp;
        unit.OnADChanged -= UpdateAD;
        unit.OnAPChanged -= UpdateAP;
        unit.OnArmorChanged -= UpdateArmor;
        unit.OnResistChanged -= UpdateResist;
        unit.OnASChanged -= UpdateAS;
        unit.OnAvoidChanged -= UpdateAvoid;

        panel.SetActive(false);
        highlight.gameObject.SetActive(false);
        highlight.parent = transform;
    }
    private void UpdateMaxHp()
    {
        hpBar.maxValue = unit.MaxHp;
        hpText.text = $"{hpBar.value}/{hpBar.maxValue}";
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
    private void UpdateAD()
    {
        ad.text = $"{unit.AD}";
    }
    private void UpdateAP()
    {
        ap.text = $"{unit.AP}";
    }
    private void UpdateAS()
    {
        attackSpeed.text = $"{unit.AS}";
    }
    private void UpdateArmor()
    {
        armor.text = $"{unit.Armor}";
    }
    private void UpdateResist()
    {
        resist.text = $"{unit.Resist}";
    }
    private void UpdateAvoid()
    {
        avoid.text = $"{unit.Avoid}";
    }
}
