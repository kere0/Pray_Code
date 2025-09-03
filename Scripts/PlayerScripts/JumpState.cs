using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : PlayerBaseState
{
    private Player player;
    private float jumpForce = 7.5f;
    //private float jumpDuration = 0.3f;
    private float jumpTimer =0.5f;
    //private bool isJump = false;
    private float jumpStay = 0.1f;

    public JumpState(Player player)
    {
        this.player = player;
    }
    public override void Enter() 
    {
        player.playerState = PlayerState.JUmpState;
        player.animator.Play("InAir");
    }
    public override void Execute()
    {
        Jumping();
        HandleMovement(player);
    }
    private void Jumping()
    {
        jumpTimer -= Time.deltaTime;
        if(jumpTimer < 0f)
        {
            jumpStay -= Time.deltaTime;
            if (jumpStay < 0f)
            {
                player.ChangeState(new FallState(player));
            }
        }
        else
        {
            player.forceReceiver.isGravity = false;
            player.cc.Move(Vector3.up * (jumpForce * Time.deltaTime));
        }
    }
    
    public override void Exit()
    {

    }
}
