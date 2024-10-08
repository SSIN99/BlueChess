using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopItemUI : MonoBehaviour
{
    [SerializeField] private Info info;
    [SerializeField] private Image Icon;
    [SerializeField] private Text Name;
    [SerializeField] private Text Effect;
    public int No;

    public void SetItem(int no)
    {
        No = no;
        Icon.sprite = info.itemIcon[no];
        Name.text = info.Items[no]["Name"];
        Effect.text = info.Items[no]["Script"];
    }
}
