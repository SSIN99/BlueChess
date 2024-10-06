using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseHandler : MonoBehaviour
{
    [SerializeField] private RoundManager round;
    [SerializeField] private GameObject bench;
    [SerializeField] private GameObject field;
    [SerializeField] private SellUI[] sellUI;
    [SerializeField] private PhysicsRaycaster raycaster;
    [SerializeField] private UnitInfoUI unitInfoUI;
    [SerializeField] GameObject outlineFriend;
    [SerializeField] GameObject outlineEnemy;
    [SerializeField] GameObject highlight;
    private bool isOutlineEnemy;
    private GameObject point;
    private GameObject hand;
    private Unit handUnit;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && 
            unitInfoUI.gameObject.activeSelf)
        {
            CloseUnitInfo();
        }
        if (Input.GetMouseButtonDown(1) &&
            point != null)
        {
            OpenUnitInfo();
        }
    }
    public void SetPoint(GameObject target)
    {
        point = target;
        if (point == null)
        {
            EndPoint();
        }
        else
        {
            StartPoint();
        }
    }
    public void StartPoint()
    {
        if (point.CompareTag("Enemy"))
        {
            isOutlineEnemy = true;
            outlineEnemy.transform.position = new Vector3(point.transform.position.x, 0.2f, point.transform.position.z);
            outlineEnemy.SetActive(true);
            outlineEnemy.transform.parent = point.transform;
        }
        else
        {
            isOutlineEnemy = false;
            outlineFriend.transform.position = new Vector3(point.transform.position.x, 0.2f, point.transform.position.z);
            outlineFriend.SetActive(true);
            outlineFriend.transform.parent = point.transform;
        }
    }
    public void EndPoint()
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
    public void SetHand(GameObject target)
    {
        hand = target;
        if (hand == null)
        {
            EndDrag();
        }
        else
        {
            StartDrag();
        }
    }
    private void StartDrag()
    {
        hand.TryGetComponent<Unit>(out handUnit);
        if (!round.IsBattleStep)
            field.SetActive(true);
        bench.SetActive(true);
        sellUI[0].Open(handUnit.Grade, handUnit.Cost);
        sellUI[1].Open(handUnit.Grade, handUnit.Cost);
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
    private void OpenUnitInfo()
    {
        unitInfoUI.gameObject.SetActive(true);
        unitInfoUI.InitInfo(point.GetComponent<Unit>());
        Vector3 pos = point.transform.position;
        pos.y += 0.2f;
        highlight.transform.position = pos;
        highlight.SetActive(true);
        highlight.transform.SetParent(point.transform);
    }
    private void CloseUnitInfo()
    {
        unitInfoUI.gameObject.SetActive(false);
        highlight.SetActive(false);
        highlight.transform.SetParent(transform);
    }
}
