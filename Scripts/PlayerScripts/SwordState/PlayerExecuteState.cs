using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerExecuteState : PlayerBaseState
{
    private Player player;
    private Transform executeTarget;
    private Vector3 colTransform;
    public PlayerExecuteState(Player player)
    {
        this.player = player;
    }
    public override void Enter()
    {
        player.animStateEventListener.OnOccursAnimStateEvent += ExecuteEventListener;
        Debug.Log("처형 들어옴");
        player.isGuardOn = true;
        player.isSkillUse = true;
        Time.timeScale = 0.3f;
        Collider col;
        if (player.target != null)
        {
            executeTarget = player.target;
            col = player.target.GetComponent<Collider>();
        }
        else
        {
            Debug.Log("처형 들어와서 가져옴");
            
                col = CircleManager.Instance.GetClosestExecuteMonster();
                if (col == null)
                {
                    player.ChangeState(new SwordMoveState(player));
                }
                else
                {
                    executeTarget = col.transform;

                }
        }
        Vector3 lookDir = (executeTarget.position - player.transform.position).normalized;
        lookDir.y = 0f;
        if (lookDir != Vector3.zero)
        {
            player.transform.rotation = Quaternion.LookRotation(lookDir);
        }
        colTransform = col.transform.position;
        player.animator.Play("Sting");
        Managers.PoolManager.ObjPop("StingParticle", colTransform);
        CircleManager.Instance.executionCircleMonsterList.Remove(col);
    }
    public override void Execute()
    {
    }
    private void ExecuteEventListener(string eventName)
    {
        switch (eventName)
        {
            case "StingColliderOn":
                Debug.Log("콜라이더 켜짐");
                Time.timeScale = 1f;
                player.sword.GetComponent<Collider>().enabled = true;
                break;
            case "StingColliderOff":
                player.sword.GetComponent<Collider>().enabled = false;
                break;
            case "StingEnd" :
                Debug.Log("처형모션 끝");
                player.ChangeState(new SwordMoveState(player));
                break;
        }
    }

    public override void Exit()
    {
        player.animStateEventListener.OnOccursAnimStateEvent -= ExecuteEventListener;
        colTransform = Vector3.zero;
        Time.timeScale = 1f;
        player.isGuardOn = false;
        player.isSkillUse = false;
    }
}
