using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Image Icon;
    [SerializeField] private Info info;
    [SerializeField] private Player player;
    [SerializeField] private ItemInfoUIHandler popUpUI;
    [SerializeField] private RectTransform rect;
    [SerializeField] private Unit targetUnit;
    [SerializeField] private CanvasGroup canvas;
    private Vector3 pos;
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
        popUpUI.InitInfo(No);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        popUpUI.Off();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvas.blocksRaycasts = false;
        pos = rect.position;
        rect.localScale *= 0.5f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = Input.mousePosition;
        if (eventData.pointerEnter == null)
        {
            targetUnit = null;
        }
        else
        {
            targetUnit = eventData.pointerEnter.GetComponent<Unit>();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(targetUnit == null || 
            targetUnit.IsItemFull)
        {
            canvas.blocksRaycasts = true;
            rect.position = pos;
            rect.localScale = Vector3.one;
        }
        else
        {
            targetUnit.EquipItem(No);
            player.RemoveItem(this);
            Destroy(gameObject);
        }
    }

}
