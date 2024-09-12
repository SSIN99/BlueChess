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
    public bool isOnField;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        this.player = player.GetComponent<Player>();
        dragHandler = player.GetComponent<DragHandler>();
        outline = GetComponent<Outline>();
    }
    public void BeSold()
    {
        curTile.unit = null;
        curTile = null;
        dragHandler.SetHand(null);
        player.CheckUnitList();
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
        curTile = target;
        curTile.unit = this;
        transform.position = curTile.transform.position;
        if (curTile.type.Equals(Type.Field))
        {
            isOnField = true;
        }
        else
        {
            isOnField = false;
        }
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
            if (targetTile.type.Equals(Type.Field) &&
                targetTile.unit == null &&
                curTile.type.Equals(Type.Bench) &&
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
        player.CheckUnitList();
    }
}
