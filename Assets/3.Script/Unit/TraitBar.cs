using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TraitBar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Info info;
    [SerializeField] private Image Icon;
    [SerializeField] private Text Name;
    [SerializeField] private Image[] Amount;
    [SerializeField] private GameObject darkPanel;
    [SerializeField] private TraitInfoUIHandler popUpUI;
    private int num;

    public void InitInfo(KeyValuePair<int, int> n, bool isActive)
    {
        num = n.Key;
        Icon.sprite = info.traitIcon[n.Key];
        Name.text = info.Traits[n.Key]["Name"];

        for(int i = 0; i < Amount.Length; i++)
        {
            if(i >= int.Parse(info.Traits[n.Key]["Amount"]))
            {
                Color color = Amount[i].color;
                color.a = 0f;
                Amount[i].color = color;
            }
            else
            {
                Amount[i].color = Color.gray;
            }
        }
        for(int i = 0; i < n.Value; i++)
        {
            Amount[i].color = Color.green;
        }
        if (!isActive)
        {
            //darkPanel.SetActive(true);
            Icon.color = Color.gray;
        }
        else
        {
            //darkPanel.SetActive(false); 
            Icon.color = Color.white;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        popUpUI.gameObject.SetActive(true);
        popUpUI.InitInfo(num);   
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        popUpUI.gameObject.SetActive(false);
    }
}
