
using System.Collections.Generic;
using System.Linq;

public class minHeap//mininum meap, item 0 will always have the lowest value
{
    public List<PathfindingNode> heap = Enumerable.Repeat(default(PathfindingNode), 2000).ToList();
    protected int IndexCount = -1;
    public int listMax
    {
        get { return IndexCount; }
    }
    void swap(int x, int y)//swaps the position of two items in the heap
    {
        PathfindingNode temp = heap[x];
        heap[x] = heap[y];
        heap[y] = temp;

    }
    public void UpShift(int n)//item will be continuously shifted up the heap by swapping with its parent until its cparent has a lower value
    {
        while (n > 0 && (heap[n].Fcost < heap[(n - 1) / 2].Fcost || (heap[n].Fcost== heap[(n - 1) / 2].Fcost && heap[n].Hcost < heap[(n - 1) / 2].Hcost)))
        {//checks if the parent has a hight or lower cost 
            swap(n, (n - 1) / 2);
            n = (n - 1) / 2;
        }
    }
    public void Insert(PathfindingNode n)//inserts a new item into heap
    {
        IndexCount += 1;
        heap[IndexCount] = n;//added to last position and upshifted
        UpShift(IndexCount);

    }
    void DownShift(int n)//item swapped with child until child has a higher value
    {
        int M = n;
        int left = (n * 2) + 1;//left child
        int right = (n * 2) + 2;//right child
        if (left <= IndexCount && heap[left].Fcost < heap[M].Fcost)
        {
            M = left;
        }
        if (right <= IndexCount && heap[right].Fcost < heap[M].Fcost)
        {
            M = right;
        }
        if (n != M)
        {
            swap(n, M);
            DownShift(M);
        }
    }
    public void Remove(int n)
    {//removes from list
        heap[n] = heap[IndexCount];
        heap[IndexCount] = null;
        IndexCount -= 1;//swaps with last item and shifts last item back down
        DownShift(n);
    }
    public PathfindingNode Takemin()//takes the lowest item
    {
        PathfindingNode min = heap[0];
        heap[0] = heap[IndexCount];
        heap[IndexCount] = null;
        IndexCount -= 1;//swaps with last item and downshifts last item
        DownShift(0);
        return min;
    }
}
