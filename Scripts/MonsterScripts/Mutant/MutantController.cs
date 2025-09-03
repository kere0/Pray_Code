using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class MutantController : NormalMonsterBase
{
    
    void Awake()
    {
        base.Awake();
        targetID = TargetID.Mutant;
        state = MonsterState.Idle;
    }
    private void Start()
    {
        base.Start();
        animStateEventListener.OnOccursAnimStateEvent -= MutantAttackEventRecieve;
        animStateEventListener.OnOccursAnimStateEvent += MutantAttackEventRecieve;
        CombatSystem.Instance.RegisterCreature(cc, this);
        ICollider = cc;
        // 탐지범위              
        detectRange = 10f;   
        maxHp = 100;         
        currentHp = maxHp;   
        normalMonster = false;
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
                }
                Vector3 lookDir = (player.transform.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(lookDir);
                if (rand == 0)
                {
                    AnimPlay("Attack1");
                }
                else if (rand == 1)
                {
                    AnimPlay("Attack2");
                }
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
    private void MutantAttackEventRecieve(string eventName)
    {
        switch (eventName)
        {
            case "MAttack1Start1" :
                attackCircle = Managers.PoolManager.ObjPop("BlinkCircle", targetPoint.transform.position);
                CircleManager.Instance.CircleMonsterAdd(cc);
                StartCoroutine(CircleListRemove(cc));
                RightHand.enabled = true;
                break;
            case "MAttack1End1" :
                RightHand.enabled = false;
                break;
            case "MAttack2Start1" :
                RightHand.enabled = true;
                break;
            case "MAttack2End1" :
                RightHand.enabled = false;
                break;
            case "MAttack2Start2" :
                RightHand.enabled = true;
                break;
            case "MAttack2End2" :
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
            animator.Play("Death");
        }
    }
}