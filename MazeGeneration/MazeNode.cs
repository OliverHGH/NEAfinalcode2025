using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class MazeNode : MonoBehaviour
{
    public MazeNode parent;
    public List<MazeNode> Neigbours= new List<MazeNode>();
    public int Xpos, Ypos;
    public bool Visited;
    private bool AllNeigboursVisited;
    public bool Allvisited
    {
        get { return AllNeigboursVisited; }
    }
    [SerializeField]
    private GameObject Top, Left;

    public void DestroyTop()
    {
        Top.SetActive(false);//deletes top wall
    }
    public void DestroyLeft()//deletes left hand wall
    {
        Left.SetActive(false);
    }

    public MazeNode GetNeighbour()
    {
        if (Neigbours.Count == 0)
        {
            AllNeigboursVisited = true;
            return null;//retuns null if all neigbours are visited
        }
        int n = Random.Range(0, Neigbours.Count);
        while (Neigbours[n].Visited)
        {
            Neigbours.Remove(Neigbours[n]);
            if (Neigbours.Count == 0)
            {
                AllNeigboursVisited = true;
                return null;
            }
            n = Random.Range(0, Neigbours.Count);//goes through neigbour list until one is unvisited
        }
        if(Neigbours.Count == 1)
        {
            AllNeigboursVisited = true;
        }
        return Neigbours[n];
    }
}
