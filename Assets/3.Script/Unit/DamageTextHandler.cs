using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextHandler : MonoBehaviour
{
    [SerializeField] private GameObject damageText;
    private Queue<GameObject> damageList;
    private Vector2 offset;

    private void Start()
    {
        damageList = new Queue<GameObject>();

        for(int i = 0; i < 50; i++)
        {
            GameObject temp = Instantiate(damageText, transform);
            temp.SetActive(false);
            damageList.Enqueue(temp);
        }
    }
    public void PrintDamage(float damage, GameObject target)
    {
        GameObject temp = damageList.Dequeue();
        temp.SetActive(true);
        damageList.Enqueue(temp);

        offset = Random.insideUnitCircle;
        Vector2 pos = Camera.main.WorldToScreenPoint(target.transform.position);
        pos.y += 50f;
        pos.x += offset.x * 10f;
        pos.y += offset.y * 10f;

        temp.GetComponent<DamageText>().InitDamage(damage, pos);
    }
}
