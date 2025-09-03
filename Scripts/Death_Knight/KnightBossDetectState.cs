using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBossDetectState : BossBaseState
{
    private KnightBoss knightBoss;
   
    public KnightBossDetectState(KnightBoss knightBoss)
    {
        this.knightBoss = knightBoss;
    }
    public override void Enter()
    {
    }
    public override void Execute()
    {
        if (knightBoss.KnightBossCutSceneController.knightBossActive == true)
        {
            Collider[] colliders = Physics.OverlapSphere(knightBoss.transform.position, 50f, LayerMask.GetMask("Player"));
            if (colliders.Length > 0)
            {
                knightBoss.target = colliders[0].transform;
            }
            if (knightBoss.target != null)
            {
                Debug.Log(knightBoss.target.name);
                knightBoss.ChangeState(new KnightBoosMoveState(knightBoss));
            }
        }
    }
    public override void Exit()
    {
    }
}
