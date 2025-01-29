using System.Collections;
using System.Collections.Generic;
using GameManage;
using UnityEngine;

public class GunItem : InventoryObj
{


   public  List<GameObject> enemyaims;
    MathsMethods mathsforangle = new MathsMethods();
    LayerMask walls;
    Transform shootingpos;
    public GunItem(Transform play, GameObject obs,string id, List<GameObject> enemies, LayerMask blocked)
    {
        player = play;
        Name = "Gun";//name of object
        Object = obs;
        UseableOnce = false;
        stacknum = 1;
        objectparts.Add(obs);
        ID = id;//identifies unique objes
        enemyaims = enemies;
        walls = blocked;
    }
    public override void Use()//function called when item used
    {
        foreach(GameObject x in enemyaims)
        {
            if (mathsforangle.islookingat(player.transform, x.transform, 20, 20, walls,true))//checks if gun shot at an enemy
            {
                Debug.Log("enemy shot");
                AngelBT enemyscript = x.GetComponent<AngelBT>();;//if an enemy was hit, it takes damage
                enemyscript.TakeDamage();

            }      
        }
    }

}
