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

        Wait waitShoot = new Wait { Timer = 0.3f };
        sequence2.Children.Add(waitShoot);

        Shoot shoot = new Shoot();
        Weapon weapon = Weapon;
        shoot.shakeCamera = true;
        shoot.weapons.Add(weapon);

        sequence2.Children.Add(shoot);

        SpawnFallingRocks rocks = new SpawnFallingRocks();
        rocks.rockPrefab = RocksPrefab.GetComponent<AbstractProjectile>();
        rocks.spawnAreaCollider = SpawnAreaCollider;
        sequence2.Children.Add(rocks);


        Wait wait2 = new Wait { Timer = 3 };
        sequence2.Children.Add(wait2);

        Repeater repeater = new Repeater();

        RandomSelector selector = new RandomSelector();
        selector.Children.Add(sequence);
        selector.Children.Add(sequence2);




        Selector initialSelector = new Selector();
        Sequence nextStep = new Sequence();
        IsHealthUnder health = new IsHealthUnder();
        SetBlackboard blackboard = new SetBlackboard();
        blackboard.Name = "BossStep";
        blackboard.Value = 1;
        
        nextStep.Children.Add(health);
        nextStep.Children.Add(blackboard);

        initialSelector.Children.Add(nextStep);
        initialSelector.Children.Add(selector);

        Selector stepSelector = new Selector();


        Sequence sequenceStep2 = new Sequence();

        CheckBlackboard checkStep2 = new CheckBlackboard();
        checkStep2.Value = 2;
        checkStep2.Name = "BossStep";

        Log log = new Log { Text = "Step2" };

        sequenceStep2.Children.Add(checkStep2);
        sequenceStep2.Children.Add(log);


        Sequence sequenceStep1 = new Sequence();

        CheckBlackboard checkStep = new CheckBlackboard();
        checkStep.Value = 1;
        checkStep.Name = "BossStep";

        sequenceStep1.Children.Add(checkStep);

        Jump recoverJump = new Jump();
        recoverJump.horizontalForce = -4;
        recoverJump.jumpForce = 10;
        recoverJump.animationTriggerName = "Roll";

        Wait waitJump = new Wait { Timer = 1.0f };

        SpawnAndWaitDead spawnMaggot = new SpawnAndWaitDead();
        spawnMaggot.DamageCollider = DamageCollider;
        spawnMaggot.Prefab = MaggotPrefab;
        spawnMaggot.Transform = MaggotTransform;

        SetBlackboard step2 = new SetBlackboard();
        step2.Name = "BossStep";
        step2.Value = 2;

        sequenceStep1.Children.Add(recoverJump);
        sequenceStep1.Children.Add(waitJump);
        sequenceStep1.Children.Add(spawnMaggot);
        sequenceStep1.Children.Add(step2);

        stepSelector.Children.Add(sequenceStep2);
        stepSelector.Children.Add(sequenceStep1);
        stepSelector.Children.Add(initialSelector);

        repeater.Children.Add(stepSelector);

        root.Children.Add(repeater);

        BehaviorTree.Root = root;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
