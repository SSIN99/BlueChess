using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseHandler : MonoBehaviour
{
    #region À¯´Ö µå·¡±×
    [SerializeField] private RoundManager round;
    [SerializeField] private GameObject bench;
    [SerializeField] private GameObject field;
    [SerializeField] private GameObject sellUI;
    [SerializeField] private PhysicsRaycaster raycaster;
    [SerializeField] private Text[] costText;
    #endregion

    [SerializeField] GameObject unitInfoUI;
    private bool isDragUnit;
    private GameObject hand;
    public GameObject Hand
    {
        get { return Hand; }
        private set
        {
            hand = value;
            if (hand == null)
            {
                EndDrag();
            }
            else
            {
                OnDrag();
            }
        }
    }
    
    public void SetHand(GameObject target)
    {
        if(target != null)
        {
            isDragUnit = target.CompareTag("Unit") ? true : false;
        }
        Hand = target;
    }
    private void OnDrag()
    {
        if (isDragUnit)
        {
            if(!round.IsBattleStep)
                field.SetActive(true);

            bench.SetActive(true);
            sellUI.SetActive(true);
            LayerMask layerMask = LayerMask.GetMask("Tile");
            raycaster.eventMask = layerMask;
            costText[0].text = $"+{hand.GetComponent<UnitControl>().Cost}";
            costText[1].text = $"+{hand.GetComponent<UnitControl>().Cost}";
        }
    }
    private void EndDrag()
    {
        if (isDragUnit)
        {
            field.SetActive(false);
            bench.SetActive(false);
            sellUI.SetActive(false);
            LayerMask layerMask = LayerMask.GetMask("Unit", "Battle");
            raycaster.eventMask = layerMask;
        }
    }
}
