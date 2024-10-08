using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemShop : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Button rerollBtn;
    [SerializeField] private Text rerollCountText;
    [SerializeField] private ItemShopItemUI[] itemList;
    private int rerollCount;
    public void OpenItemShop()
    {
        shopPanel.SetActive(true);
        rerollCount = 3;
        rerollBtn.interactable = true;
        SetRandomItem();
    }
    public void SetRandomItem()
    {
        int rand = 0;
        foreach (var item in itemList)
        {
            rand = Random.Range(0, 9);
            item.SetItem(rand);
        }
        rerollCount -= 1;
        rerollCountText.text = $"{rerollCount}";
        if (rerollCount == 0)
            rerollBtn.interactable = false;
    }
    public void GetSelectedItem()
    {
        foreach(var item in itemList)
        {
            inventory.AddItem(item.No);
        }
        shopPanel.SetActive(false);
    }
}
