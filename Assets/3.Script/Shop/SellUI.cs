using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
public class SellUI : MonoBehaviour, IDropHandler
{
    [SerializeField] private UnitManager unitManager;
    [SerializeField] private Text cost;
    [SerializeField] private RectTransform rect;
    [SerializeField] private bool isLeft;

    public void Open(int grade, int cost)
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
                this.cost.text = $"+{cost}";
                break;
            case 2:
                this.cost.text = $"+{cost * 3 - 1}";
                break;
            case 3:
                this.cost.text = $"+{cost * 9 - 1}";
                break;
        }
    }
    public void Close()
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
        unitManager.SellUnit(eventData.pointerDrag);
    }
}
