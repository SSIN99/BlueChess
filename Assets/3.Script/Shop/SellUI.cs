using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
public class SellUI : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Player player;
    [SerializeField] private Image image;
    [SerializeField] private Text text;
    [SerializeField] private RectTransform rect;
    [SerializeField] private bool isLeft;

    public void Active(int grade, int cost)
    {
        if (isLeft)
        {
            rect.DOAnchorPosX(-900, 0.1f);
        }
        else
        {
            rect.DOAnchorPosX(900, 0.1f);
        }
        switch (grade)
        {
            case 1:
                text.text = $"{cost}";
                break;
            case 2:
                text.text = $"{cost * 3 - 1}";
                break;
            case 3:
                text.text = $"{cost * 9 - 1}";
                break;
        }
    }
    public void NonActive()
    {
        if (isLeft)
        {
            rect.DOAnchorPosX(-1200, 0.2f);
        }
        else
        {
            rect.DOAnchorPosX(1200, 0.2f);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        player.SellUnit(eventData.pointerDrag);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = new Color(0.8f, 0.8f, 0.8f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = new Color(1, 1, 1);
    }
}
