using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class SpinCondition : BehaviourNode
{
    private BehaviourNode p;
    private bool x;
 
    public override NodeState Evaluate()
    {
        p = getroot();
        if(p.DataChecker.TryGetValue("NeedToSpin", out object check)&& check is bool)
        {
            x = (bool)check;//checks in dictionary if need to spin bool is true
            if(x == true)
            {
                return NodeState.SUCCESS;
            }
            return NodeState.FAILURE;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
