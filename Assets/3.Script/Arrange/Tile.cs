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
    private Renderer rend;
    private Color basicColor;
    public ArrangeControl unit;
    public Type type;


    private void Start()
    {
        rend = GetComponent<Renderer>();
        basicColor = rend.material.color;
    }
    private void OnDisable()
    {
        rend.material.color = basicColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rend.material.color = new Color(0, 1f, 0.2f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        rend.material.color = basicColor;
    }

}
