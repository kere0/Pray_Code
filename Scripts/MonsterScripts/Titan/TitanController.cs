using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class TitanController : NormalMonsterBase
{
    void Awake()
    {
        base.Awake();
        
        
        targetID = TargetID.Titan;
        state = MonsterState.Idle;
    }
    private void Start()
    {
        base.Start();
        animStateEventListener.OnOccursAnimStateEvent -= TitanAttackEventRecieve;
        animStateEventListener.OnOccursAnimStateEvent += TitanAttackEventRecieve;
        CombatSystem.Instance.RegisterCreature(cc, this);
        ICollider = cc;
        // 탐지범위
        detectRange = 50f;
        maxHp = 100;       
        currentHp = maxHp;
        normalMonster = false;
    }
    private void Update()
    {
        ExecutionCheck();
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        UpdateState();
        QuestCheck();
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
                LeftHand.enabled = false;
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
    private void TitanAttackEventRecieve(string eventName)
    {
        switch (eventName)
        {
            case "TAttack1Start1" :
                RightHand.enabled = true;
                break;
            case "TAttack1End1" :
                RightHand.enabled = false;
                break;
            case "TAttack2Start1" :
                attackCircle = Managers.PoolManager.ObjPop("BlinkCircle", targetPoint.transform.position);
                CircleManager.Instance.CircleMonsterAdd(cc);
                StartCoroutine(CircleListRemove(cc));
                RightHand.enabled = true;
                break;
            case "TAttack2End1" :
                RightHand.enabled = false;
                break;
            case "TAttack2Start2" :
                LeftHand.enabled = true;
                break;
            case "TAttack2End2" :
                LeftHand.enabled = false;
                break;
            case "TAttack2Start3" :
                RightHand.enabled = true;
                break;
            case "TAttack2End3" :
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
            LeftHand.enabled = false;
            RightHand.enabled = false;
            Memory_Mission_Controller.Instance.Notify(targetID);
            cc.enabled = false;
            animator.Play("Death");
        }
    }
}