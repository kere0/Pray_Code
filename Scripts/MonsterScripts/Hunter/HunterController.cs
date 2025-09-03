using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class HunterController : NormalMonsterBase
{
    public Collider monsterCollider;

    void Awake()
    {
        base.Awake();
        targetID = TargetID.Hunter;
        state = MonsterState.Idle;
    }
    private void Start()
    {   
        base.Start();
        animStateEventListener.OnOccursAnimStateEvent -= HunterAttackEventRecieve;
        animStateEventListener.OnOccursAnimStateEvent += HunterAttackEventRecieve;
        CombatSystem.Instance.RegisterCreature(monsterCollider, this);
        ICollider = monsterCollider;
        attackRange = 3.5f;
        // 탐지범위              
        detectRange = 10f;   
        maxHp = 99;         
        currentHp = maxHp;   
        
    }
    private void Update()
    {
        ExecutionCheck();
        QuestCheck();
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        UpdateState();
        if (currentHp <= 0)
        {
            state = MonsterState.Death;
        }
        
    }
    void UpdateState()
    {
        switch (state)
        {
            case MonsterState.Idle:
                animator.Play("Idle");
                Detect();
                break;
            case MonsterState.Move:
                animator.Play("Walk");
                Move();
                break;
            case MonsterState.Attack:
                if (isAttacking == true)
                {
                    if (stateInfo.normalizedTime >= 1f)
                    {
                        state = MonsterState.Idle;
                        isAttacking = false;
                        return;
                    }
                }
                if (isAttacking == true) return;
                if (isAttacking == false)
                {
                    rand = Random.Range(0, 2);
                    isAttacking = true;
                    Vector3 lookDir = (player.transform.position - transform.position).normalized;
                    transform.rotation = Quaternion.LookRotation(lookDir);
                }

                AnimPlay("Attack1");
                break;
            case MonsterState.TakeDamage:
                animator.Play("GetHit");
                isAttacking = false;
                RightHand.enabled = false;
                cc.Move(forceReceiver.Movement * Time.deltaTime);
                if (stateInfo.normalizedTime >= 1.0f)
                {
                    rand = 0;
                    state = MonsterState.Move;
                }
                break;
            case MonsterState.Death:
                Death();
                break;
            default:
                break;
        }
    }
    private void HunterAttackEventRecieve(string eventName)
    {
        switch (eventName)
        {
            case "HAttackStart" :
                RightHand.enabled = true;
                break;
            case "HAttackEnd" :
                RightHand.enabled = false;
                break;
        }
    }
    void Death()
    {
        if (isDie == false)
        {
            isDie = true;
            currentHp = 0;
            RightHand.enabled = false;
            Memory_Mission_Controller.Instance.Notify(targetID);
            monsterCollider.enabled = false;
            animator.Play("Death");
        }
    }
}