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
    public Arrangement unit;
    private Renderer rend;
    private Color defaultColor;


    private void Start()
    {
        rend = GetComponent<Renderer>();
        defaultColor = rend.material.color;
    }
    private void OnDisable()
    {
        rend.material.color = defaultColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rend.material.color = new Color(0, 1f, 0.2f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        rend.material.color = defaultColor;
    }

}
