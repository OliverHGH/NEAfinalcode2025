using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class PatrolScript : BehaviourNode
{
    pathfinder finder;
    GridNav grid;
    Transform enemy;
    List<PathfindingNode> pathpatrol;
    public float timer;
    public float dtime;
    int Lpos = 0;
    public int worldpatrolsize = 50;
    Vector3 nextpos;
    PathfindingNode[,] pathgrid;
    public PatrolScript(pathfinder find,GridNav g, Transform e)
    {
        enemy = e;
        finder = find;
        grid = g;
        pathgrid = g.creategrid();
    }
    public override NodeState Evaluate()
    {
        BehaviourNode p = getroot();
        
        if (pathpatrol == null)
        {
   
            Lpos = 0;
            int x = Random.Range(-(worldpatrolsize - 1), worldpatrolsize);
            int z = Random.Range(-(worldpatrolsize - 1), worldpatrolsize);
            Vector3 pos = new Vector3(x, 0, z);
            while (grid.nodefromworldpoint(pos,pathgrid).passable == false)//if chosen pos is blocked ,a new position is selected
            {
                x = Random.Range(-(worldpatrolsize-1), worldpatrolsize);
                z = Random.Range(-(worldpatrolsize - 1), worldpatrolsize);
                pos = new Vector3(x, 0, z);//finds a new random position in world limits

            }

            if (finder == null)
            {
                Debug.Log("pathfinder is null");
            }
            pathpatrol = finder.FindPath(enemy.position,pos,pathgrid);//finds path to chosen location
        }
       
        nextpos = new Vector3(pathpatrol[Lpos].worldcoord.x, enemy.position.y, pathpatrol[Lpos].worldcoord.z);
        enemy.LookAt(nextpos);
        enemy.position = Vector3.MoveTowards(enemy.position, nextpos, 6f * dtime);//moves towards point in list
        float xdif = Mathf.Abs(enemy.position.x - nextpos.x);
        float zdif = Mathf.Abs(enemy.position.z - nextpos.z);
        if (xdif + zdif < 0.05) //checks if close to next point
        {
            Lpos += 1;//will now move towards next point in list
        }
        if (xdif + zdif < 0.05 && Lpos == (pathpatrol.Count - 1))
        {
            pathpatrol = null;//path is null if it has been traversed fully
        }
        return NodeState.SUCCESS;
    }
}
