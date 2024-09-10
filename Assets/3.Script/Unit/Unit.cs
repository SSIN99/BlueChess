using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region 유닛정보
    public int No;
    public string Name;
    public int Origin;
    public int Class;
    public int Cost;
    public int maxHealth;
    public int curHealth;
    public int maxMP;
    public int startMP;
    public int curMP;
    public int AD;
    public int AP;
    public int Armor;
    public int Resistance;
    public int AS;
    public int Range;
    public bool isOnField;
    #endregion

    #region 유닛배치
    private Tile curTile;
    private Tile targetTile;
    private UnitManager unitManager;
    private DragHandler dragHandler;
    private MonoBehaviour outline;
    #endregion

    #region 유닛정보메소드
    public void InitInfo(Dictionary<string, string> data)
    {
        No = int.Parse(data["No"]);
        Name = data["Name"];
        Origin = int.Parse(data["Origin"]);
        Class = int.Parse(data["Class"]);
        Cost = int.Parse(data["Cost"]);
        maxHealth = int.Parse(data["Health"]);
        curHealth = maxHealth;
        maxMP = int.Parse(data["MaxMP"]);
        startMP = int.Parse(data["StartMP"]);
        curMP = startMP;
        AD = int.Parse(data["AD"]);
        AP = int.Parse(data["AP"]);
        Armor = int.Parse(data["Armor"]);
        Resistance = int.Parse(data["Resistance"]);
        AS = int.Parse(data["AS"]);
        Range = int.Parse(data["Range"]);
    }
    #endregion

    #region 유닛배치메소드
    public void BeSold()
    {
        curTile.unit = null;
        curTile = null;
        dragHandler.SetHand(null);
    }
    public void SetTile(Tile target)
    {
        if(curTile != null)
        {
            curTile.unit = null;
        }
        if(target.unit != null)
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
    public void OnPointerExit(PointerEventData eventData)
    {
        outline.enabled = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        outline.enabled = true;
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
        if(eventData.pointerEnter == null)
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
            if(targetTile.type.Equals(Type.Field) &&
                targetTile.unit == null &&
                curTile.type.Equals(Type.Bench) &&
                unitManager.isFullField)
            {
                transform.position = curTile.transform.position;
            }
            else
            {
                SetTile(targetTile);
            }
        }
        dragHandler.SetHand(null);
    }
    #endregion




    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        dragHandler = player.GetComponent<DragHandler>();
        unitManager = player.GetComponent<UnitManager>();
        outline = GetComponent<Outline>();
    }

}
