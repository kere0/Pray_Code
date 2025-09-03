using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardState : PlayerBaseState
{
    private Player player;

    public GuardState(Player player)
    {
        player.playerState = PlayerState.GuardState;
        this.player = player;
    }
    public override void Enter()
    {
        player.animator.Play("Guard");
        player.isGuardOn = true;
    }

    public override void Execute()
    {
        if (player.stateInfo.IsName("Guard"))
        {
            if (player.onDamage == true)
            {
                player.sword.particle.Play();
                player.ChangeState(new GuardCounterAttack(player));
                
            }
            if (player.stateInfo.normalizedTime >= 1f)
            {
                player.ChangeState(new SwordMoveState(player));
            }
        }
    }

    public override void Exit()
    {
        player.isGuardOn = false;
    }
}
