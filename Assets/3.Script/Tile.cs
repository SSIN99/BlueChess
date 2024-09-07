using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    Bench,
    Field
}

public class Tile : MonoBehaviour
{
    public Type type;
    public Vector3 pos;
    public bool isEmpty;
    public Unit unit;

    private void Start()
    {
        pos = transform.position;
        pos.y += 0.2f;
        isEmpty = true;
    }
}
