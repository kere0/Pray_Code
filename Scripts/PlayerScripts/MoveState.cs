using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : PlayerBaseState
{
    public static readonly int Speed = Animator.StringToHash("Speed");

    private Player player;
    public MoveState(Player player)
    {
        this.player = player;
    }
    public override void Enter()
    {
        player.animator.CrossFade("LocoMotion", 0.1f);
        player.playerState = PlayerState.MoveState;
    }
    public override void Execute()
    {
        if (GameManager.Instance.isInputEnabled == true)
        {
            if (player.stateInfo.IsName("LocoMotion"))
            {
                if (player.stateInfo.normalizedTime >= 0.1f)
                {
                    player.playerState = PlayerState.MoveState;
                    player.isSword = false;
                    HandleMovement(player);
                    HandleAnim();
                    if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        player.ChangeState(new SwordMoveState(player));
                    }
                }
            }
        }
        else
        {
            if (EndSceneOpen.Instance != null)
            {
                if (EndSceneOpen.Instance.isEndSceneTrigger == false)
                {
                    axisInput = Vector2.zero;
                    player.cc.Move(Vector3.zero);
                    currentSpeed = 0f;
                    HandleAnim();
                }
            }
            else
            {
                axisInput = Vector2.zero;
                player.cc.Move(Vector3.zero);
                currentSpeed = 0f;
                HandleAnim();
            }
        }
    }
    private void HandleAnim()
    {
        player.animator.SetFloat(Speed, currentSpeed, 0.1f, Time.deltaTime);
    }
    public override void Exit() { }
}
