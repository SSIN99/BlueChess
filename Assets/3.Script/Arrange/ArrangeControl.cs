using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArrangeControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Tile curTile;
    private Tile targetTile;
    private Player player;
    private Unit unit;
    private MouseHandler mouse;
    private UnitInfoUIHandler infoUI;
    private Animator anim;
    public bool IsOnField => curTile.type == Type.Field;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        this.player = player.GetComponent<Player>();
        unit = GetComponent<Unit>();
        mouse = player.GetComponent<MouseHandler>();
        infoUI = GameObject.FindGameObjectWithTag("UnitInfoUI").GetComponent<UnitInfoUIHandler>();
        anim = GetComponent<Animator>();
    }
    public void LeaveTile()
    {
        curTile.unit = null;
        curTile = null;
        mouse.SetHand(null);
    }
    public void InitTile(Tile target)
    {
        curTile = target;
        curTile.unit = this;
        transform.position = curTile.transform.position;
    }
    public void SetTile(Tile target)
    {
        if (curTile != null)
        {
            curTile.unit = null;
        }
        if (target.unit != null)
        {
            target.unit.SetTile(curTile);
        }
        if(curTile.type != target.type)
        {
            if(curTile.type == Type.Bench)
            {
                player.RemoveBench(gameObject);
                player.AddField(gameObject);
            }
            else
            {
                player.RemoveField(gameObject);
                player.AddBench(gameObject);
            }
        }
        curTile = target;
        curTile.unit = this;
        transform.position = curTile.transform.position;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
       infoUI.target = gameObject;
       mouse.SetOutline(transform);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        infoUI.target = null;
        mouse.SetOutline(null);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (unit.IsBattle) return;
        mouse.SetHand(eventData.pointerDrag);
        anim.Play("PickUp");
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (unit.IsBattle) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        plane.Raycast(ray, out float distnace);
        Vector3 pos = ray.GetPoint(distnace);
        pos.y += 0.5f;
        transform.position = pos;

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
        if (unit.IsBattle) return;
        if (targetTile == null)
        {
            transform.position = curTile.transform.position;
        }
        else
        {
            if (targetTile.type == Type.Field &&
                targetTile.unit == null &&
                curTile.type == Type.Bench &&
                player.isFullField)
            {
                transform.position = curTile.transform.position;
            }
            else
            {
                SetTile(targetTile);
            }
        }
        mouse.SetHand(null);
        anim.Play("Idle");
    }
}
