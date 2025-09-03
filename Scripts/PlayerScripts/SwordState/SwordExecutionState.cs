using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordExecutionState : PlayerBaseState
{
    private Player player;
    private float jumpTimer = 1f;
    private float jumpStay = 0.1f;
    public SwordExecutionState(Player player)
    {
        this.player = player;
    }
    public override void Enter()
    {
        player.animator.Play("FinishSkill");
    }

    public override void Execute()
    {
        if (player.stateInfo.normalizedTime >= 1f)
        {
            player.ChangeState(new SwordMoveState(player));
        }
        jumpTimer -= Time.deltaTime;
        if(jumpTimer < 0f)
        {   
            jumpStay -= Time.deltaTime;
            if (jumpStay < 0f)
            {
                player.forceReceiver.isGravity = true;
                player.cc.Move(player.forceReceiver.Movement* Time.deltaTime);
            }
        }
        else
        {
            player.forceReceiver.isGravity = false;
            player.cc.Move(Vector3.up * (5f * Time.deltaTime));
        }
    }

    public override void Exit()
    {
        
    }
}
