using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public abstract class Btree : MonoBehaviour 
    {
        protected BehaviourNode root;//root node of the tree

        public virtual void Start()
        {
            root = CreateTree(); //creates a tree and assigns the root to be the rot of the created tree
        }
        public virtual void Update()
        {
            root.Evaluate(); //evaluates the root and in turn the whole tree
        }

        public abstract BehaviourNode CreateTree();//empty base for creating a tree
      


    }
}
