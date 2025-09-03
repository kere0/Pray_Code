using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBossKnockBackState : BossBaseState
{
    private KnightBoss knightBoss;
    public KnightBossKnockBackState(KnightBoss knightBoss)
    {
        this.knightBoss = knightBoss;
    }
    public override void Enter()
    {
        // 피격 애니메이션 추가하기
        knightBoss.animator.Play("Hit");
    }
    public override void Execute()
    {
        //Debug.Log("데미지 들어옴");
        knightBoss.knightBossController.Move(knightBoss.forceReceiver.Movement  * Time.deltaTime);
        if (knightBoss.forceReceiver.impact.magnitude <= 0.05f)
        {
            knightBoss.onDamage = false;
            knightBoss.forceReceiver.RemoveForce();
            knightBoss.ChangeState(new KnightBoosMoveState(knightBoss));
        }
    }
    public override void Exit()
    {
    }
}
