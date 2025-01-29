using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class VisitedLastPosCond : BehaviourNode
{
    public override NodeState Evaluate()
    {
        BehaviourNode p = getroot();
        if((p.DataChecker.TryGetValue("lastposvisited", out object visited))&& visited is bool)
        {
            bool v = (bool)visited;
            if(v== false)
            {
                return NodeState.SUCCESS;
            }
            return NodeState.FAILURE;
        }
        return NodeState.FAILURE;
    }
}
