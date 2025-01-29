using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryObj//base class for unventory object 
{
    protected int stacknum;//maximum amount of object that can be stacked
    public int maxstack
    {
        get { return stacknum; }
    }
    protected string Name;
    public string objectname//name of object
    {
        get { return Name; }
    }

    protected bool UseableOnce = false;
    public bool UseOnce
    {
        get { return UseableOnce; }
    }
    public bool ininv=false;//checks if item is in inventory
    protected Transform player;//reference to player
    public GameObject Object;
    public List<GameObject> objectparts = new List<GameObject>();//all parts of the item
    public string ID;
    
    public virtual void Use()
    {
    }
    public void Drop()
    {
        ininv = false;
        Vector3 pos = player.position + player.forward*2f+player.up*.3f;
        Equip();
        Object.transform.position = pos;//drops the item infront of player

    }


    public virtual void Equip()
    {
        foreach (GameObject x in objectparts)
        {
            x.SetActive(true); //sets items to active
        }
    }
    public virtual void UnEquip()
    {
        foreach(GameObject x in objectparts)
        {
            x.SetActive(false);
        }
    }
    public virtual void Pickup()
    {
        UnEquip();//sets it to inactive
        ininv = true;
    }
  
}
