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
    public UnitArrange unit;
    private Renderer rend;
    private Color basicColor;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        basicColor = rend.material.color;
    }

    private void OnDisable()
    {
        rend.material.color = basicColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rend.material.color = basicColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rend.material.color = new Color(0, 0.9f, 0.3f);
    }
}
