using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitArrange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Tile curTile;
    [SerializeField] private Tile targetTile;
    private Player player;
    private DragHandler dragHandler;
    private MonoBehaviour outline;
    private Animator anim;
    public bool isOnField;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        this.player = player.GetComponent<Player>();
        dragHandler = player.GetComponent<DragHandler>();
        outline = GetComponent<Outline>();
        anim = GetComponent<Animator>();
    }
    public void BeSold()
    {
        curTile.unit = null;
        curTile = null;
        dragHandler.SetHand(null);
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
                isOnField = true;
                player.RemoveBench(gameObject);
                player.AddField(gameObject);
            }
            else
            {
                isOnField = false;
                player.RemoveField(gameObject);
                player.AddBench(gameObject);
            }
        }
        curTile = target;
        curTile.unit = this;
        transform.position = curTile.transform.position;
    }
    public void ReturnTile()
    {
        transform.position = curTile.transform.position;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        outline.enabled = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        outline.enabled = false;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragHandler.SetHand(eventData.pointerDrag);
        anim.Play("PickUp");
    }
    public void OnDrag(PointerEventData eventData)
    {
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
        dragHandler.SetHand(null);
        anim.Play("Idle");
    }
}
