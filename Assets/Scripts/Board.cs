using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;

    [SerializeField] BoardTile tilePrefab;
    [SerializeField] Material startMat;
    [SerializeField] Material destMat;
    [SerializeField] Material pathMat;



    Grid grid;

    BoardTile[,] board;

    float offset_row;
    float offset_col;

    Vector2Int start;
    Vector2Int dest;

    private void Awake()
    {
        offset_row = height / 2f - 0.5f;
        offset_col = width / 2f - 0.5f;
        InitializeBoard();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector2Int tilePos = GetTile(ray);
            if (tilePos.x >= 0 && tilePos.y >= 0)
            {
                SetStartingTile(tilePos.x, tilePos.y);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector2Int tilePos = GetTile(ray);
            if (tilePos.x >= 0 && tilePos.y >= 0)
            {
                SetDestinationTile(tilePos.x, tilePos.y);
            }
        }
    }
    //make sure board is at least 2x2
    private void OnValidate()
    {
        if (width < 2) width = 2;
        if (height < 2) height = 2;
    }



    void InitializeBoard()
    {
        grid = new Grid(width,height);
        board = new BoardTile[width, height];
        grid.InitializeGrid();
        for(int row = 0; row< height; row++)
        {
            for(int col = 0; col<width; col++)
            {
                BoardTile t = Instantiate(tilePrefab, this.transform);
                t.transform.position = new Vector2(col-offset_col, row - offset_row);
                t.Index = new Vector2Int(col, row);
                board[col, row] = t;
                
            }
        }

    }

    public void BFS()
    {
        int r = start.y;
        int c = start.x;
        List<Node> nodes = grid.BFS(r, c);

        foreach(Node n in nodes)
        {
            SetPathTile(n.pos.x, n.pos.y);
        }
    }

    public void Dijkstra()
    {
        int r = start.y;
        int c = start.x;
        List<Node> nodes = grid.Dijkstra(r, c);

        foreach (Node n in nodes)
        {
            SetPathTile(n.pos.x, n.pos.y);
        }
    }

    public void Greedy()
    {
        int r = start.y;
        int c = start.x;

        int dr = dest.y;
        int dc = dest.x;

        List<Node> nodes = grid.Greedy(r, c, dr, dc);

        foreach (Node n in nodes)
        {
            SetPathTile(n.pos.x, n.pos.y);
        }
    }

    public void AStar()
    {
        int r = start.y;
        int c = start.x;

        int dr = dest.y;
        int dc = dest.x;
        List<Node> nodes = grid.AStar(r, c, dr, dc);

        foreach (Node n in nodes)
        {
            SetPathTile(n.pos.x, n.pos.y);
        }
    }

    Vector2Int GetTile(Ray ray)
    {
        if(Physics.Raycast(ray,out RaycastHit hit))
        {
            int x = (int)(hit.point.x + offset_col + 0.5f);
            int y = (int)(hit.point.y + offset_row + 0.5f);
            
            return new Vector2Int(x, y);
        }

        return new Vector2Int(-1, -1);
        
    }

    void SetStartingTile(int x, int y) 
    {
        
        grid.SetStartNode(x, y);
        board[x, y].SetColor(startMat);
        start = new Vector2Int(x, y);
    }
    void SetDestinationTile(int x, int y)
    {
        grid.SetDestinationNode(x, y);
        board[x, y].SetColor(destMat);
        dest = new Vector2Int(x, y); ;
    }

    void SetPathTile(int x, int y)
    {
        board[x, y].SetColor(pathMat);
    }

    void UpdateGrid()
    {

    }
}
