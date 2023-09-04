using System;
using UnityEngine;
namespace Core.AI
{
    public class BehaviorTree : MonoBehaviour
    {
        public TreeNode Root { get; set; }

        TreeNode _currentNode;

        private void Awake()
        {
           
        }

        private void Update()
        {
            if ( _currentNode != null)
            {
                _currentNode.Update(this, gameObject);
            }
            else
            {
                Root.Update(this, gameObject);
            }
        }


        internal void SetRepeater(Repeater repeater)
        {
            _currentNode = repeater; 
        }
    }

}
