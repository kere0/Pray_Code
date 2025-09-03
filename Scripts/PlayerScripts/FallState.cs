using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : PlayerBaseState
{
    private Player player;
    public FallState(Player player)
    {
        this.player = player;
    }
    public override void Enter() 
    {
        player.playerState = PlayerState.FallState;
        player.animator.Play("InAir");

        //player.animator.SetBool("IsFall", true);
    }
    public override void Execute() 
    { 
        HandleMovement(player);
        if (player.forceReceiver.isGravity != true)
        {
            player.forceReceiver.isGravity = true;
        }
        if(player.cc.isGrounded == true)
        {
            //player.animator.SetBool("IsFall", false);
            player.animator.Play("JumpLand");
            if (player.stateInfo.normalizedTime >= 1f)
            {
                // 수정하기
                if (player.isSword == false)
                {
                    player.ChangeState(new MoveState(player));    
                }
                else if (player.isSword == true)
                {
                    player.ChangeState(new SwordMoveState(player));
                }
            }
           
        }
    }
    public override void Exit() 
    {
     
    }
}
