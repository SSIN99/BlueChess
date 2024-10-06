using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject popUpUI;
    [SerializeField] private Image Icon;
    [SerializeField] private Image InfoIcon;
    [SerializeField] private Text Name;
    [SerializeField] private Text Effect;
    [SerializeField] private RectTransform rect;
    [SerializeField] private CanvasGroup itemUI;
    
    
    private Info info;
    private Inventory inventory;
    private Unit unit;
    private Vector3 defaultPos;
    private int no;

    private void Awake()
    {
        info = GameObject.FindGameObjectWithTag("Info").GetComponent<Info>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    public void InitInfo(int no)
    {
        this.no = no;
        Icon.sprite = info.itemIcon[no];
        InfoIcon.sprite = info.itemIcon[no];
        Name.text = info.Items[no]["Name"];
        Effect.text = info.Items[no]["Script"];
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        popUpUI.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        popUpUI.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemUI.blocksRaycasts = false;
        defaultPos = rect.position;
        rect.localScale *= 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = Input.mousePosition;
        if (eventData.pointerEnter == null)
        {
            unit = null;
        }
        else
        {
            unit = eventData.pointerEnter.GetComponent<Unit>();
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(unit == null || 
            unit.IsItemFull ||
            unit.IsBattle)
        {
            itemUI.blocksRaycasts = true;
            rect.position = defaultPos;
            rect.localScale = Vector3.one;
        }
        else
        {
            unit.EquipItem(no);
            inventory.RemoveItem(this);
            Destroy(gameObject);
        }
    }

}
