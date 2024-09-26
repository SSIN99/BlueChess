using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPrinter : MonoBehaviour
{
    [SerializeField] private GameObject hitPrefab;
    private Queue<GameObject> hit;
    private Vector3 offset;

    private void Start()
    {
        hit = new Queue<GameObject>();
        for (int i = 0; i < 50; i++)
        {
            GameObject h = Instantiate(hitPrefab, transform);
            h.SetActive(false);
            hit.Enqueue(h);
        }
    }
    public void PrintSFXHit(Vector3 target)
    {
        GameObject temp = hit.Dequeue();

        offset = Random.insideUnitCircle;
        offset *= 0.5f;
        offset.y += 1f;
        offset.z -= 0.1f;
        temp.transform.position = target + offset;
        temp.transform.forward = Camera.main.transform.forward;
        
        temp.SetActive(true);
        hit.Enqueue(temp);
    }
}
