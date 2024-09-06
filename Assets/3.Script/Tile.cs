using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum Type {
        Waiting,
        Field
    }

    public Type type;
    public Vector3 pos;
    public Vector3 offset;
    public bool isEmpty;

    private void Start()
    {
        pos = transform.position + offset;
        isEmpty = true;
    }
}
