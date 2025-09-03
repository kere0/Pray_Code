using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBossDashAtkState : BossBaseState
{
    private KnightBoss knightBoss;
    private int rand;
    private Vector3 atkDir;
    public KnightBossDashAtkState(KnightBoss knightBoss)
    {
        this.knightBoss = knightBoss;
    }
    public override void Enter()
    {
        knightBoss.animStateEventListener.OnOccursAnimStateEvent += DashEventReceive;
        rand = Random.Range(0, 2);
        switch (rand)
        {
            case 0:
                knightBoss.animator.Play("DashAttackSkill");
                atkDir = (knightBoss.target.transform.position - knightBoss.transform.position);
                atkDir.y = 0f;
                knightBoss.transform.rotation = Quaternion.LookRotation(atkDir);
                knightBoss.forceReceiver.AddForce(atkDir*3f, 0.3f);
                
                if (Vector3.Distance(knightBoss.target.transform.position, knightBoss.transform.position) > 1f)
                {
                    knightBoss.knightBossController.Move(knightBoss.forceReceiver.Movement);
                }
                break;
            case 1:
                knightBoss.animator.Play("DashAttackSkill_2");
                atkDir = (knightBoss.target.transform.position - knightBoss.transform.position);
                atkDir.y = 0f;
                knightBoss.transform.rotation = Quaternion.LookRotation(atkDir);
                knightBoss.forceReceiver.AddForce(atkDir*5f, 0.3f);

                break;
        }
    }

    public override void Execute()
    {
        switch (rand)
        {
            case 0:
                break;
            case 1:
                knightBoss.knightBossController.Move(knightBoss.forceReceiver.Movement  * Time.deltaTime);
                break;
        }

        if (knightBoss.stateInfo.IsName("DashAttackSkill") || knightBoss.stateInfo.IsName("DashAttackSkill_2"))
        {
            if (knightBoss.stateInfo.normalizedTime >= 1f )
            {
                knightBoss.ChangeState(new KnightBoosMoveState(knightBoss));
            }
        }
    }

    private void DashEventReceive(string eventName)
    {
        switch (eventName)
        {
            
            case "K_OnSword_Dash1" :
                knightBoss.KnightBossWeapon.GetComponent<Collider>().enabled = true;
                break;
            case "K_OffSword_Dash1" :
                knightBoss.KnightBossWeapon.GetComponent<Collider>().enabled = false;
                break; 
            case "K_OnSword_Dash2" :
                knightBoss.KnightBossWeapon.GetComponent<Collider>().enabled = true;
                break;
            case "K_OffSword_Dash2" :
                knightBoss.KnightBossWeapon.GetComponent<Collider>().enabled = false;
                break; 
        }
    }
    public override void Exit()
    {
        knightBoss.animStateEventListener.OnOccursAnimStateEvent -= DashEventReceive;
    }
}
