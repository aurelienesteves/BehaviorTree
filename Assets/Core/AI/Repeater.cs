using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.AI
{
    public class Repeater : TreeNode
    {

        public Repeater()
        {
                Children = new List<TreeNode>();
        }

        public override TreeNodeState Update(BehaviorTree tree, GameObject owner)
        {
            tree.SetRepeater(this);

            var state = Children[0].Update(tree, owner);

            if ( state == TreeNodeState.Success || state == TreeNodeState.Failed)
            {
                Children[0].Reset();
            }

            return state;
        }




    }
}
