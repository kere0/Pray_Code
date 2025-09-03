using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class KryllController : NormalMonsterBase
{
    private BoxCollider boxCollider;
    public GameObject spiltKryll;
    
    void Awake()
    {
        base.Awake();
        spiltKryll = Resources.Load<GameObject>("Kryll/Split_Kryll");
        TryGetComponent(out boxCollider);
        targetID = TargetID.Kryll;
        state = MonsterState.Idle;
    }
    private void Start()
    {
        base.Start();
        animStateEventListener.OnOccursAnimStateEvent -= KyllAttackEventRecieve;
        animStateEventListener.OnOccursAnimStateEvent += KyllAttackEventRecieve;
        CombatSystem.Instance.RegisterCreature(boxCollider, this);
        ICollider = boxCollider;

        // 탐지범위
        detectRange = 10f;
        maxHp = 63;
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
                //animator.Play("Walk");
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
                    // if (lookDir != Vector3.zero)
                    // {
                    //     Quaternion targetRotation = Quaternion.LookRotation(lookDir);
                    //     //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime*10f);
                    // }
                    transform.rotation = Quaternion.LookRotation(lookDir);
                    if (rand == 0)
                    {
                        AnimPlay("Attack1");

                    }
                    else if (rand == 1)
                    {
                        AnimPlay("Attack2");
                    }
                    else if (rand == 2)
                    {
                        AnimPlay("Attack3");
                    }
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
                //animator.Play("Death");
                Death();
                break;
            default:
                break;
        }
    }
    private void KyllAttackEventRecieve(string eventName)
    {
        switch (eventName)
        {
            case "Attack1Start" :
                LeftHand.enabled = true;
                break;
            case "Attack1End" :
                LeftHand.enabled = false;
                break;
            case "Attack2Start1" :
                LeftHand.enabled = true;
                attackCircle = Managers.PoolManager.ObjPop("BlinkCircle", targetPoint.transform.position);
                CircleManager.Instance.CircleMonsterAdd(boxCollider);
                StartCoroutine(CircleListRemove(boxCollider));
                break;
            case "Attack2End1" :
                LeftHand.enabled = false;
                break;
            case "Attack2Start2" :
                RightHand.enabled = true;
                break;
            case "Attack2End2" :
                RightHand.enabled = false;
                break;
            case "Attack3Start" :
                LeftHand.enabled = true;
                RightHand.enabled = true;
                attackCircle = Managers.PoolManager.ObjPop("BlinkCircle", targetPoint.transform.position);
                CircleManager.Instance.CircleMonsterAdd(boxCollider);
                StartCoroutine(CircleListRemove(boxCollider));
                break;
            case "Attack3End" :
                LeftHand.enabled = false;
                RightHand.enabled = false;
                break;
        }
    }
    public override void ExecutionCheck()
    {
        if (isDie == false)
        {
            if (ExecutionInvoke == false)                                                                          
            {                                                                                                      
                if ((currentHp / maxHp) <= 0.3f)                                                                   
                {                                                                                                  
                    execution = true;                                                                              
                    CircleManager.Instance.ExecuteCircleAdd(boxCollider);                                          
                    CircleManager.Instance.ExecutionAction.Invoke(boxCollider, targetPoint.transform.position);    
                    ExecutionInvoke = true;                                                                        
                }                                                                                                  
            }                                                                                                      
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
            GetComponent<Collider>().enabled = false;
            Memory_Mission_Controller.Instance.Notify(targetID);
            Instantiate(spiltKryll, transform.position, transform.rotation);
            if (CircleManager.Instance.circleMonsterList.Contains(boxCollider))
            {
                CircleManager.Instance.CircleMonsterRemove(boxCollider);
            }
            Destroy(gameObject);
        }
    }
}
