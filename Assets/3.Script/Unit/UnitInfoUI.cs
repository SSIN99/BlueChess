using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoUI : MonoBehaviour
{
    [SerializeField] Image portrait;
    [SerializeField] Image grade;
    [SerializeField] Image costColor;
    private Color[] colors =
    {
        new Color(1f, 1f, 1f),
        new Color(0.35f, 1f, 0.7f),
        new Color(0.45f, 1f, 1f),
        new Color(1f, 0.35f, 1f),
        new Color(1f, 0.81f, 0.25f)
    };
    [SerializeField] Image originIcon;
    [SerializeField] Image classIcon;
    [SerializeField] Text unitName;
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
    [SerializeField] Image[] itemIcon;
    [SerializeField] private Info info;
    private Unit unit;
   
    public void InitInfo(Unit target)
    {
        unit = target;

        int no = unit.No;
        portrait.sprite = unit.isEnemy ? info.enemyPortraits[no] : info.portraits[no];
        grade.sprite = info.gradeIcon[unit.Grade - 1];
        costColor.color = colors[unit.Cost - 1];
        if(unit.Origin == -1)
        {
            originIcon.sprite = null;
            classIcon.sprite = null;
        }
        else
        {
            originIcon.sprite = info.traitIcon[unit.Origin];
            classIcon.sprite = info.traitIcon[unit.Class];
        }
        unitName.text = unit.Name;
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


        skillIcon.sprite = unit.isEnemy ? null : info.skillIcon[unit.No];
        skillName.text = unit.isEnemy ? string.Empty : info.Skills[no]["Name"];
        skillEffect.text = unit.isEnemy ? string.Empty : info.Skills[no]["Effect"];

        ad.text = $"{unit.AD}";
        ap.text = $"{unit.AP}";
        armor.text = $"{unit.Armor}";
        resist.text = $"{unit.Resist}";
        critRatio.text = $"{unit.CritRatio}%";
        critDamage.text = $"{unit.CritDamage}%";
        attackSpeed.text = $"{unit.AS}"; ;
        avoid.text = $"{unit.Avoid}%";
        range.text = $"{unit.range}"; ;

        for(int i = 0; i < itemIcon.Length; i++)
        {
            if (i < unit.ItemCount)
                itemIcon[i].sprite = info.itemIcon[unit.itemList[i]];
            else
                itemIcon[i].sprite = null;
        }

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

    private void OnDisable()
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
