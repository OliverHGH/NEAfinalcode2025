using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameManage;

public class InventoryObjectManager : MonoBehaviour//maanges inventory objects
{
    public GameObject o4, o5, batteryinfo;
    public Text batterytext;
    public Transform p;
    public GameObject managerobject;
    public List<InventoryObj> ItemList = new List<InventoryObj>();
    public LayerMask blocked;
    protected List<GameObject> enemies = new List<GameObject>();
    TorchItem torch;
    GunItem gun;
    public GameManagerScript manager;

    void Start()
    {
        manager = managerobject.GetComponent<GameManagerScript>();
        enemies=manager.enemybodylist;
    }
    public virtual void ListofAllItems()
    {
        torch = new TorchItem(p, o4, "torch1",batteryinfo,batterytext);
        ItemList.Add(torch);
        gun = new GunItem(p, o5, "gun1", enemies, blocked);
        ItemList.Add(gun);//constructs items and puts them in a list
    }

    protected virtual void Update()
    {
         gun.enemyaims=manager.enemybodylist;//updates the list of targets for the gun
        if (torch.light.activeSelf)
        {
            torch.chargepercent-= (Time.deltaTime/2);
            if (torch.chargepercent < 0 )
            {
                torch.chargepercent = 0;//torch charge will be drained as it is used
            }
            int intpercentage = Mathf.RoundToInt(torch.chargepercent);
            string percent = (string)intpercentage.ToString();//makes a string of the percentage of battery remaining
            torch.messagetext.text = "Torch Battery: " + percent+"%";
            if(torch.chargepercent < 10)
            {
                torch.Flicker();
            }
            if (torch.chargepercent == 0)//torch turned off if power drained
            {
                Debug.Log("torch out of battery");
                torch.Use();
                torch.working = false;
            }
        } 
    }


}
