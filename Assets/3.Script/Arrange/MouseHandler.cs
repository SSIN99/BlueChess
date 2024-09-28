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
    [SerializeField] private SellUI[] sellUI;
    [SerializeField] private PhysicsRaycaster raycaster;
    #endregion

    [SerializeField] GameObject unitInfoUI;
    [SerializeField] GameObject outline;
    private Unit unit;
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
        Hand = target;
    }
    private void OnDrag()
    {
        hand.TryGetComponent<Unit>(out unit);
        if (!round.IsBattleStep)
            field.SetActive(true);

        bench.SetActive(true);
        sellUI[0].Active(unit.Grade, unit.Cost);
        sellUI[1].Active(unit.Grade, unit.Cost);
        LayerMask layerMask = LayerMask.GetMask("Tile");
        raycaster.eventMask = layerMask;
    }
    private void EndDrag()
    {
        field.SetActive(false);
        bench.SetActive(false);
        sellUI[0].NonActive();
        sellUI[1].NonActive();
        LayerMask layerMask = LayerMask.GetMask("Bench", "Field");
        raycaster.eventMask = layerMask;
    }
    public void SetOutline(Transform unit)
    {
        if(unit == null)
        {
            outline.transform.parent = transform;
            outline.SetActive(false);

        }
        else
        {
            outline.transform.position = new Vector3(unit.position.x, 0.2f, unit.position.z);
            outline.SetActive(true);
            outline.transform.parent = unit;
        }
    }
}
