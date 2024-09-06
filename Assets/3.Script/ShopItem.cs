using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [Header("UnitInfo Image")]
    [SerializeField] private Image border;
    [SerializeField] private Image memorial;
    [SerializeField] private Image originIcon;
    [SerializeField] private Image classIcon;
    [Header("UnitInfo Text")]
    [SerializeField] private Text nameText;
    [SerializeField] private Text costText;
    [SerializeField] private Text originText;
    [SerializeField] private Text classText;

    [Header("External Class")]
    [SerializeField] private Info info;
    [SerializeField] private Player player;

    private int cost;
    private GameObject unit;
    private Color[] colorOfCost =
    {
        new Color(1f, 1f, 1f),
        new Color(0.35f, 1f, 0.7f),
        new Color(0.45f, 1f, 1f),
        new Color(1f, 0.35f, 1f),
        new Color(1f, 0.81f, 0.25f)
    };
    public int no = -1;

    public void SetItemInfo(int n)
    {
        if (no.Equals(n)) return;

        no = n;
        unit = info.unitPool[n].Dequeue();
        border.color = colorOfCost[int.Parse(info.unitData[n]["Cost"]) - 1];
        memorial.sprite = info.memorials[n];
        originIcon.sprite = info.traits[int.Parse(info.unitData[n]["Origin"])];
        classIcon.sprite = info.traits[int.Parse(info.unitData[n]["Class"])];

        nameText.text = info.unitData[n]["Name"];
        cost = int.Parse(info.unitData[n]["Cost"]);
        costText.text = cost.ToString(); 
        originText.text = info.traitData[int.Parse(info.unitData[n]["Origin"])]["Name"];
        classText.text = info.traitData[int.Parse(info.unitData[n]["Class"])]["Name"];

    }

    public void OnClicked()
    {
        if(player.Gold < cost)
        {
            return;
        }
        player.PurchaseUnit(no, unit, cost);
        gameObject.SetActive(false);
    }
    public void ReturnUnit()
    {
        info.unitPool[no].Enqueue(unit);
        unit = null;
        no = -1;
    }
    private void OnDisable()
    {
        unit = null;
        no = -1;
    }
}
