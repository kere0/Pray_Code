using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBossHitState : FlameBossBaseState
{
    private FlameBoss flameBoss;

    public FlameBossHitState(FlameBoss flameBoss)
    {
        this.flameBoss = flameBoss;
    }
    public override void Enter()
    {
        flameBoss.animator.Play("Hit");
    }

    public override void Execute()
    {
        flameBoss.cc.Move(flameBoss.forceReceiver.Movement * Time.deltaTime * 60f);
        if (flameBoss.forceReceiver.impact.magnitude <= 0.05f)
        {
            flameBoss.onDamage = false;
            flameBoss.forceReceiver.RemoveForce();
            flameBoss.ChangeState(new FlameBossIdleState(flameBoss));
        }
    }
    public override void Exit()
    {
        
    }
}
