using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamageState : PlayerBaseState
{
    private Player player;
    public PlayerTakeDamageState(Player player)
    {
        this.player = player;
    }
    public override void Enter()
    {
        player.animator.Play("Hit");
    }
    public override void Execute()
    {
        player.cc.Move(player.forceReceiver.Movement);

        if (player.stateInfo.IsName("Hit"))
        {
            if (player.stateInfo.normalizedTime >= 0.9f)
            {
                if (player.isSword == false)
                {
                    player.ChangeState(new MoveState(player));
                }
                else
                {
                    player.ChangeState(new SwordMoveState(player));
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CircleManager.Instance.executionCircleMonsterList.Count != 0)
            {
                player.ChangeState(new PlayerExecuteState(player));
            }
            else if (CircleManager.Instance.circleMonsterList.Count != 0)
            {
                player.ChangeState(new PlayerBlinkSkillState(player));
            }
        }
    }
    public override void Exit()
    {
        player.onDamage = false;
    }
}
