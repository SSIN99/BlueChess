using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Unit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public class UnitInfo
    {
        public int no;
        public string unitName;
        public int origin;
        public int jobClass;
        public int cost;
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
        public void SetInfo(Dictionary<string, string> data)
        {
            no = int.Parse(data["No"]);
            unitName = data["Name"];
            origin = int.Parse(data["Origin"]);
            jobClass = int.Parse(data["Class"]);
            cost = int.Parse(data["Cost"]);
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
    }

    public UnitInfo info = new UnitInfo();
    private Tile curTile;
    private Tile targetTile;
    private MonoBehaviour outline;
    private DragHandler dragHandler;
    private UnitManager unitManager;

    public bool isOnField = false;

    public void OnEnable()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        dragHandler = player.GetComponent<DragHandler>();
        unitManager = player.GetComponent<UnitManager>();
        outline = GetComponent<Outline>();
    }
    public void InitData(Dictionary<string, string> data)
    {
        info.SetInfo(data);
    }
    public void SetTile(Tile tagetTile)
    {
        if(curTile != null)
        {
            curTile.isEmpty = true;
            curTile.unit = null;
        }
        curTile = tagetTile;
        curTile.unit = this;
        curTile.isEmpty = false;
        transform.position = curTile.transform.position;
        if (curTile.type.Equals(Type.Field))
        {
            isOnField = true;
        }
        else
        {
            isOnField = false;
        }
        unitManager.CheckUnitList();
    }
    public void ShiftTile(Tile targetTile)
    {
        targetTile.unit.SetTile(curTile);
        curTile = targetTile;
        curTile.unit = this;
        curTile.isEmpty = false;
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
    public void BeSold()
    {
        curTile.isEmpty = true;
        curTile.unit = null;
        curTile = null;
        unitManager.CheckUnitList();
        dragHandler.EndDrag();
    }

    #region MouseEvent
    public void OnPointerExit(PointerEventData eventData)
    {
        if (outline.enabled)
            outline.enabled = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!outline.enabled)
            outline.enabled = true;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragHandler.StartDrag(info.cost);
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
            transform.position = curTile.pos;
        }
        else
        {
            if (!targetTile.isEmpty)
            {
                ShiftTile(targetTile);
            }
            else
            {
                if (unitManager.numOfField.Equals(unitManager.maxNumOfField))
                {
                    transform.position = curTile.pos;
                    Debug.Log("��á��");
                }
                else
                {
                    SetTile(targetTile);
                }
            }
        }
        dragHandler.EndDrag();
    }
    #endregion


}
