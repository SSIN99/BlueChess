using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Unit Info")]
    [SerializeField] private string name;
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
    
    
    public void InitData(Dictionary<string, string> data)
    {
        name = data["Name"];
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
