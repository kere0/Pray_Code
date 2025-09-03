using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBossComboAtkState : BossBaseState
{
    private  KnightBoss knightBoss;
    private bool attackCircle = false;
    public KnightBossComboAtkState(KnightBoss knightBoss)
    {
        this.knightBoss = knightBoss;
    }
    public override void Enter()
    {
        knightBoss.animStateEventListener.OnOccursAnimStateEvent += EventReceive;
        knightBoss.animator.Play("Attack_3Combo");
    }

    public override void Execute()
    {
        if (knightBoss.stateInfo.normalizedTime >= 1f )
        {
            knightBoss.ChangeState(new KnightBoosMoveState(knightBoss));
            //Debug.Log("끝");
        }
        if (knightBoss.onDamage)
        {
            knightBoss.ChangeState(new KnightBossKnockBackState(knightBoss));
        }
    }
    private void EventReceive(string eventName)
    {
        switch (eventName)
        {
            case "K_StartAttack1" :
                attackCircle = Managers.PoolManager.ObjPop("BlinkCircle", knightBoss.targetPoint.transform.position);
                CircleManager.Instance.CircleMonsterAdd(knightBoss.knightBossController);
                knightBoss.CoroutineController(CircleListRemove(knightBoss.knightBossController));
                knightBoss.KnightBossWeapon.GetComponent<Collider>().enabled = true;
                break;
            case "K_EedAttack1" :
                knightBoss.KnightBossWeapon.GetComponent<Collider>().enabled = false;
                break; 
            case "K_StartAttack2" :
                knightBoss.KnightBossWeapon.GetComponent<Collider>().enabled = true;
                break;
            case "K_EedAttack2" :
                knightBoss.KnightBossWeapon.GetComponent<Collider>().enabled = false;
                break; 
            case "K_StartAttack3" :
                knightBoss.KnightBossWeapon.GetComponent<Collider>().enabled = true;
                break;
            case "K_EedAttack3" :
                knightBoss.KnightBossWeapon.GetComponent<Collider>().enabled = false;
                break; 
        }
    }
    public IEnumerator CircleListRemove(Collider collider)
    {
        yield return new WaitForSeconds(0.4f);
        CircleManager.Instance.CircleMonsterRemove(collider);
    }
    public override void Exit()
    {
        knightBoss.animStateEventListener.OnOccursAnimStateEvent -= EventReceive;
    }
}
