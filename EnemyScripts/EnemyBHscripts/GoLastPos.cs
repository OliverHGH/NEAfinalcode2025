using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class GoLastPos : BehaviourNode
{
    private pathfinder PathFinder;
    private Transform enemy, lastpos;
    private List<PathfindingNode> path;
    BehaviourNode rootretrive;
    bool pathexists = false;
    int n = 0;
    Vector3 positionlast,nextpos;
    public float dtime;
    PathfindingNode[,] gridpath;
    public GoLastPos(Transform e, pathfinder Path, GridNav g)//makes enemy go to the last place they saw the player in
    {
        enemy = e;
        PathFinder = Path;
        gridpath = g.creategrid();
    }
    public override NodeState Evaluate()
    {
        if (pathexists == false)
        {
            rootretrive = getroot();
            if ((rootretrive.DataChecker.TryGetValue("lastseenpos", out object pos)) && pos is Vector3)//gets positon player was last seen in from dictionary
            {
                path = PathFinder.FindPath(enemy.position, (Vector3)pos,gridpath);//creates a path to the position
                pathexists = true;
                positionlast = (Vector3)pos;
                n = 0;
            }
        }
        enemy.LookAt(positionlast);
        nextpos = new Vector3(path[n].worldcoord.x, enemy.position.y, path[n].worldcoord.z);
        enemy.position = Vector3.MoveTowards(enemy.position, nextpos, 15f * dtime);//enemy moves towards the next position in the path
        float xdif = Mathf.Abs(enemy.position.x - nextpos.x);
        float zdif = Mathf.Abs(enemy.position.z - nextpos.z);
        if (xdif + zdif < 0.05) //checks if close to next point
        {
           n += 1;//moves onto next point in path
        }
        if (xdif + zdif < 0.05 && n == (path.Count - 1))//checks if path has been fully traversed
        {
            
            pathexists = false;
            rootretrive.DataChecker["lastposvisited"] = true;//tells dictionary that the position has been visited
        }
        return NodeState.SUCCESS;
    }
    
}
