using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    
    [SerializeField] private int level = 1;
    [SerializeField] private int gold = 5;

    private List<UnitControl> unitList;

    public int Level
    {
        get
        {
            return level;
        }
        private set
        {
            level = value;
        }
    }

    private void Start()
    {
        unitList = new List<UnitControl>();
    }
}
