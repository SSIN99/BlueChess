using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Info info;
    [SerializeField] private RectTransform inventory;
    [SerializeField] private RectTransform[] cells;
    [SerializeField] private GameObject item;
    [SerializeField] private Text countText;
    [SerializeField] private Image btnIcon;
    [SerializeField] private Sprite[] btnImage;
    private bool isOpend = false;
    private List<Item> itemList;

    public void Start()
    {
        itemList = new List<Item>();
    }

    public void OpenInventory()
    {
        if (isOpend)
        {
            isOpend = false;
            inventory.DOAnchorPosX(-375f, 0.2f);
            btnIcon.sprite = btnImage[0];
        }
        else
        {
            isOpend = true;
            inventory.DOAnchorPosX(20f, 0.2f);
            btnIcon.sprite = btnImage[1];
        }
    }
    public void AddItem(int no)
    {
        if (itemList.Count >= 12) return;

        GameObject itemObject = Instantiate(item);
        itemObject.transform.SetParent(inventory);
        itemObject.transform.position = cells[itemList.Count].position;

        Item newItem = itemObject.GetComponent<Item>();
        itemList.Add(newItem);
        newItem.InitInfo(no);

        countText.text = $"{itemList.Count}";
    }
    public void RemoveItem(Item item)
    {
        itemList.Remove(item);
        countText.text = $"{itemList.Count}";
        Sorting();
    }
    public void Sorting()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].transform.position = cells[i].position;
        }
    }
    public void GetRandomItem()
    {
        int rand = Random.Range(0, info.Items.Count);
        AddItem(rand);
    }
}
