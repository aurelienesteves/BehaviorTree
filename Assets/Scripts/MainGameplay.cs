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
    public Collider2D SpawnAreaCollider;
    public GameObject DamageCollider;
    public GameObject RocksPrefab;
    public GameObject MaggotPrefab;
    public Transform MaggotTransform;

    void Start()
    {
        CreateBehavior();
    }


    TreeNode SequenceFallingAttack()
    {
        Repeater repeater = new Repeater { RepeatCount = 8 };
        Sequence sequence = new Sequence();

        SpawnFallingRocks rocks = new SpawnFallingRocks();
        rocks.rockPrefab = RocksPrefab.GetComponent<AbstractProjectile>();
        rocks.spawnAreaCollider = SpawnAreaCollider;
        sequence.Add(rocks);

        SetTrigger attack = new SetTrigger { triggerName = "Attack" };
        sequence.Add(attack);

        sequence.Add(new Wait { Timer = 0.6f });
        sequence.Add(new ChangeDirection());

        repeater.Add(sequence);

        return repeater;
    }

    TreeNode SequenceJumpAttack()
    {
        Sequence sequence = new Sequence();

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

        sequence.Add(new Wait { Timer = 3});

        return sequence;
    }

    TreeNode SequenceLittleAttack()
    {
        Sequence sequence = new Sequence();

        FacePlayer face = new FacePlayer();
        sequence.Children.Add(face);

        SetTrigger startAttack = new SetTrigger { triggerName = "StartAttack" };
        sequence.Children.Add(startAttack);

        Wait waitAttack = new Wait { Timer = 1.2f };
        sequence.Children.Add(waitAttack);

        SetTrigger attack = new SetTrigger { triggerName = "Attack" };
        sequence.Children.Add(attack);

        return sequence;
    }

    TreeNode SequenceMiddleAttack()
    {
        Sequence sequence = new Sequence();

        FacePlayer face = new FacePlayer();
        sequence.Children.Add(face);

        SetTrigger startAttack = new SetTrigger { triggerName = "StartAttack" };
        sequence.Children.Add(startAttack);

        Wait waitAttack = new Wait { Timer = 1.2f };
        sequence.Children.Add(waitAttack);

        SetTrigger attack = new SetTrigger { triggerName = "Attack" };
        sequence.Children.Add(attack);

        Wait waitShoot = new Wait { Timer = 0.3f };
        sequence.Children.Add(waitShoot);

        Shoot shoot = new Shoot();
        Weapon weapon = Weapon;
        shoot.shakeCamera = true;
        shoot.weapons.Add(weapon);

        sequence.Children.Add(shoot);

        return sequence;
    }

    TreeNode SequenceBigAttack()
    {
        Sequence sequence = new Sequence();

        FacePlayer face = new FacePlayer();
        sequence.Children.Add(face);

        SetTrigger startAttack = new SetTrigger { triggerName = "StartAttack" };
        sequence.Children.Add(startAttack);

        Wait waitAttack = new Wait { Timer = 1.2f };
        sequence.Children.Add(waitAttack);

        SetTrigger attack = new SetTrigger { triggerName = "Attack" };
        sequence.Children.Add(attack);

        Wait waitShoot = new Wait { Timer = 0.3f };
        sequence.Children.Add(waitShoot);

        Shoot shoot = new Shoot();
        Weapon weapon = Weapon;
        shoot.shakeCamera = true;
        shoot.weapons.Add(weapon);

        sequence.Children.Add(shoot);

        SpawnFallingRocks rocks = new SpawnFallingRocks { spawnCount = 10 };
        rocks.rockPrefab = RocksPrefab.GetComponent<AbstractProjectile>();
        rocks.spawnAreaCollider = SpawnAreaCollider;
        sequence.Children.Add(rocks);

        return sequence;
    }

    TreeNode SequenceToNextStep(int step)
    {
        Sequence sequence = new Sequence();
        Jump recoverJump = new Jump();
        recoverJump.horizontalForce = -4;
        recoverJump.jumpForce = 10;
        recoverJump.animationTriggerName = "Roll";

        Wait waitJump = new Wait { Timer = 0.7f };

        SpawnAndWaitDead spawnMaggot = new SpawnAndWaitDead();
        spawnMaggot.DamageCollider = DamageCollider;
        spawnMaggot.Prefab = MaggotPrefab;
        spawnMaggot.Transform = MaggotTransform;

        SetBlackboard step2 = new SetBlackboard();
        step2.Name = "BossStep";
        step2.Value = step;

        sequence.Add(recoverJump);
        sequence.Add(waitJump);
        sequence.Add(spawnMaggot);
        sequence.Add(step2);

        return sequence;
    }

    TreeNode SequenceIntro()
    {
        Sequence sequence = new Sequence();
        sequence.Add(new FacePlayer());
        Jump jump = new Jump();
        jump.horizontalForce = 0;
        jump.jumpForce = 15;
        jump.jumpTime = 1.4f;
        jump.animationTriggerName = "Jump";
        jump.shakeCameraOnLanding = true;
        sequence.Add(jump);
        return sequence;
    }


    TreeNode SequencePhase1()
    {
        Sequence sequence = new Sequence();
        sequence.Add(SequenceLittleAttack());
        sequence.Add(new Wait { Timer = 1 });
        sequence.Add(SequenceJumpAttack());

        return sequence;
    }

    private void CreateBehavior()
    {
        TreeNode root = new TreeNode();
        root.Children = new List<TreeNode>();
        var sequence = SequenceIntro();
        sequence.Add(new Wait { Timer = 1 });
        Repeater repeater = new Repeater();
        sequence.Add(repeater);


        Sequence sequenceCheckPhase2 = new Sequence();
        sequenceCheckPhase2.Add(  new IsHealthUnder { HealthTreshold = 10 } );
        sequenceCheckPhase2.Add(SequenceToNextStep(1));


        Selector selector = new Selector();
        selector.Add(sequenceCheckPhase2);
        selector.Add(SequencePhase1());

        repeater.Add(selector);



        root.Add(sequence);
        //Sequence sequence = new Sequence();

        //Wait wait = new Wait();
        //wait.Timer = 1.3f;
        //sequence.Children.Add(wait);


      

        //Repeater repeater = new Repeater();

        //RandomSelector selector = new RandomSelector();
        //selector.Children.Add(sequence);
        //selector.Children.Add(sequence2);

        //Selector initialSelector = new Selector();
        //Sequence nextStep = new Sequence();
        //IsHealthUnder health = new IsHealthUnder();
        //SetBlackboard blackboard = new SetBlackboard();
        //blackboard.Name = "BossStep";
        //blackboard.Value = 1;

        //nextStep.Children.Add(health);
        //nextStep.Children.Add(blackboard);

        //initialSelector.Children.Add(nextStep);
        //initialSelector.Children.Add(selector);

        //Selector stepSelector = new Selector();
        //Sequence sequenceStep2 = new Sequence();

        //CheckBlackboard checkStep2 = new CheckBlackboard();
        //checkStep2.Value = 2;
        //checkStep2.Name = "BossStep";

        //Log log = new Log { Text = "Step2" };

        //sequenceStep2.Children.Add(checkStep2);
        //sequenceStep2.Children.Add(log);

        //Sequence sequenceStep1 = new Sequence();

        //CheckBlackboard checkStep = new CheckBlackboard();
        //checkStep.Value = 1;
        //checkStep.Name = "BossStep";

        //sequenceStep1.Children.Add(checkStep);



        //stepSelector.Children.Add(sequenceStep2);
        //stepSelector.Children.Add(sequenceStep1);
        //stepSelector.Children.Add(initialSelector);

        //repeater.Children.Add(stepSelector);

        // root.Children.Add(repeater);

        BehaviorTree.Root = root;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
