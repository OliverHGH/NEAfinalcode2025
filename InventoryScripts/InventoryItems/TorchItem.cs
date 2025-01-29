using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TorchItem : InventoryObj
{
    public GameObject light;
    GameObject message;
    public Text messagetext;
    public float chargepercent;
    public bool working = true;
    public override void Use()
    {
        if (working)
        {
            light.SetActive(!light.activeSelf);
            message.SetActive(!message.activeSelf);
        }
    }

    public void Flicker()
    {
        int random = Random.Range(1, 10);
        if (random == 1)
        {
            Use();
        }
    }

    public TorchItem(Transform play, GameObject obs, string id,GameObject batterylevel, Text info)
    {
        player = play;
        Name = "Torch";
        Object = obs;
        UseableOnce = false;
        stacknum = 1;
        objectparts.Add(obs);
        Transform lightt = obs.transform.GetChild(0);
        light = lightt.gameObject;
        ID = id;
        chargepercent = 100;
        message = batterylevel;
        messagetext = info;
        
    }

}
