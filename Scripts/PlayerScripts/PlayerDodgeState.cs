using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{
    private static readonly int SPEED = Animator.StringToHash("Speed");
    private Player player;
    private Vector3 dodgeForce;
    private Vector3 force;
    private Vector3 dampingVelocity;
    public PlayerDodgeState(Player player)
    {
        this.player = player;
    }
    public override void Enter()
    {
        player.animStateEventListener.OnOccursAnimStateEvent += EventReceive;
        dodgeForce = player.transform.right * 5f;
        player.isGuardOn = true;
        if (player.onLeftDodge == true)
        {
            player.animator.Play("Dodge_Left");
            Debug.Log("Dodge left");
        }
        else if (player.onRightDodge == true)
        {
            player.animator.Play("Dodge_Right");
            Debug.Log("Dodge Right");

        }
    }

    public override void Execute()
    {
        if (player.stateInfo.normalizedTime >= 1f  && (player.stateInfo.IsName("Dodge_Left") || player.stateInfo.IsName("Dodge_Right")))
        {
            player.ChangeState(new SwordMoveState(player));
        }
        player.animator.SetFloat(SPEED, currentSpeed);
        if (player.onLeftDodge == true)
        {
            force = Vector3.SmoothDamp(-dodgeForce*1f, Vector3.zero, ref dampingVelocity, 0.2f);
            player.cc.Move(force * Time.deltaTime);
        }
        else if (player.onRightDodge == true)
        {
            force = Vector3.SmoothDamp(dodgeForce*1f, Vector3.zero, ref dampingVelocity, 0.2f);
            player.cc.Move(force * Time.deltaTime);

        }
    }

    private void EventReceive(string eventName)
    {
        switch (eventName)
        {
            case "MoveStop_L" :
                dodgeForce = Vector3.zero;
                break;
            case "MoveStop_R" :
                dodgeForce = Vector3.zero;
                break;
        }
    }

    public override void Exit()
    {
        player.isGuardOn = false;
        player.onDodge = false;
        player.onRightDodge = false;
        player.onLeftDodge = false;
        player.animStateEventListener.OnOccursAnimStateEvent -= EventReceive;
    }
}
