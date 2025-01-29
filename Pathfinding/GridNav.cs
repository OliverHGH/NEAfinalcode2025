using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNav : MonoBehaviour
{
    public LayerMask unwalkablemask;
    public Vector2 worldsize;
    public float noderadius;

    float nodediameter;
    int gridx, gridy;
    public Transform player;
    List<PathfindingNode> weightedlist = new List<PathfindingNode>();

    void Awake()
    {
        nodediameter = 2 * noderadius;
        gridx = Mathf.RoundToInt(worldsize.x / nodediameter);
        gridy = Mathf.RoundToInt(worldsize.y / nodediameter);

    }
    public PathfindingNode[,] creategrid()//returns a grid of pathfinding nodes to be used in A* algorithm
    {
        gridx = Mathf.RoundToInt(worldsize.x / nodediameter);
        gridy = Mathf.RoundToInt(worldsize.y / nodediameter);
        PathfindingNode[,] grid = new PathfindingNode[gridx, gridy];//makes a grid
        Vector3 bottomleftnode = transform.position - Vector3.right * worldsize.x / 2- Vector3.forward*worldsize.y/2;
        for(int x = 0; x < gridx; x++)
        {
            for(int y = 0;y < gridy; y++)
            {
                Vector3 worldpos = bottomleftnode + Vector3.right * (x * nodediameter + noderadius) + Vector3.forward * (y * nodediameter + noderadius);
                //finds the position of the node
                bool walkable =!(Physics.CheckSphere(worldpos, noderadius,unwalkablemask));//checks if the positon represented by node is blocked
                grid[x, y] = new PathfindingNode(walkable, worldpos,x,y);//fills in grid with constructed nodes
            }    
        }
        for(int x = 0;x < gridx; x++)
        {
           for(int y = 0; y < gridy; y++)
            {
                grid[x, y].neighbours = neighbourFind(grid[x,y],grid);// assigns the nodes their neigbours
            }
        }
        return grid;
        
    }
    public List<PathfindingNode> neighbourFind(PathfindingNode findNof, PathfindingNode[,]grid)//function that finds the neigbours of an inputed node
    {
        List<PathfindingNode> listofneigbours = new List<PathfindingNode>();
        int ym1 = findNof.ypos - 1;
        int yp1 = findNof.ypos + 1;
        int xm1 = findNof.xpos - 1;
        int xp1 = findNof.xpos + 1;
        if (ym1 >= 0 && ym1 < gridy)
        {
            listofneigbours.Add(grid[findNof.xpos, ym1]);
        }
        if (yp1 >= 0 && yp1 < gridy)
        {
            listofneigbours.Add(grid[findNof.xpos, yp1]);
        }
        if (xm1 >= 0 && xm1 < gridx)
        {
            listofneigbours.Add(grid[xm1, findNof.ypos]);
        }
        if (xp1 >= 0 && xp1 < gridx)
        {
            listofneigbours.Add(grid[xp1, findNof.ypos]);
        }
        if (xm1 >= 0 && xm1 < gridx&& ym1 >= 0 && ym1 < gridy)
        {
            listofneigbours.Add(grid[xm1, ym1]);
        }
        if (xp1 >= 0 && xp1 < gridx && ym1 >= 0 && ym1 < gridy)
        {
            listofneigbours.Add(grid[xp1, ym1]);
        }
        if (xm1 >= 0 && xm1 < gridx && yp1 >= 0 && yp1 < gridy)
        {
            listofneigbours.Add(grid[xm1, yp1]);
        }
        if (xp1 >= 0 && xp1 < gridx && yp1 >= 0 && yp1 < gridy) { 
        listofneigbours.Add(grid[ xp1, yp1]); }
        return listofneigbours;//checks if the neigbours are within the bounds of the grid before adding them
       
    }

    public PathfindingNode nodefromworldpoint(Vector3 wpos, PathfindingNode[,] b)
    {
        float xpercent = (wpos.x + worldsize.x/2) / worldsize.x;
        float ypercent = (wpos.z + worldsize.y/2) / worldsize.y;
        xpercent = Mathf.Clamp01(xpercent);
        ypercent = Mathf.Clamp01(ypercent);
        int x =Mathf.RoundToInt((gridx - 1)* xpercent);
        int y = Mathf.RoundToInt((gridy - 1) * ypercent);
        return (b[x, y]);//finds the node that correstpons to a positon within the world
    }

    private void Update()
    {
        
    }
   
    public List<PathfindingNode> squaremaker(PathfindingNode C, PathfindingNode[,]grid)//creates a list of the nodes that surround an inputted node
    {
        List<PathfindingNode> clist = new List<PathfindingNode>();
        List<PathfindingNode> N = new List<PathfindingNode>();
        N = neighbourFind(C,grid);
        foreach(PathfindingNode x in N)
        {
           foreach(PathfindingNode n in neighbourFind(x, grid))
           {
                if (clist.Contains(n) == false)
                {
                    clist.Add(n);//finds neigbours and neigbours of neigbours
                }
           }
        }
        return clist;//used to find nodes infront of player
    }

  

}
