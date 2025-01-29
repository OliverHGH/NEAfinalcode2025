using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class SelectorNode : BehaviourNode
    {
        public SelectorNode(List<BehaviourNode> list)
        {
            BNchildren = list;
            foreach(BehaviourNode x in BNchildren)
            {
                x.parent = this;
            }
        }
        public override NodeState Evaluate()
        {
            foreach(BehaviourNode x in BNchildren)//will evaluate list of children until one is a success
            {
                state = x.Evaluate();
                if (state == NodeState.SUCCESS || state == NodeState.RUNNING)
                {
                    return state;
                }
            }
            state = NodeState.FAILURE;
            return state;
        }

    }

}

