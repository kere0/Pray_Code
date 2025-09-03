using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using Unity.VisualScripting;
using UnityEngine;

public class SwordAtkState : PlayerBaseState
{
    private static readonly int ON_ATTACK = Animator.StringToHash("OnAttack");
    private Player player;
    private float comboCount;
    private bool isAttacking = false;
    public SwordAtkState(Player player)
    {
        this.player = player;
    }
    public override void Enter()
    {
        player.animStateEventListener.OnOccursAnimStateEvent += EventReceive;
        player.playerState = PlayerState.SwordAttackState;
        player.animator.Play("Attack02_1");
        comboCount++;
    }

    public override void Execute()
    {
        isAttacking = player.stateInfo.IsName("Attack02_1") || player.stateInfo.IsName("Attack02_2")
                                                            || player.stateInfo.IsName("Attack02_3")
                                                            || player.stateInfo.IsName("Attack02_4");
        OnAnimatorMove();
        if (isAttacking == true)
        {
            if (player.stateInfo.normalizedTime >= 0.9f)
            {
                player.ChangeState(new SwordMoveState(player));
            }
        }
        ComboAttack();
        if (CameraController.Instance.isLockOn == true)
        {
            if (player.target != null)
            {
                Vector3 direction = (player.target.position - player.transform.position).normalized;
                direction.y = 0;
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime*5f);
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

    private void ComboAttack()
    {
        if (player.stateInfo.normalizedTime >= 0.1f && comboCount != 4)
        {
            if (Input.GetMouseButtonDown(0))
            {
                comboCount++;
                player.animator.Play("Attack02_" + $"{comboCount.ToString()}");
            }
            
        }
        if (comboCount == 4 && player.stateInfo.normalizedTime >= 0.7f)
        {
            player.ChangeState(new SwordMoveState(player));
        }
    }
    private void EventReceive(string eventName)
    {
        switch (eventName)
        {
            case "P_StartAttack1" :
                player.sword.GetComponent<Collider>().enabled = true;
                break;
            case "P_EndAttack1" :
                player.sword.GetComponent<Collider>().enabled = false;
                break; 
            case "P_StartAttack2" :
                player.sword.GetComponent<Collider>().enabled = true;
                break;
            case "P_EndAttack2" :
                player.sword.GetComponent<Collider>().enabled = false;
                break; 
            case "P_StartAttack3" :
                player.sword.GetComponent<Collider>().enabled = true;
                break;
            case "P_EndAttack3" :
                player.sword.GetComponent<Collider>().enabled = false;
                break; 
            case "P_StartAttack4" :
                player.sword.GetComponent<Collider>().enabled = true;
                break;
            case "P_EndAttack4" :
                player.sword.GetComponent<Collider>().enabled = false;
                break; 
        }
    }
    public override void Exit()
    {
        comboCount = 0;
        player.animStateEventListener.OnOccursAnimStateEvent -= EventReceive;
        player.sword.GetComponent<Collider>().enabled = false;
    }
    void OnAnimatorMove()
    {
            player.cc.Move(player.animator.deltaPosition);
    }
}
