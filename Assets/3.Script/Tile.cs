using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public enum Type
{
    Bench,
    Field
}

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Type type;
    public Vector3 pos;
    public bool isEmpty = true;
    public Unit unit;

    private Renderer rend;
    private Color defaultColor;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        defaultColor = rend.material.color;
        pos = transform.position;
    }

    private void OnDisable()
    {
        rend.material.color = defaultColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rend.material.color = defaultColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rend.material.color = new Color(0, 0.9f, 0.3f);
    }
}
