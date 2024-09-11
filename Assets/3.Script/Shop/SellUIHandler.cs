using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SellUIHandler : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Player player;
    [SerializeField] private Image image;

    public void OnDrop(PointerEventData eventData)
    {
        player.SellUnit(eventData.pointerDrag.GetComponent<Unit>());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = new Color(0.8f, 0.8f, 0.8f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = new Color(1,1,1);
    }
}
