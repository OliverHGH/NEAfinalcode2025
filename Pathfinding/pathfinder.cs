using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
public class pathfinder : MonoBehaviour
{
    GridNav grid;
    public List<PathfindingNode> pointstowalk;
    public bool sneakymode = false;
    void Awake()
    {
        grid = GetComponent<GridNav>();
    }
    static int FindDif(PathfindingNode a, PathfindingNode b)
    {
        int ydif = a.ypos - b.ypos;
        if (ydif < 0)
        {
            ydif = -ydif;
        }
        int xdif = a.xpos - b.xpos;
        if (xdif < 0)
        {
            xdif = -xdif;
        }
        float Fanswer = Mathf.Sqrt((xdif * xdif) + (ydif * ydif));
        return (Mathf.FloorToInt(10 * Fanswer));
    }//finds dif between nodes using pythag, multipleis by 10
    public List<PathfindingNode> FindPath(Vector3 start, Vector3 end, PathfindingNode[,] PathGrid)
    {
        
        minHeap Openlist = new minHeap();
        HashSet<PathfindingNode> ClosedList = new HashSet<PathfindingNode>();
        PathfindingNode firstnode = grid.nodefromworldpoint(start,PathGrid);//finds grid corresponding to start
        PathfindingNode endnode = grid.nodefromworldpoint(end,PathGrid); //finds grid corresponding to end
        if(endnode.passable == false)
        {
           foreach(PathfindingNode alt in endnode.neighbours)
            {
                if(alt.passable == true)
                {
                    endnode = alt;
                }
            }
        }//if the end node is blocked, a random neigbour will be chosen instead
        firstnode.parent = null;
        Openlist.Insert(firstnode);
        while (Openlist.listMax >= 0)//while the openlist contains nodes
        {

            PathfindingNode currentnode = Openlist.Takemin();//gets node in openlist with lowest cost
            ClosedList.Add(currentnode);
            if (currentnode.xpos == endnode.xpos && currentnode.ypos == endnode.ypos)//if the current node is the target node
            {
                List<PathfindingNode> foundpath = new List<PathfindingNode>();
                PathfindingNode x = currentnode;
                while (x.parent != null)
                {
                    foundpath.Add(x);
                    x = x.parent;
                }
                foundpath.Add(firstnode);
                foundpath.Reverse();//gets path by adding the parents of nodes, and then reversing order
                List<PathfindingNode> waypoints = simplifypath(foundpath);
                ClosedList = null;
                Openlist = null;//resets lists
                return waypoints;
            }
            else
            {
                foreach (PathfindingNode posbeingchecked in currentnode.neighbours)//checks the neigbouring nodes
                {
                    bool alreadyinopen = false;

                    if (posbeingchecked.passable == false || ClosedList.Contains(posbeingchecked)) // skips if neighbour is inpassable
                    {
                        continue;
                    }//skips if node is in closed list or blocked
                    for (int isino = 0; isino <= Openlist.listMax; isino++)
                    {

                        if (Openlist.heap[isino].xpos == posbeingchecked.xpos && Openlist.heap[isino].ypos == posbeingchecked.ypos) //checks if item is already in heap
                        {

                            alreadyinopen = true;
                            int ngcost = currentnode.Gcost + FindDif(posbeingchecked, currentnode);
                            if (Openlist.heap[isino].Gcost > ngcost)//if the new path is cheaper
                            {

                                Openlist.heap[isino].Gcost = ngcost;
                                Openlist.heap[isino].parent = currentnode;
                                Openlist.UpShift(isino); // update node's cost and parent and mantain heap property


                            }
                            break;
                        }
                    }
                    if (alreadyinopen == false)//if neigbour not in open
                    {

                        posbeingchecked.parent = currentnode;
                        posbeingchecked.Gcost = currentnode.Gcost + FindDif(currentnode, posbeingchecked);
                        posbeingchecked.Hcost = FindDif(posbeingchecked, endnode);//has costs and parent initialised
                        if (sneakymode == true)//if enemy needs to be sneaky, it will consider the weight of nodes
                        {
                            posbeingchecked.Hcost += posbeingchecked.Weight;
                        }
                        Openlist.Insert(posbeingchecked);      // adds node to open
                    }
                }
            }

        }
        ClosedList = null;
        Openlist = null;
        List<PathfindingNode> empty = new List<PathfindingNode>();
        return empty;
        
    }

    List<PathfindingNode> simplifypath(List<PathfindingNode> path)//simplifies the path by removing nodes that are in a straight line
    {
        List<PathfindingNode> waypoints = new List<PathfindingNode>();
        Vector2 lastdirection = new Vector2(0, 0);
        for (int n = 1; n < path.Count; n++)
        {
            Vector2 newdirection = new Vector2(path[n - 1].xpos - path[n].xpos, path[n - 1].ypos - path[n].ypos);//if the new node is in same directon as last one, not added
            if (lastdirection != newdirection)
            {
                waypoints.Add(path[n - 1]);
            }
            lastdirection = newdirection;
        }
        waypoints.Add(path[path.Count - 1]);//adds last node
        return waypoints;
    }

}


