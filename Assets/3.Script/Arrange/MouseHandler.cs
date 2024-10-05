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
    [SerializeField] GameObject outlineFriend;
    [SerializeField] GameObject outlineEnemy;
    private bool isOutlineEnemy;
    private Unit unit;
    private GameObject hand;
    
    public void SetHand(GameObject target)
    {
        hand = target;
        if (hand == null)
        {
            EndDrag();
        }
        else
        {
            OnDrag();
        }
    }
    private void OnDrag()
    {
        hand.TryGetComponent<Unit>(out unit);
        if (!round.IsBattleStep)
            field.SetActive(true);
        bench.SetActive(true);
        sellUI[0].Open(unit.Grade, unit.Cost);
        sellUI[1].Open(unit.Grade, unit.Cost);
        LayerMask layerMask = LayerMask.GetMask("Tile");
        raycaster.eventMask = layerMask;
    }
    private void EndDrag()
    {
        field.SetActive(false);
        bench.SetActive(false);
        sellUI[0].Close();
        sellUI[1].Close();
        LayerMask layerMask = LayerMask.GetMask("Bench", "Field", "Enemy");
        raycaster.eventMask = layerMask;
    }
    public void SetOutline(Transform unit)
    {

        if(unit == null)
        {
            if (isOutlineEnemy)
            {
                outlineEnemy.transform.parent = transform;
                outlineEnemy.SetActive(false);
            }
            else
            {
                outlineFriend.transform.parent = transform;
                outlineFriend.SetActive(false);
            }
        }
        else
        {
            if (unit.CompareTag("Enemy"))
            {
                isOutlineEnemy = true;
                outlineEnemy.transform.position = new Vector3(unit.position.x, 0.2f, unit.position.z);
                outlineEnemy.SetActive(true);
                outlineEnemy.transform.parent = unit;
            }
            else
            {
                isOutlineEnemy = false;
                outlineFriend.transform.position = new Vector3(unit.position.x, 0.2f, unit.position.z);
                outlineFriend.SetActive(true);
                outlineFriend.transform.parent = unit;
            }
        }
    }
}
