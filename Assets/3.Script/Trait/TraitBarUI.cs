using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TraitBarUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Info info;
    [SerializeField] private Image Icon;
    [SerializeField] private Text Name;
    [SerializeField] private Image[] cell;
    [SerializeField] private GameObject inactive;
    [SerializeField] private TraitInfoUI popUpUI;
    private int no;

    public void InitInfo(KeyValuePair<int, int> data, bool isActive)
    {
        no = data.Key;
        Icon.sprite = info.traitIcon[data.Key];
        Name.text = info.Traits[data.Key]["Name"];

        for(int i = 0; i < cell.Length; i++)
        {
            if(i < int.Parse(info.Traits[data.Key]["Amount"]))
            {
                cell[i].gameObject.SetActive(true);
                if(i < data.Value)
                {
                    cell[i].color = Color.green;
                }
                else
                {
                    cell[i].color = Color.gray;
                }
            }
            else
            {
                cell[i].gameObject.SetActive(false);
            }
        }
        if (!isActive)
        {
            inactive.SetActive(true);
            Icon.color = Color.gray;
        }
        else
        {
            inactive.SetActive(false); 
            Icon.color = Color.white;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        popUpUI.gameObject.SetActive(true);
        popUpUI.InitInfo(no);   
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        popUpUI.gameObject.SetActive(false);
    }
}
