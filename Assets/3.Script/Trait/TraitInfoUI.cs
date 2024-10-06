using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraitInfoUI : MonoBehaviour
{
    [SerializeField] private Info info;
    [SerializeField] private UnitManager unitManager;
    [SerializeField] private Image traitIcon;
    [SerializeField] private Text traitName;
    [SerializeField] private Image[] Borders;
    [SerializeField] private Image[] Portraits;
    [SerializeField] private Text script;
    [SerializeField] private Text[] effects;
    private Color[] colors =
    {
        Color.white,
        new Color(0.3f, 1, 0.7f),
        new Color(0.3f, 0.65f, 1f),
        new Color(0.64f, 0.38f, 1f),
        new Color(1f, 0.85f, 0.2f)
    };

    public void InitInfo(int no)
    {
        traitIcon.sprite = info.traitIcon[no];
        traitName.text = info.Traits[no]["Name"];

        int unitNo = 0;
        for (int i = 0; i < Portraits.Length; i++)
        {
            if (i < int.Parse(info.Traits[no]["Amount"]))
            {
                unitNo = int.Parse(info.Traits[no][$"Unit{i + 1}"]);

                Portraits[i].gameObject.SetActive(true);
                Portraits[i].sprite = info.portraits[unitNo];
                Portraits[i].color = unitManager.CheckUnitOnField(unitNo) ? Color.white : Color.gray;

                Borders[i].gameObject.SetActive(true);
                Borders[i].color = colors[int.Parse(info.Units[unitNo]["Cost"]) - 1];
            }
            else
            {
                Borders[i].gameObject.SetActive(false);
                Portraits[i].gameObject.SetActive(false);
            }
        }

        script.text = info.Traits[no]["Script"];
        int rank = unitManager.CheckTraitRank(no);
        for (int i = 0; i < effects.Length; i++)
        {
            if ( i < int.Parse(info.Traits[no]["Count"]))
            {
                effects[i].gameObject.SetActive(true);
                effects[i].text = info.Traits[no][$"Effect{i + 1}"];

                Color color = effects[i].color;
                color.a = i == rank - 1 ? 1f : 0.7f;
                effects[i].color = color;
            }
            else
            {
                effects[i].gameObject.SetActive(false);
        
            }
        }
    }
}
