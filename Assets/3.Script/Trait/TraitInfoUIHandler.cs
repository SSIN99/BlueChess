using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraitInfoUIHandler : MonoBehaviour
{
    [SerializeField] private Info info;
    [SerializeField] private Player player;
    [SerializeField] private Image Icon;
    [SerializeField] private Text Name;
    [SerializeField] private Image[] Borders;
    [SerializeField] private Image[] Portraits;
    [SerializeField] private Text script;
    [SerializeField] private Text[] effects;

    public void InitInfo(int n)
    {
        Icon.sprite = info.traitIcon[n];
        Name.text = info.Traits[n]["Name"];

        for(int i = 0; i <Portraits.Length; i++)
        {
            Borders[i].gameObject.SetActive(false);
            Portraits[i].gameObject.SetActive(false);
        }
        int index = 0;
        for(int i = 0; i< info.Units.Count; i++)
        {
            if(int.Parse(info.Units[i]["Origin"]) == n ||
                int.Parse(info.Units[i]["Class"]) == n)
            {
                switch (int.Parse(info.Units[i]["Cost"]))
                {
                    case 1:
                        Borders[index].color = Color.white;
                        break;
                    case 2:
                        Borders[index].color = new Color(0.3f, 1, 0.7f);
                        break;
                    case 3:
                        Borders[index].color = new Color(0.3f, 0.65f, 1f);
                        break;
                    case 4:
                        Borders[index].color = new Color(0.64f, 0.38f, 1f);
                        break;
                    case 5:
                        Borders[index].color = new Color(1f, 0.85f, 0.2f);
                        break;
                }
                Borders[index].gameObject.SetActive(true);
                Portraits[index].gameObject.SetActive(true);
                Portraits[index].sprite = info.portraits[i];
                if (player.CheckUnitOnField(i))
                {
                    Portraits[index].color = Color.white;
                }
                else
                {
                     Portraits[index].color = Color.gray;
                }

                index++;
                if(index >= int.Parse(info.Traits[n]["Amount"]))
                {
                    break;
                }
            }
        }
        script.text = info.Traits[n]["Script"];
        for(int i = 0; i < effects.Length; i++)
        {
            if(info.Traits[n][$"Effect{i + 1}"] == string.Empty)
            {
                effects[i].gameObject.SetActive(false);
            }
            else
            {
                effects[i].gameObject.SetActive(true);
                effects[i].text = info.Traits[n][$"Effect{i + 1}"];
                Color color = effects[i].color;
                color.a = 0.5f;
                effects[i].color = color;
            }
        }

        int count = player.traitCount[n];
        if(count >= int.Parse(info.Traits[n]["Rank1"]) &&
            count < int.Parse(info.Traits[n]["Rank2"]))
        {
            Color color = effects[0].color;
            color.a = 1f;
            effects[0].color = color;
        }
        else if(count >= int.Parse(info.Traits[n]["Rank2"]) &&
            count < int.Parse(info.Traits[n]["Rank3"]))
        {
            Color color = effects[1].color;
            color.a = 1f;
            effects[1].color = color;
        }
        else if(count >= int.Parse(info.Traits[n]["Rank3"]) &&
            count < int.Parse(info.Traits[n]["Rank4"]))
        {
            Color color = effects[2].color;
            color.a = 1f;
            effects[2].color = color;
        }
        else if(count >= int.Parse(info.Traits[n]["Rank4"]))
        {
            Color color = effects[3].color;
            color.a = 1f;
            effects[3].color = color;
        }
    }

}
