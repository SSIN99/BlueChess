using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsedUnitUI : MonoBehaviour
{
    [SerializeField] private Info info;
    [SerializeField] private Image portrait;
    [SerializeField] private Image grade;
    [SerializeField] private GameObject[] itemList;
    [SerializeField] private Image[] item;

    public void SetUI(Unit unit)
    {
        portrait.sprite = info.portraits[unit.No];
        grade.sprite = info.gradeIcon[unit.Grade - 1];
        for(int i = 0; i< unit.ItemCount; i++)
        {
            itemList[i].SetActive(true);
            item[i].sprite = info.itemIcon[unit.itemList[i]];
        }
    } 
}
