using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class Grid
{
    
    int height;
    int width;
    Node[,] gridMatrix;//[col.row]

    

    public Grid()
    {
        this.width = 10;
        this.height = 10;
        this.gridMatrix = new Node[ width, height];       
    }
    public Grid(int width, int height)
    {
        this.width = width;
        this.height = height;
        this.gridMatrix = new Node[width, height];
    }

    public void InitializeGrid()
    {
        for (int r = 0; r < height; r++)
        {
            for (int c = 0; c < width; c++)
            {
                Node node = new Node();
                gridMatrix[c, r] = node;
                node.pos = new Vector2Int( c,r);
            }
        }
    }

    public void SetStartNode(int c, int r)
    {
        
        gridMatrix[c,r].SetStart();
    }

    public void SetDestinationNode(int c, int r)
    {
        gridMatrix[c,r].SetDestination();
    }

   

    #region path finding

    bool IsDestination(int r, int c)
    {
        return gridMatrix[c,r].isDest;
    }

    List<Node> GetPath(Dictionary<Node, Node> came_from, Node start, Node end)
    {
        List<Node> path = new List<Node>();
       // path.Add(end);
        Node next = came_from[end];
        
        while (next != start)
        {
            path.Add(next);
            next = came_from[next];
        }
       // path.Add(start);
        path.Reverse(); // from start to end
        return path;
    }

    public List<Node> BFS(int r, int c)
    {
        Queue<Node> q = new Queue<Node>();
        Dictionary<Node,Node> came_from = new Dictionary<Node, Node>();

        Node start = gridMatrix[c,r];
        

        q.Enqueue(start);
        int dist = 0;
        while (q.Count > 0)
        {
            
            Node node = q.Dequeue();
            Debug.Log("inside grid node:" + node.pos);
            r = node.pos.y;
            c = node.pos.x;

            if (node.isDest)
            {
                return GetPath(came_from, start, node);
                
                //early exit
            }
            //push 4 direction on to the queue
            //up
            
            dist++;
            if (r - 1 >= 0 && !came_from.ContainsKey(gridMatrix[c, r - 1]))
            {
                gridMatrix[c, r - 1].totalcost = dist;
                q.Enqueue(gridMatrix[c, r - 1]);
                came_from[gridMatrix[c, r - 1]] = node;
                
            }
            //down
            if (r + 1 < height && !came_from.ContainsKey(gridMatrix[c , r + 1]))
            {
                gridMatrix[c , r + 1].totalcost = dist;
                q.Enqueue(gridMatrix[c , r + 1]);
                came_from[gridMatrix[c , r + 1]] = node;
            }
            //left
            if (c - 1 >= 0 && !came_from.ContainsKey(gridMatrix[ c - 1, r]))
            {
                gridMatrix[ c - 1, r].totalcost = dist;
                q.Enqueue(gridMatrix[ c - 1, r]);
                came_from[gridMatrix[ c - 1, r]] = node;
            }
            //right
            if (c + 1 < width && !came_from.ContainsKey(gridMatrix[ c + 1, r]))
            {
                gridMatrix[ c + 1, r].totalcost = dist;
                q.Enqueue(gridMatrix[ c + 1, r]);
                came_from[gridMatrix[ c + 1, r]] = node;
            }
            

        }
        return new List<Node>();




    }

    //apply when node has a non unifrom cost distribution
    //Priority Queue from https://github.com/BlueRaja/High-Speed-Priority-Queue-for-C-Sharp
    public List<Node> Dijkstra(int r, int c)
    {
        //based on gcost that should be assigned with node

        SimplePriorityQueue<Node> q = new SimplePriorityQueue<Node>();
        
        // Queue<Node> q = new Queue<Node>();
        Dictionary<Node, Node> came_from = new Dictionary<Node, Node>();

        Dictionary<Node, int> cost_so_far = new Dictionary<Node, int>();


        Node start = gridMatrix[c,r];
        q.Enqueue(start,0);
        cost_so_far[start] = 0; 
        while (q.Count > 0)
        {
            Node node = q.Dequeue();
            r = node.pos.y;
            c = node.pos.x;

            if (node.isDest)
            {
                return GetPath(came_from, start, node);

                //early exit
            }

            //push 4 direction on to the queue
            //up

            if (r - 1 >= 0 )
            {
                int new_cost = cost_so_far[node] + gridMatrix[c, r - 1].gcost;
                if (!cost_so_far.ContainsKey(gridMatrix[c, r - 1]) || new_cost<cost_so_far[gridMatrix[c, r - 1]])
                {
                    
                    q.Enqueue(gridMatrix[c, r - 1],new_cost);
                    came_from[gridMatrix[c, r - 1]] = node;
                    cost_so_far[gridMatrix[c, r - 1]] = new_cost;
                }

            }
            //down
            if (r + 1 < height)
            {
                int new_cost = cost_so_far[node] + gridMatrix[c, r + 1].gcost;
                if (!cost_so_far.ContainsKey(gridMatrix[c, r + 1]) || new_cost < cost_so_far[gridMatrix[c, r + 1]])
                {
                    
                    q.Enqueue(gridMatrix[c, r + 1], new_cost);
                    came_from[gridMatrix[c, r + 1]] = node;
                    cost_so_far[gridMatrix[c, r + 1]] = new_cost;
                }
            }

         
            //left
            if (c - 1 >= 0)
            {
                int new_cost = cost_so_far[node] + gridMatrix[ c - 1 , r].gcost;
                if (!cost_so_far.ContainsKey(gridMatrix[ c - 1, r]) || new_cost < cost_so_far[gridMatrix[ c - 1 , r]])
                {
                    
                    q.Enqueue(gridMatrix[ c - 1 , r], new_cost);
                    came_from[gridMatrix[ c - 1 , r]] = node;
                    cost_so_far[gridMatrix[ c - 1 , r]] = new_cost;
                }

            }

            //right
            if (c + 1 < width)
            {
                int new_cost = cost_so_far[node] + gridMatrix[ c + 1, r].gcost;
                if (!cost_so_far.ContainsKey(gridMatrix[ c + 1, r]) || new_cost < cost_so_far[gridMatrix[ c + 1, r]])
                {
                    
                    q.Enqueue(gridMatrix[ c + 1, r], new_cost);
                    came_from[gridMatrix[ c + 1, r]] = node;
                    cost_so_far[gridMatrix[ c + 1, r]] = new_cost;
                }

            }
            

        }


        return new List<Node>();
    }

    //no diagonal move
    int heuristic(Node start, Node end)
    {
        int h = Mathf.Abs(start.pos.x - end.pos.x)+ Mathf.Abs(start.pos.y - end.pos.y);

        return h;
    }
    //greedy best first search
    public List<Node> Greedy(int r, int c, int dr, int dc)
    {
        SimplePriorityQueue<Node> q = new SimplePriorityQueue<Node>();
        // Queue<Node> q = new Queue<Node>();
        Dictionary<Node, Node> came_from = new Dictionary<Node, Node>();

        Node start = gridMatrix[ c, r];
        Node end = gridMatrix[dc, dr];
        int start_h = heuristic(start, end);
        q.Enqueue(start, start_h);
        
        while (q.Count > 0)
        {
            Node node = q.Dequeue();
            r = node.pos.y;
            c = node.pos.x;

            if (node.isDest)
            {
                return GetPath(came_from, start, node);

                //early exit
            }
            //push 4 direction on to the queue
            //up
            if (r - 1 >= 0 && !came_from.ContainsKey(gridMatrix[ c, r - 1]))
            {
                int new_h = heuristic(gridMatrix[ c, r - 1], end);
                q.Enqueue(gridMatrix[ c, r - 1], new_h);
                came_from[gridMatrix[ c, r - 1]] = node;

            }
            //down
            if (r + 1 <height && !came_from.ContainsKey(gridMatrix[c, r + 1]))
            {
                int new_h = heuristic(gridMatrix[c, r + 1], end);
                q.Enqueue(gridMatrix[c, r + 1], new_h);
                came_from[gridMatrix[c, r + 1]] = node;

            }

            //left
            if (c - 1 >= 0 && !came_from.ContainsKey(gridMatrix[ c - 1, r]))
            {
                int new_h = heuristic(gridMatrix[ c - 1, r], end);
                q.Enqueue(gridMatrix[ c - 1, r], new_h);
                came_from[gridMatrix[ c - 1, r]] = node;

            }
            

            //right
            if (c + 1 < width && !came_from.ContainsKey(gridMatrix[ c + 1, r]))
            {
                int new_h = heuristic(gridMatrix[ c + 1, r], end);
                q.Enqueue(gridMatrix[ c + 1, r], new_h);
                came_from[gridMatrix[ c + 1, r]] = node;

            }

        }


        return new List<Node>();
    }
    public List<Node> AStar(int r, int c, int dr, int dc)
    {
        SimplePriorityQueue<Node> q = new SimplePriorityQueue<Node>();
        Dictionary<Node, Node> came_from = new Dictionary<Node, Node>();
        Dictionary<Node, int> cost_so_far = new Dictionary<Node, int>();


        Node start = gridMatrix[ c, r];
        Node end = gridMatrix[dc, dr];

        int start_h = heuristic(start, end);
        start.SetHcost(start_h);
        q.Enqueue(start, start_h+0);
        cost_so_far[start] = start_h;
        while (q.Count > 0)
        {
            Node node = q.Dequeue();
            r = node.pos.y;
            c = node.pos.x;
            if (node.isDest)
            {
                return GetPath(came_from, start, node);

                //early exit
            }
            //push 4 direction on to the queue
            //up

            if (r - 1 >= 0)
            {
                int new_cost = cost_so_far[node] + gridMatrix[ c, r -1 ].gcost;
                if (!cost_so_far.ContainsKey(gridMatrix[ c, r -1 ]) || new_cost < cost_so_far[gridMatrix[ c, r -1 ]])
                {
                    int totalcost = new_cost + heuristic(gridMatrix[ c, r -1 ], end);
                    q.Enqueue(gridMatrix[ c, r -1 ], totalcost);
                    came_from[gridMatrix[ c, r -1 ]] = node;
                    cost_so_far[gridMatrix[ c, r -1 ]] = new_cost;
                }

            }
            //down
            if (r + 1 < height)
            {
                int new_cost = cost_so_far[node] + gridMatrix[ c, r + 1].gcost;
                if (!cost_so_far.ContainsKey(gridMatrix[c, r + 1]) || new_cost < cost_so_far[gridMatrix[ c, r + 1]])
                {
                    int totalcost = new_cost + heuristic(gridMatrix[ c, r + 1], end);
                    q.Enqueue(gridMatrix[ c, r + 1], totalcost);
                    came_from[gridMatrix[ c, r + 1]] = node;
                    cost_so_far[gridMatrix[ c, r + 1]] = new_cost;
                }
            }


            //left
            if (c - 1 >= 0)
            {
                int new_cost = cost_so_far[node] + gridMatrix[ c - 1, r ].gcost;
                if (!cost_so_far.ContainsKey(gridMatrix[ c - 1, r ]) || new_cost < cost_so_far[gridMatrix[ c - 1, r ]])
                {
                    int totalcost = new_cost + heuristic(gridMatrix[ c - 1, r ], end);
                    q.Enqueue(gridMatrix[ c - 1, r ], totalcost);
                    came_from[gridMatrix[ c - 1, r ]] = node;
                    cost_so_far[gridMatrix[ c - 1, r ]] = new_cost;
                }

            }

            //right
            if (c + 1 < width)
            {
                int new_cost = cost_so_far[node] + gridMatrix[ c + 1, r ].gcost;
                if (!cost_so_far.ContainsKey(gridMatrix[ c + 1, r ]) || new_cost < cost_so_far[gridMatrix[ c + 1, r ]])
                {
                    int totalcost = new_cost + heuristic(gridMatrix[ c + 1, r ], end);
                    q.Enqueue(gridMatrix[ c + 1, r ], totalcost);
                    came_from[gridMatrix[ c + 1, r ]] = node;
                    cost_so_far[gridMatrix[ c + 1, r ]] = new_cost;
                }

            }


        }


        return new List<Node>();
    }
    
    #endregion
}
