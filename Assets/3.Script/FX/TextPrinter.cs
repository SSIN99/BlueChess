using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TextType
{
    Attack,
    Crit,
    Heal,
    Avoid,
    Skill
}

public class TextPrinter : MonoBehaviour
{
    [SerializeField] private GameObject textPrefab;
    private Vector2 offset;
    
    public void PrintText(string value, Vector3 target, TextType type)
    {
        GameObject temp = Instantiate(textPrefab, transform);

        offset = Random.insideUnitCircle;
        Vector2 pos = Camera.main.WorldToScreenPoint(target);
        pos.y += 50f;
        pos.x += offset.x * 15f;
        pos.y += offset.y * 15f;

        temp.GetComponent<TextUI>().InitText(value, pos, type);
    }
}
