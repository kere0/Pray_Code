using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBossIdleState : FlameBossBaseState
{
    private FlameBoss flameBoss;
    private float attackTime = 2f;
    public FlameBossIdleState(FlameBoss flameBoss)
    {
        this.flameBoss = flameBoss;
    }
    public override void Enter()
    { 
        flameBoss.animator.CrossFade("Idle", 0.2f);
    }
    public override void Execute()
    {
        Collider[] collider = Physics.OverlapSphere(flameBoss.transform.position, 50f, LayerMask.GetMask("Player"));
        if (collider.Length > 0)
        {
            flameBoss.target = collider[0].transform;
        }

        if (flameBoss.target != null)
        {
            flameBoss.ChangeState(new FlameBossAtkState(flameBoss));
        }
    }

    public override void Exit()
    {
        
    }
}
