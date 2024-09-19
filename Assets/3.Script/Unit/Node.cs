using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector3 position;
    public bool isWalk;
    public Node parent;
    public float costG;
    public float costH;
    public float costF => costG + costH;

    private void Start()
    {
        position = transform.position;
        isWalk = true;
    }
}
