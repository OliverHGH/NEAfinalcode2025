using Unity.VisualScripting;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public MazeNode[,] MazeGrid;
    public MazeNode Node;
    private MazeNode CurrentNode, NextNode;
    int size,half;
    void Awake()
    {
        size =50;//decides size of maze by determining its length in modes
        int xlength = size;
        int ylength = size;
        half = size / 2;
        MazeGrid = new MazeNode[xlength, ylength];
        for (int x=0; x <size ; x++)
        {
            for (int y=0; y <size ; y++)
            { 
                Vector2 pos = new Vector2(ArrayToWorld(x),ArrayToWorld(y));
                MazeGrid[x,y]= Instantiate(Node,new Vector3(pos.x,2.25f,pos.y),Quaternion.identity);//creates new maze nodes to fill array
                MazeGrid[x, y].Xpos = x;
                MazeGrid[x,y].Ypos = y;
            }
        }
        for(int x=0; x <size; x++)
        {
            for(int y=0; y <size; y++)
            {
                if (x + 2 < size)
                {
                    MazeGrid[x,y].Neigbours.Add(MazeGrid[x+1,y]);
                }
                if (x > 0)
                {
                    MazeGrid[x, y].Neigbours.Add(MazeGrid[x -1, y]);
                }
                if (y + 1 < size)
                {
                    MazeGrid[x, y].Neigbours.Add(MazeGrid[x, y+1]);
                }
                if (y > 1)
                {
                    MazeGrid[x, y].Neigbours.Add(MazeGrid[x, y-1]);
                }
            }
        }//adds neigbours to nodes if the nodes are in bounds
        CurrentNode = MazeGrid[1, 1];//creates root of mode
        NextNode = CurrentNode.GetNeighbour();
        Path();
        while(CurrentNode.Xpos != 1 || CurrentNode.Ypos != 1)//will iterate until path returns to original node
        {
            CurrentNode = FindUnvisited(CurrentNode);//goes back up path until it finds a node with unvisited neigbours
            NextNode = CurrentNode.GetNeighbour();
            Path();//repeatedly calls path function
        }
        for(int i = 0; i < (size *size)/5; i++)
        {
            int x = Random.Range(1, size - 1);
            int y = Random.Range(1, size - 1);
            MazeGrid[x,y].DestroyLeft();
            x = Random.Range(1, size - 1);
            y = Random.Range(1, size - 1);//randomly destroys walls to open up map
            MazeGrid[x, y].DestroyTop();
        }
        for (int i = 0; i < 5; i++)
        {
            CreateRoom();
        }
    }



    private int ArrayToWorld(int pos)
    {
        return (pos - half) * 5;//turns a position in the array into a world position
    }//the array is centered on 0,0, and each node in the maze is a 5*5 square

    private void CreateRoom()
    {
        int roomsize = Random.Range(4, 6);//knocks down all walls in a random square to create a room
        int x = Random.Range(1, size-(roomsize+2));
        int y = Random.Range(1, size - (roomsize+2));
        for (int roomx = x; roomx < (x + roomsize); roomx++)
        {
            for (int roomy = y; roomy < (y + roomsize); roomy++)
            {
                if(roomy > size)
                {
                    Debug.Log("test weird");
                }
                MazeGrid[roomx,roomy].DestroyLeft();
                MazeGrid[roomx,roomy].DestroyTop();

            }
        }

    }

    private void Path()
    {
        while(NextNode != null)
        {
            CurrentNode.Visited = true;
            NextNode.Visited = true;
            NextNode.parent = CurrentNode;//nextnode's parent is set to be the current node
            if (NextNode.Xpos > CurrentNode.Xpos)
            {
                NextNode.DestroyLeft();
            }
            if(NextNode.Xpos < CurrentNode.Xpos)
            {
                CurrentNode.DestroyLeft();
            }
            if(NextNode.Ypos > CurrentNode.Ypos)
            {
                CurrentNode.DestroyTop();
            }
            if (NextNode.Ypos < CurrentNode.Ypos)
            {
                NextNode.DestroyTop();
            }//makes a path between current and new node
            CurrentNode=NextNode;//new node is now current node
            NextNode=CurrentNode.GetNeighbour();//next node is random neigbour of current node
        }
    }

    private MazeNode FindUnvisited(MazeNode node)
    {
        while (node.Allvisited)//backtracks until it finds a node with unvisited neigbours
        {
            if (node.Xpos == 1 && Node.Ypos == 1)///if it backtracks to begining without finding unvisited node, it will return original node
            {
                return node;
            }
            node = node.parent;
        }
        return node;
    }
}
