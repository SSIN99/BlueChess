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

    [SerializeField]
    private bool isOnField;
    private Tile curTile;
    private Tile targetTile;
    private RaycastHit hit;
    private Ray ray;
    [SerializeField] private LayerMask layer;
    
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
        transform.position = tile.pos;
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
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            targetTile = hit.transform.GetComponent<Tile>();
            transform.position = targetTile.pos;
        }
        else
        {
            targetTile = null;
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            plane.Raycast(ray, out float range);
            transform.position = ray.GetPoint(range);
        }
    }
    private void OnMouseUp()
    {
        if (targetTile == null)
        {
            transform.position = curTile.pos;
        }
        else
        {
            SetTile(targetTile);
        }
    }
}
