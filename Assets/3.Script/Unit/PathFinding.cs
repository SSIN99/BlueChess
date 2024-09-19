using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public List<Node> FindPath(Node start, Node end, List<Node> allNodes)
    {
        var openList = new List<Node>();
        var closedList = new HashSet<Node>();

        openList.Add(start);

        while (openList.Count > 0)
        {
            var currentNode = openList.OrderBy(n => n.costF).First();
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode.position == end.position)
            {
                return ReconstructPath(currentNode);
            }

            foreach (var neighbor in GetNeighbors(currentNode, allNodes))
            {
                if (!neighbor.isWalk || closedList.Contains(neighbor)) continue;

                var tentativeGCost = currentNode.costG + GetDistance(currentNode, neighbor);
                if (!openList.Contains(neighbor) || tentativeGCost < neighbor.costG)
                {
                    neighbor.costG = tentativeGCost;
                    neighbor.costH = GetDistance(neighbor, end);
                    neighbor.parent = currentNode;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return new List<Node>();
    }

    private float GetDistance(Node a, Node b)
    {
        return Vector3.Distance(a.position, b.position);
    }

    private List<Node> GetNeighbors(Node node, List<Node> allNodes)
    {
        
        var neighbors = new List<Node>();

        var directions = new Vector3[]
        {
            new Vector3(2, 0, 0), new Vector3(-2, 0, 0),
            new Vector3(1, 0, 1.8f), new Vector3(-1, 0, 1.8f),
            new Vector3(1, 0, -1.8f), new Vector3(-1, 0, -1.8f)
        };

        foreach (var direction in directions)
        {
            var neighborPosition = node.position + direction;
            var neighbor = allNodes.FirstOrDefault(n => n.position == neighborPosition);
            if (neighbor != null)
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    private List<Node> ReconstructPath(Node endNode)
    {
        var path = new List<Node>();
        var currentNode = endNode;

        while (currentNode != null)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }
}
