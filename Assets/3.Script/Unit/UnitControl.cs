using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class UnitControl : MonoBehaviour
{
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
    public float AS;
    public int Range;

    public float radius;
    public GameObject enemy;

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
        AS = float.Parse(data["AS"]);
        Range = int.Parse(data["Range"]);

    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
