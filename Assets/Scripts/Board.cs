using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;

    [SerializeField] BoardTile tile;
    [SerializeField] Color tileColor;

    Grid grid;
    //make sure board is at least 2x2
    private void OnValidate()
    {
        if (width < 2) width = 2;
        if (height < 2) height = 2;
    }

    void InitializeGrid()
    {
        this.grid = new Grid(width, height);

    }

    void InitializeBoard()
    {
        InitializeGrid();

    }

    void SetStartingTile() 
    {

    }
    void SetDestination()
    {

    }
}
