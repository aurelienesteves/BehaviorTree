using Core.AI;
using Core.Combat;
using Core.Combat.Projectiles;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameplay : MonoBehaviour
{
    public BehaviorTree BehaviorTree;
    public Weapon Weapon;

    // Start is called before the first frame update
    void Start()
    {
        CreateBehavior();
    }

    private void CreateBehavior()
    {
        TreeNode root = new TreeNode();
        root.Children = new List<TreeNode>();
        Sequence sequence = new Sequence();

        Wait wait = new Wait();
        wait.Timer = 1.3f;
        sequence.Children.Add(wait);

        Jump jump = new Jump();
        jump.horizontalForce = 3;
        jump.jumpForce = 20;
        jump.buildupTime = 0.5f;
        jump.jumpTime = 1.3f;
        jump.animationTriggerName = "Jump";
        jump.shakeCameraOnLanding = true;
        sequence.Children.Add(jump);

        FacePlayer face = new FacePlayer();
        sequence.Children.Add(face);




        Sequence sequence2 = new Sequence();

        SetTrigger startAttack = new SetTrigger { triggerName = "StartAttack" };
        sequence2.Children.Add(startAttack);

        Wait waitAttack = new Wait {  Timer = 1.2f };
        sequence2.Children.Add(waitAttack);

        SetTrigger attack = new SetTrigger { triggerName = "Attack" };
        sequence2.Children.Add(attack);

        Shoot shoot = new Shoot();
        Weapon weapon = Weapon;
        shoot.shakeCamera = true;
        shoot.weapons.Add(weapon);

        sequence2.Children.Add(shoot);

        Wait wait2 = new Wait { Timer = 1.2f };
        sequence2.Children.Add(wait2);

        Repeater repeater = new Repeater();
        repeater.Children.Add(sequence2);

        root.Children.Add(repeater);

        BehaviorTree.Root = root;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
