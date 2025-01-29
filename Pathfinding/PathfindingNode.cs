using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingNode
{

    public bool passable;//if node can be crossed or not
    public bool inopen = false;
    public Vector3 worldcoord;
    public int xpos;
    public int ypos;//position in grid
    public int Gcost;
    public int Hcost;//costs
    public int Weight, HeapPos;
    public List<PathfindingNode> neighbours;
    public int Fcost { get { return Hcost + Gcost; } }
    public PathfindingNode parent = null;
    public  PathfindingNode(bool _passable, Vector3 _worldcoord, int _xpos,int _ypos)//constructs nodes
    {
        passable = _passable;
        worldcoord = _worldcoord;
        xpos = _xpos;
        ypos = _ypos;
    }
   
}
