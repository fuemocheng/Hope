using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable
{

    public float nodeTotalCost;
    public float estimatedCost;
    public bool bObstacle;
    public Node parent;
    public Vector3 position;


    public Node()
    {
        this.estimatedCost = 0f;
        this.nodeTotalCost = 1f;
        this.bObstacle = false;
        this.parent = null;
    }

    public Node(Vector3 vector3)
    {
        this.estimatedCost = 0f;
        this.nodeTotalCost = 1f;
        this.bObstacle = false;
        this.parent = null;
        this.position = vector3;
    }

    public int CompareTo(object obj)
    {
        return 1;
    }

   
}
