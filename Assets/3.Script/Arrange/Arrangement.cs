using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Arrangement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private UnitManager unitManager;
    private MouseHandler mouse;
    private Unit unit;
    private Animator anim;

    private Tile curTile;
    private Tile targetTile;

    private void Awake()
    {
        unitManager = GameObject.FindGameObjectWithTag("Player").GetComponent<UnitManager>();
        mouse = GameObject.FindGameObjectWithTag("Mouse").GetComponent<MouseHandler>();
        unit = GetComponent<Unit>();
        anim = GetComponent<Animator>();
    }
    public void InitTile(Tile tile)
    {
        curTile = tile;
        curTile.unit = this;
        transform.position = curTile.transform.position;
    }
    public void ChangeTile(Tile target)
    {
        if (curTile != null)
        {
            curTile.unit = null;
        }
        if (target.unit != null)
        {
            target.unit.ChangeTile(curTile);
        }
        if(curTile.type != target.type)
        {
            if(curTile.type == Type.Bench)
            {
                unitManager.RemoveBench(unit);
                unitManager.AddField(unit);
            }
            else
            {
                unitManager.RemoveField(unit);
                unitManager.AddBench(unit);
            }
        }
        InitTile(target);
    }
    public void LeaveTile()
    {
        curTile.unit = null;
        curTile = null;
        mouse.SetHand(null);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
       mouse.SetPoint(eventData.pointerEnter);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        mouse.SetPoint(null);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (unit.IsBattle || unit.isEnemy) return;

        anim.Play("PickUp");
        mouse.SetHand(eventData.pointerDrag);
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (unit.IsBattle || unit.isEnemy) return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPos.z));
        worldPos.y = 1f;
        transform.position = worldPos;

        if (eventData.pointerEnter == null)
        {
            targetTile = null;
        }
        else
        {
            targetTile = eventData.pointerEnter.GetComponent<Tile>();
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (unit.IsBattle || unit.isEnemy) return;

        if (targetTile == null)
        {
            transform.position = curTile.transform.position;
        }
        else
        {
            if (targetTile.type == Type.Field &&
                targetTile.unit == null &&
                curTile.type == Type.Bench &&
                unitManager.isFullField)
            {
                transform.position = curTile.transform.position;
            }
            else
            {
                ChangeTile(targetTile);
            }
        }
        anim.Play("Idle");
        mouse.SetHand(null);
    }
}
