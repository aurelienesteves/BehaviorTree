﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.AI
{
    public class Selector : TreeNode
    {
        int _currentChildIndex;

        public Selector()
        {
                Children = new List<TreeNode>();
        }

        public override TreeNodeState Update(BehaviorTree tree, GameObject owner)
        {
            for (; _currentChildIndex < Children.Count;)
            {
                var child = Children[_currentChildIndex];
                var state = child.Update(tree, owner);

                switch (state)
                {
                    case TreeNodeState.Success:
                        return TreeNodeState.Success;
                    case TreeNodeState.Failed:
                        _currentChildIndex++;
                        continue;
                    case TreeNodeState.Running:
                        break;
                }
            }

            return TreeNodeState.Success;
        }

        protected override void ResetInternal()
        {
            _currentChildIndex = 0;
        }


    }
}
