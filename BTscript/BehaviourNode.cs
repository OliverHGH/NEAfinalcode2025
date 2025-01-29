using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public enum NodeState//enum for states
    {
        RUNNING,
        FAILURE,
        SUCCESS
    }


    public abstract class BehaviourNode//base class for behaviour nodes
    {
        public BehaviourNode parent = null;
        protected NodeState state;
        public Dictionary<string,object> DataChecker = new Dictionary<string,object>();
        public BehaviourNode getroot()//will get the root of the tree- used for nodes to share information through a dictionary
        {
            BehaviourNode r;
            r = this;
            while (r.parent != null)
            {
                r = r.parent;
            }
            return r;
        }

        public List<BehaviourNode> BNchildren;//list of nodes children
        public virtual NodeState Evaluate()//embty base method for evaluating nodes
        {
            return NodeState.FAILURE;
        }

    }


}