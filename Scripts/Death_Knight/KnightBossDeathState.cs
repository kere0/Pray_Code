using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBossDeathState : BossBaseState
{
    KnightBoss knightBoss;
    private float jumpTimer = 0.5f;
    private float jumpStay = 0.1f;

    public KnightBossDeathState(KnightBoss knightBoss)
    {
        this.knightBoss = knightBoss;
    }
    public override void Enter()
    {
        knightBoss.animator.Play("Die");
        knightBoss.KnightBossWeapon.enabled = false;
        knightBoss.knightBossBlock.SetActive(false);
        SoundManager.Instance.PlayBgm(Bgm.Chapter2Bgm);
        GameManager.Instance.isBossBattle = false;
        knightBoss.knightBossBlock2.SetActive(false);
    }

    public override void Execute()
    {
        jumpTimer -= Time.deltaTime;
        if(jumpTimer < 0f)
        {   
            jumpStay -= Time.deltaTime;
            if (jumpStay < 0f)
            {
                knightBoss.forceReceiver.isGravity = true;
                knightBoss.knightBossController.Move(knightBoss.forceReceiver.Movement* Time.deltaTime);
            }
        }
        else
        {
            knightBoss.forceReceiver.isGravity = false;
            knightBoss.knightBossController.Move(Vector3.up * (5f * Time.deltaTime));
        }
    }
    public override void Exit()
    {
        
    }
}
