using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoUIHandler : MonoBehaviour
{
    [SerializeField] Transform highlight;
    private GameObject target;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            highlight.parent = transform;
            target = null;
            gameObject.SetActive(false);
        }
    }
    public void SetUnitInfo(GameObject unit)
    {
        target = unit;
        highlight.position = target.transform.position;
        highlight.parent = target.transform;

    }
}
