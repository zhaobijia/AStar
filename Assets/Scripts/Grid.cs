using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<T>
{
    int width;
    int height;
    T[,] gridMatrix;//[row,col]

    //for a * path
    int hCost;//HEURISTIC FUNCTION 
    int gCost;
    int totalCost;

    public Grid()
    {
        this.width = 10;
        this.height = 10;
        this.gridMatrix = new T[width, height];       
    }
    public Grid(int width, int height)
    {
        this.width = width;
        this.height = 10;
        this.gridMatrix = new T[width, height];
    }

    public void InitializeGrid(T t)
    {
        for (int r = 0; r < height; r++)
        {
            for (int c = 0; c < width; c++)
            {
                gridMatrix[r, c] = t;
            }
        }
    }

    public void SetHCost(int hcost)
    {
        this.hCost = hcost;
    }
    public void SetGCost(int gcost)
    {
        this.gCost = gcost;
    }

    public int CalculateTotalCost()
    {
        totalCost = gCost + hCost;
        return totalCost;
    }
}
