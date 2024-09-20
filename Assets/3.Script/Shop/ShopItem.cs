using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    #region UI
    [SerializeField] private Image border;
    [SerializeField] private Image memorial;
    [SerializeField] private Image originIcon;
    [SerializeField] private Image classIcon;
    [SerializeField] private Text nameText;
    [SerializeField] private Text costText;
    [SerializeField] private Text originText;
    [SerializeField] private Text classText;
    private Color[] colorList =
   {
        new Color(1f, 1f, 1f),
        new Color(0.35f, 1f, 0.7f),
        new Color(0.45f, 1f, 1f),
        new Color(1f, 0.35f, 1f),
        new Color(1f, 0.81f, 0.25f)
    };
    #endregion

    #region Info
    [SerializeField] private Info info;
    [SerializeField] private Player player;
    public int no;
    private int cost;
    private GameObject item;
    #endregion

    public void SetItem(int n)
    {
        no = n;
        item = info.unitPool[n].Dequeue();

        UnitControl unit = item.GetComponent<UnitControl>();
       
        border.color = colorList[unit.Cost - 1];
        memorial.sprite = info.memorials[n];
        originIcon.sprite = info.traits[unit.Origin];
        classIcon.sprite = info.traits[unit.Class];
        nameText.text = unit.Name;
        cost = unit.Cost;
        costText.text = cost.ToString(); 
        originText.text = info.traitData[unit.Origin]["Name"];
        classText.text = info.traitData[unit.Class]["Name"];

    }
    public void ReturnItem()
    {
        info.unitPool[no].Enqueue(item.gameObject);   
    }
    public void OnClicked()
    {
        if (player.Gold < cost ||
            player.isFullBench)
        {
            return;
        }
        player.PurchaseUnit(no,item);
        gameObject.SetActive(false);
    }
}
