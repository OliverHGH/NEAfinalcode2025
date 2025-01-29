using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class SpinToLook : BehaviourNode
{
    private Transform enemy;
    private Vector3 rotate = new Vector3(0, 1, 0);
    public float dtime;
    private BehaviourNode p;
    
    public float timer;
    public SpinToLook(Transform e)
    {
        enemy = e;
        timer = 0;
    }

    public override NodeState Evaluate()
    {
        p = getroot();
        timer += dtime;
        if (timer > 2)//will execute when spun fully(2*180=360)
        {
            p.DataChecker["NeedToSpin"] = false;
            timer = 0;
            return NodeState.SUCCESS;//won't need to spin anymore when fully rotated
        }
        enemy.Rotate(rotate * dtime*180);//will spin around
        return NodeState.SUCCESS;
    }

}
