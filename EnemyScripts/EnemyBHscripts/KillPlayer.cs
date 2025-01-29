using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using GameManage;
public class KillPlayer : BehaviourNode
{
    GameManagerScript manager;
    AngelBT killer;
    public KillPlayer(GameManagerScript gm, AngelBT enemy)
    {
        manager = gm;
        killer = enemy;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("u ded lol");
        manager.gameover = true;//tells game manager player has been killed
        killer.haskilledplayer = true;//determines which enemy killed player
        return NodeState.SUCCESS;
    }

}
