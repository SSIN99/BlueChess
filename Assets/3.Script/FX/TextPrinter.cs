using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TextType
{
    Attack,
    Crit,
    Heal,
    Avoid
}

public class TextPrinter : MonoBehaviour
{
    [SerializeField] private GameObject textPrefab;
    private Queue<GameObject> textPool;
    private Vector2 offset;

    private void Start()
    {
        textPool = new Queue<GameObject>();
        for(int i = 0; i < 50; i++)
        {
            GameObject t = Instantiate(textPrefab, transform);
            t.SetActive(false);
            textPool.Enqueue(t);
        }
    }
    public void PrintText(string value, Vector3 target, TextType type)
    {
        GameObject temp = textPool.Dequeue();
        temp.SetActive(true);
        textPool.Enqueue(temp);

        offset = Random.insideUnitCircle;
        Vector2 pos = Camera.main.WorldToScreenPoint(target);
        pos.y += 50f;
        pos.x += offset.x * 15f;
        pos.y += offset.y * 15f;

        temp.GetComponent<TextUI>().InitText(value, pos, type);
    }
}
