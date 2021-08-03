using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int value;
    public Vector2Int pos;
    
    //for a star
    public int gcost;
    public int hcost;
    public int totalcost;

    public bool isStart;
    public bool isDest;

    public Node()
    {
        this.value = 0;

        this.gcost = 0;
        this.hcost = 0;

        this.isStart = false;
        this.isDest = false;
    }

    public Node(int gcost)
    {
        this.value = 0;

        this.gcost = gcost;
        this.isStart = false;
        this.isDest = false;
    }

   

    public Node(int value, int gcost)
    {
        this.value = value;

        this.gcost = gcost;

        this.isStart = false;
        this.isDest = false;
    }

    public void SetHcost(int hcost)
    {
        this.hcost = hcost;
        
    }

    public void SetGcost(int gcost)
    {
        this.gcost = gcost;
    }

    public void CalculateTotalCost()
    {
        this.totalcost = this.hcost + this.gcost;
    }

    public void SetStartNode()
    {
        this.isStart = true;
        this.isDest = false;
    }

    public void SetDestinationNode()
    {
        this.isStart = false;
        this.isDest = true;
    }

    public void SetNormalNode()
    {
        this.isStart = false;
        this.isDest = false;
    }
}
