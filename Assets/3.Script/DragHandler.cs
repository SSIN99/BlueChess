using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class DragHandler : MonoBehaviour
{
    [SerializeField] private PhysicsRaycaster raycaster;
    [SerializeField] private GameObject tile;
    [SerializeField] private RectTransform sellUI;
    [SerializeField] private Text[] costText;

    public void StartDrag(int cost)
    {
        tile.SetActive(true);
        LayerMask layerMask = LayerMask.GetMask("Tile");
        raycaster.eventMask = layerMask;
        sellUI.DOScaleX(1, 0.15f);
        costText[0].text = $"+{cost}";
        costText[1].text = $"+{cost}";
    }

    public void EndDrag()
    {
        tile.SetActive(false);
        LayerMask layerMask = LayerMask.GetMask("Unit");
        raycaster.eventMask = layerMask;
        sellUI.DOScaleX(1.5f, 0.15f);
    }
}
