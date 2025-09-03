using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardCounterAttack : PlayerBaseState
{
    private Player player;
    private float time;

    public GuardCounterAttack(Player player)
    {
        this.player = player;
    }
    public override void Enter()
    {
       player.animator.Play("Guard");
       player.animStateEventListener.OnOccursAnimStateEvent += QuardAttackEventReceive;
       player.isGuardOn = true;
       player.isSkillUse = true;
    }

    public override void Execute()
    {
        if (player.stateInfo.IsName("GuardAttack") == false)
        {
            player.forceReceiver.AddForce(-player.transform.forward*Time.deltaTime, 0.1f);
            player.cc.Move(player.forceReceiver.Movement);
        }
        else
        {
            player.forceReceiver.RemoveForce();
        }
        time += Time.deltaTime;
        if (player.stateInfo.IsName("Guard"))
        {
            if (Input.GetMouseButtonDown(0))
            {
                player.animator.Play("GuardAttack");
                GameObject go = Managers.PoolManager.ObjPop("CountAttackSkill",player.transform.position);
                go.transform.rotation = player.transform.rotation;
            }
        }

        if (time >= 0.5f && player.stateInfo.IsName("GuardAttack") == false)
        {
            player.ChangeState(new SwordMoveState(player));

        }
        if (player.stateInfo.IsName("GuardAttack"))
        {
            if (player.stateInfo.normalizedTime >= 1f)
            {
                player.ChangeState(new SwordMoveState(player));
            }
        }
    }

    private void QuardAttackEventReceive(string eventName)
    {
        switch (eventName)
        {
            case "GuardAttackStart" :
                player.sword.GetComponent<Collider>().enabled = true;
                break;
            case "GuardAttackEnd" :
                player.sword.GetComponent<Collider>().enabled = false;
                break;
            case "GuardAttackParticleEnd" :
                break;
        }
    }
    public override void Exit()
    {
        player.animStateEventListener.OnOccursAnimStateEvent -= QuardAttackEventReceive;
        player.onDamage = false;
        player.isGuardOn = false;
        player.isSkillUse = false;
    }
}
