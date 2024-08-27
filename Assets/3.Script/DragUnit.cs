using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragUnit : MonoBehaviour
{

    private bool isDrag;
    private Camera mainCamera;
    private Vector3 startPos;

    private RaycastHit unit;
    private RaycastHit tile;
    [SerializeField] private LayerMask layer_unit;
    [SerializeField] private LayerMask layer_tile;

    private Plane ground;

    private void Start()
    {
        isDrag = false;
        mainCamera = Camera.main;
        ground = new Plane(Vector3.up, Vector3.zero);
    }


    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!isDrag)
        {
            if (Physics.Raycast(ray, out unit, 1f, layer_unit))
            {
                if (Input.GetMouseButton(0))
                {
                    startPos = unit.transform.position;
                    isDrag = true;
                }
            }
        }
        else
        {
            ground.Raycast(ray, out float dis);
            unit.transform.position = ray.GetPoint(dis) + Vector3.up * 0.2f;

            if (Input.GetMouseButtonUp(0))
            {
                isDrag = false;
                if (Physics.Raycast(ray, out tile, 1f, layer_tile))
                {
                    //이미 유닛이 존재하는 타일 막아야함 
                    unit.transform.position = new Vector3(tile.transform.position.x, 0f, tile.transform.position.z);
                }
                else
                {
                    unit.transform.position = startPos;
                }
            }
        }
    }
}
