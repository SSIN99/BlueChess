using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour
{
    #region Info
    [Header("Unit Info")]
    [SerializeField] private string no;
    [SerializeField] private string unitName;
    [SerializeField] private int origin;
    [SerializeField] private int jobClass;
    [SerializeField] private int cost;
    [Header("Unit Stat")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int curHealth;
    [SerializeField] private int maxMP;
    [SerializeField] private int startMP;
    [SerializeField] private int curMP;
    [SerializeField] private int AD;
    [SerializeField] private int AP;
    [SerializeField] private int Armor;
    [SerializeField] private int Resistance;
    [SerializeField] private int AS;
    [SerializeField] private int Range;
    #endregion


    private bool isOnField;
    [SerializeField]
    private GameObject tileLine;
    private Tile curTile;
    private Tile targetTile;
    private MonoBehaviour outline;
    private BoxCollider col;
    private RaycastHit hit;
    private Ray ray;
    [SerializeField] private LayerMask layer;

    private void OnEnable()
    {
        tileLine = GameObject.FindGameObjectWithTag("Map").transform.GetChild(1).gameObject;
        col = GetComponent<BoxCollider>();
        outline = GetComponent<Outline>();
    }
    public void InitData(Dictionary<string, string> data)
    {
        no = data["No"];
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

        isOnField = false;
    }
    public void SetTile(Tile tile)
    {
        if(curTile != null)
        {
            curTile.isEmpty = true;
            curTile.unit = null;
        }
        if (!tile.isEmpty)
        {
            tile.unit.SetTile(curTile);
        }
        transform.position = tile.transform.position;
        curTile = tile;
        curTile.unit = this;
        curTile.isEmpty = false;
        if (curTile.type.Equals(Type.Field))
        {
            isOnField = true;
        }
        else
        {
            isOnField = false;
        }
    }
    private void OnMouseDrag()
    {
        col.enabled = false;
        tileLine.SetActive(true);
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            targetTile = hit.transform.GetComponent<Tile>();
        }
        else
        {
            targetTile = null;
        }
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        plane.Raycast(ray, out float range);
        Vector3 pos = ray.GetPoint(range);
        pos.y += 0.5f;
        transform.position = pos;
    }
    private void OnMouseUp()
    {
        col.enabled = true;
        tileLine.SetActive(false);
        if (targetTile == null)
        {
            transform.position = curTile.pos;
        }
        else
        {
            SetTile(targetTile);
        }
    }
    private void OnMouseEnter()
    {
        if (!outline.enabled)
            outline.enabled = true;
    }
    private void OnMouseExit()
    {
        if (outline.enabled)
            outline.enabled = false;
    }
}
