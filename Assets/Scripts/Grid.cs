using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class Grid
{
    
    int height;
    int width;
    Node[,] gridMatrix;//[row,col]

    

    public Grid()
    {
        this.width = 10;
        this.height = 10;
        this.gridMatrix = new Node[height, width];       
    }
    public Grid(int width, int height)
    {
        this.width = width;
        this.height = 10;
        this.gridMatrix = new Node[height, width];
    }

    public void InitializeGrid()
    {
        for (int r = 0; r < height; r++)
        {
            for (int c = 0; c < width; c++)
            {
                Node node = new Node();
                gridMatrix[r, c] = node;
                node.pos = new Vector2Int(r, c);
            }
        }
    }

   

    #region path finding

    bool IsDestination(int r, int c)
    {
        return gridMatrix[r, c].isDest;
    }

    List<Node> GetPath(Dictionary<Node, Node> came_from, Node start, Node end)
    {
        List<Node> path = new List<Node>();
        path.Add(end);
        Node next = came_from[end];
        
        while (next != start)
        {
            path.Add(next);
            next = came_from[next];
        }
        path.Add(start);
        path.Reverse(); // from start to end
        return path;
    }

    public void BFS(int r, int c)
    {
        Queue<Node> q = new Queue<Node>();
        Dictionary<Node,Node> came_from = new Dictionary<Node, Node>();

        Node start = gridMatrix[r, c];
        q.Enqueue(start);
        int dist = 0;
        while (q.Count > 0)
        {
            Node node = q.Dequeue();
            if (node.isDest) break; //early exit
            //push 4 direction on to the queue
            //up
            dist++;
            if (r - 1 > 0 && !came_from.ContainsKey(gridMatrix[r - 1, c]))
            {
                gridMatrix[r - 1, c].totalcost = dist;
                q.Enqueue(gridMatrix[r - 1, c]);
                came_from[gridMatrix[r - 1, c]] = node;
                
            }
            //down
            if (r + 1 < height && !came_from.ContainsKey(gridMatrix[r + 1, c]))
            {
                gridMatrix[r + 1, c].totalcost = dist;
                q.Enqueue(gridMatrix[r + 1, c]);
                came_from[gridMatrix[r + 1, c]] = node;
            }
            //left
            if (c - 1 > 0 && !came_from.ContainsKey(gridMatrix[r, c - 1]))
            {
                gridMatrix[r, c - 1].totalcost = dist;
                q.Enqueue(gridMatrix[r, c - 1]);
                came_from[gridMatrix[r, c - 1]] = node;
            }
            //right
            if (c + 1 < width && !came_from.ContainsKey(gridMatrix[r, c - 1]))
            {
                gridMatrix[r, c + 1].totalcost = dist;
                q.Enqueue(gridMatrix[r, c + 1]);
                came_from[gridMatrix[r, c + 1]] = node;
            }

        }

    }

    //apply when node has a non unifrom cost distribution
    //Priority Queue from https://github.com/BlueRaja/High-Speed-Priority-Queue-for-C-Sharp
    public void Dijkstra(int r, int c)
    {
        //based on gcost that should be assigned with node

        SimplePriorityQueue<Node> q = new SimplePriorityQueue<Node>();
       // Queue<Node> q = new Queue<Node>();
        Dictionary<Node, Node> came_from = new Dictionary<Node, Node>();
        Dictionary<Node, int> cost_so_far = new Dictionary<Node, int>();


        Node start = gridMatrix[r, c];
        q.Enqueue(start,0);
       
        while (q.Count > 0)
        {
            Node node = q.Dequeue();
            if (node.isDest) break; //early exit
            //push 4 direction on to the queue
            //up
            
            if (r - 1 > 0 )
            {
                int new_cost = cost_so_far[node] + gridMatrix[r - 1, c].gcost;
                if (!cost_so_far.ContainsKey(gridMatrix[r - 1, c]) || new_cost<cost_so_far[gridMatrix[r - 1, c]])
                {
                    
                    q.Enqueue(gridMatrix[r - 1, c],new_cost);
                    came_from[gridMatrix[r - 1, c]] = node;
                    cost_so_far[gridMatrix[r - 1, c]] = new_cost;
                }

            }
            //down
            if (r + 1 < height)
            {
                int new_cost = cost_so_far[node] + gridMatrix[r + 1, c].gcost;
                if (!cost_so_far.ContainsKey(gridMatrix[r + -1, c]) || new_cost < cost_so_far[gridMatrix[r + 1, c]])
                {
                    
                    q.Enqueue(gridMatrix[r + 1, c], new_cost);
                    came_from[gridMatrix[r + 1, c]] = node;
                    cost_so_far[gridMatrix[r + 1, c]] = new_cost;
                }
            }

         
            //left
            if (c - 1 > 0)
            {
                int new_cost = cost_so_far[node] + gridMatrix[r, c - 1].gcost;
                if (!cost_so_far.ContainsKey(gridMatrix[r, c - 1]) || new_cost < cost_so_far[gridMatrix[r, c - 1]])
                {
                    
                    q.Enqueue(gridMatrix[r, c - 1], new_cost);
                    came_from[gridMatrix[r, c - 1]] = node;
                    cost_so_far[gridMatrix[r, c - 1]] = new_cost;
                }

            }

            //right
            if (c + 1 < width)
            {
                int new_cost = cost_so_far[node] + gridMatrix[r, c + 1].gcost;
                if (!cost_so_far.ContainsKey(gridMatrix[r, c - 1]) || new_cost < cost_so_far[gridMatrix[r, c + 1]])
                {
                    
                    q.Enqueue(gridMatrix[r, c + 1], new_cost);
                    came_from[gridMatrix[r, c + 1]] = node;
                    cost_so_far[gridMatrix[r, c + 1]] = new_cost;
                }

            }
            

        }
    }

    //no diagonal move
    int heuristic(Node start, Node end)
    {
        int h = Mathf.Abs(start.pos.x - end.pos.x)+ Mathf.Abs(start.pos.y - end.pos.y);

        return h;
    }
    //greedy best first search
    public void Greedy(int r, int c, Node end)
    {
        SimplePriorityQueue<Node> q = new SimplePriorityQueue<Node>();
        // Queue<Node> q = new Queue<Node>();
        Dictionary<Node, Node> came_from = new Dictionary<Node, Node>();

        Node start = gridMatrix[r, c];
        int start_h = heuristic(start, end);
        q.Enqueue(start, start_h);

        while (q.Count > 0)
        {
            Node node = q.Dequeue();
            if (node.isDest) break; //early exit
                                    //push 4 direction on to the queue
                                    //up
            if (r - 1 > 0 && !came_from.ContainsKey(gridMatrix[r - 1, c]))
            {
                int new_h = heuristic(gridMatrix[r - 1, c], end);
                q.Enqueue(gridMatrix[r - 1, c], new_h);
                came_from[gridMatrix[r - 1, c]] = node;

            }
            //down
            if (r + 1 <height && !came_from.ContainsKey(gridMatrix[r + 1, c]))
            {
                int new_h = heuristic(gridMatrix[r + 1, c], end);
                q.Enqueue(gridMatrix[r + 1, c], new_h);
                came_from[gridMatrix[r + 1, c]] = node;

            }

            //left
            if (c - 1 > 0 && !came_from.ContainsKey(gridMatrix[r, c - 1]))
            {
                int new_h = heuristic(gridMatrix[r, c - 1], end);
                q.Enqueue(gridMatrix[r, c - 1], new_h);
                came_from[gridMatrix[r, c - 1]] = node;

            }
            

            //right
            if (c + 1 < width && !came_from.ContainsKey(gridMatrix[r, c + 1]))
            {
                int new_h = heuristic(gridMatrix[r, c + 1], end);
                q.Enqueue(gridMatrix[r, c + 1], new_h);
                came_from[gridMatrix[r, c + 1]] = node;

            }

        }
    }
    public void AStar(int r, int c, Node end)
    {
        SimplePriorityQueue<Node> q = new SimplePriorityQueue<Node>();
        Dictionary<Node, Node> came_from = new Dictionary<Node, Node>();
        Dictionary<Node, int> cost_so_far = new Dictionary<Node, int>();


        Node start = gridMatrix[r, c];
        int start_h = heuristic(start, end);
        start.SetHcost(start_h);
        q.Enqueue(start, start_h+0);

        while (q.Count > 0)
        {
            Node node = q.Dequeue();
            if (node.isDest) break; //early exit
                                    //push 4 direction on to the queue
                                    //up

            if (r - 1 > 0)
            {
                int new_cost = cost_so_far[node] + gridMatrix[r - 1, c].gcost;
                if (!cost_so_far.ContainsKey(gridMatrix[r - 1, c]) || new_cost < cost_so_far[gridMatrix[r - 1, c]])
                {
                    int totalcost = new_cost + heuristic(gridMatrix[r - 1, c], end);
                    q.Enqueue(gridMatrix[r - 1, c], totalcost);
                    came_from[gridMatrix[r - 1, c]] = node;
                    cost_so_far[gridMatrix[r - 1, c]] = new_cost;
                }

            }
            //down
            if (r + 1 < height)
            {
                int new_cost = cost_so_far[node] + gridMatrix[r + 1, c].gcost;
                if (!cost_so_far.ContainsKey(gridMatrix[r + -1, c]) || new_cost < cost_so_far[gridMatrix[r + 1, c]])
                {
                    int totalcost = new_cost + heuristic(gridMatrix[r + 1, c], end);
                    q.Enqueue(gridMatrix[r + 1, c], totalcost);
                    came_from[gridMatrix[r + 1, c]] = node;
                    cost_so_far[gridMatrix[r + 1, c]] = new_cost;
                }
            }


            //left
            if (c - 1 > 0)
            {
                int new_cost = cost_so_far[node] + gridMatrix[r, c - 1].gcost;
                if (!cost_so_far.ContainsKey(gridMatrix[r, c - 1]) || new_cost < cost_so_far[gridMatrix[r, c - 1]])
                {
                    int totalcost = new_cost + heuristic(gridMatrix[r, c - 1], end);
                    q.Enqueue(gridMatrix[r, c - 1], totalcost);
                    came_from[gridMatrix[r, c - 1]] = node;
                    cost_so_far[gridMatrix[r, c - 1]] = new_cost;
                }

            }

            //right
            if (c + 1 < width)
            {
                int new_cost = cost_so_far[node] + gridMatrix[r, c + 1].gcost;
                if (!cost_so_far.ContainsKey(gridMatrix[r, c - 1]) || new_cost < cost_so_far[gridMatrix[r, c + 1]])
                {
                    int totalcost = new_cost + heuristic(gridMatrix[r, c + 1], end);
                    q.Enqueue(gridMatrix[r, c + 1], totalcost);
                    came_from[gridMatrix[r, c + 1]] = node;
                    cost_so_far[gridMatrix[r, c + 1]] = new_cost;
                }

            }


        }
    }
    
    #endregion
}
