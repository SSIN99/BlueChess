using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler
{
    [SerializeField] private Image Icon;
    [SerializeField] private Info info;
    [SerializeField] private Player player;
    [SerializeField] private ItemInfoUIHandler popUpUI;

    private RectTransform rect;
    private int No;

    private void Awake()
    {
        info = GameObject.FindGameObjectWithTag("Info").GetComponent<Info>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        popUpUI = GameObject.FindGameObjectWithTag("ItemInfoUI").GetComponent<ItemInfoUIHandler>();
    }

    public void SetItem(int no)
    {
        No = no;
        Icon.sprite = info.itemIcon[no];
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        popUpUI.gameObject.SetActive(true);
        popUpUI.InitInfo(No);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        popUpUI.gameObject.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = Input.mousePosition;
    }
}
