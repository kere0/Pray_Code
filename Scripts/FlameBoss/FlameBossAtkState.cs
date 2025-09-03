using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBossAtkState : FlameBossBaseState
{
    FlameBoss flameBoss;
    private bool isAttacking = false;
    private bool isDashAttacking = false;
    private float attackDelay = 0f;
    private Vector3 atkDir;
    private bool isDashing = false;
    private GameObject attackCircle;
    public FlameBossAtkState(FlameBoss flameBoss)
    {
        this.flameBoss = flameBoss;
    }
    public override void Enter()
    {
        flameBoss.animStateEventListener.OnOccursAnimStateEvent += FlameBossEventReceive;
    }
    private void FlameBossEventReceive(string eventName)
    {
        switch (eventName)
        {
            case "Attack1Start" :
                flameBoss.hammer.hammerCollider.enabled = true;
                attackCircle = Managers.PoolManager.ObjPop("BlinkCircle", flameBoss.targetPoint.transform.position);
                foreach (Collider collider in flameBoss.legColliders)
                {
                    CircleManager.Instance.CircleMonsterAdd(collider);
                }
                flameBoss.CoroutineController(CircleListRemove());
                break;
            case "Attack1End" :
                flameBoss.hammer.hammerCollider.enabled = false;
                break;
            case "DashAttackStart" :
                flameBoss.handAttack.handAttackCollider.enabled = true;
                Debug.Log("대쉬어택~~~~~~~~~~~~~~~");
                break;
            case "DashAttack" :
                isDashing = true;
                flameBoss.CoroutineController(DashCoroutine());
                break;
            case "DashAttackEnd" :
                flameBoss.handAttack.handAttackCollider.enabled = false;

                isDashing = false;
                break;
            case "Skill1Start" :
                GameObject meteor = Managers.PoolManager.ObjPop("Meteor",flameBoss.target.transform.position);    
                meteor.transform.position = flameBoss.target.transform.position;
                break;
            case "Skill2Start" :
                flameBoss.handAttack.handAttackCollider.enabled = true;
                break;
            case "Skill2End" :
                flameBoss.handAttack.handAttackCollider.enabled = false;
                GameObject go = Managers.PoolManager.ObjPop("FireFoot",flameBoss.target.transform.position);    
                go.transform.position = flameBoss.target.transform.position;
                break;    
        }
    }
    public override void Execute()
    {
        attackDelay += Time.deltaTime;
        
        if (attackDelay > 0.5f)
        {
            AttackRange();
        }
        if (flameBoss.animStateInfo.IsName("DashAttack"))
        {
            Vector3 dir = (flameBoss.target.position - flameBoss.transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            flameBoss.transform.rotation = Quaternion.Slerp(flameBoss.transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
    void AttackRange()
    {
        if (isAttacking == true)
        {
            if (flameBoss.animStateInfo.normalizedTime >= 1f)
            {
                flameBoss.ChangeState(new FlameBossIdleState(flameBoss));
                Debug.Log("아이들상태로 돌아감~~~~~~");
            }
        }
        if (isAttacking) return;
        float dis = Vector3.Distance(flameBoss.target.position, flameBoss.transform.position);
        Vector3 dir = (flameBoss.target.position - flameBoss.transform.position).normalized;
        
        // 땅찍기
        if (dis <= 5f)
        {
            isAttacking = true;
            flameBoss.animator.Play("Skill2");
            flameBoss.transform.rotation = Quaternion.LookRotation(dir);
        }
        // 둔기 휘두르기
        else if (dis < 8f && dis > 5f)
        {
            isAttacking = true;
            flameBoss.animator.Play("Attack1");
            flameBoss.transform.rotation = Quaternion.LookRotation(dir);
        }
        else if (dis >= 8 && dis < 13f)
        {
            // 내위치로 오면서 공격
            isDashAttacking = true;
            isAttacking = true;
            flameBoss.animator.Play("DashAttack");
        }
        else if (dis >= 13f && dis < 15f)
        {
            float rand = Random.Range(0, 2);
            if (rand == 0)
            {
                isDashAttacking = true;
                isAttacking = true;
                flameBoss.animator.Play("DashAttack");
            }
            else if (rand == 1)
            {
                isAttacking = true;
                flameBoss.animator.Play("Skill1");
            }
        }
        else if (dis >= 15)
        {
            isAttacking = true;
            flameBoss.animator.Play("Skill1");
        }
    }
    public override void Exit()
    {
        flameBoss.animStateEventListener.OnOccursAnimStateEvent -= FlameBossEventReceive;
        isAttacking = false;
        isDashAttacking = false;
        attackDelay = 0f;
        flameBoss.hammer.hammerCollider.enabled = false;
        flameBoss.handAttack.handAttackCollider.enabled = false;
    }
    IEnumerator DashCoroutine()
    {
        atkDir = (flameBoss.target.transform.position - flameBoss.transform.position);
        atkDir.y = 0f;
        while (isDashing)
        {
            if (Vector3.Distance(flameBoss.target.transform.position, flameBoss.transform.position) > 3f)
            {
                flameBoss.cc.Move(atkDir * 3f*Time.deltaTime);
            }
            else
            {
                isDashing = false;
                flameBoss.forceReceiver.RemoveForce();
                flameBoss.cc.Move(Vector3.zero);
            }
            yield return null;
        }
    }
    public IEnumerator CircleListRemove()
    {
        yield return new WaitForSeconds(0.4f);
        foreach (Collider collider in flameBoss.colliders)
        {
            CircleManager.Instance.CircleMonsterRemove(collider);
        }
    }
}
