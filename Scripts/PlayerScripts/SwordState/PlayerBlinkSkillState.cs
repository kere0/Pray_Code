using System.Collections;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;

public class PlayerBlinkSkillState : PlayerBaseState
{
    private Player player;
    private Vector3 blinkTargetPos;
    private Transform blinkTarget;
    private Vector3 dashTarget;
    private float blinkBuffer = 1f;
    private float dashSpeed = 10f;
    private float dashDuration = 1f;
    private float elapsedTime = 0f;

    private bool isDashing = false;
    Vector3 dampingVelocity;
    private bool isDashEnd = false;

    public PlayerBlinkSkillState(Player player)
    {
        this.player = player;
    }

    public override void Enter()
    {
        player.blinkParticle.Play();
        player.isGuardOn = true;
        player.isSkillUse = true;
        Time.timeScale = 0.1f;

        player.animStateEventListener.OnOccursAnimStateEvent += DashEventListener;


        float distanceBehind = 0;
        Collider col;

        if (player.target != null)
        {
            blinkTarget = player.target;
            col = player.target.GetComponent<Collider>();
        }
        else
        {
            blinkTarget = CircleManager.Instance.GetClosestMonster().transform;
            col = CircleManager.Instance.GetClosestMonster();
        }
        // 몸에 안박히게 하기위해서
        distanceBehind = col.bounds.extents.z + blinkBuffer;

        blinkTargetPos = blinkTarget.position + -blinkTarget.forward * distanceBehind*2f;
        blinkTargetPos.y = 0f;
        

        // 순간이동 
        player.cc.enabled = false;
        player.transform.position = blinkTargetPos;
        player.cc.enabled = true;

        // 돌진
        dashTarget = blinkTarget.position;
        dashTarget.y = 0f;
        elapsedTime = 0f;
        isDashing = true;
        if (CircleManager.Instance.circleMonsterList.Contains(col))
        {
            CircleManager.Instance.CircleMonsterRemove(col);
        }
    }

    public override void Execute()
    {
        if (!isDashing) return;
        
        player.animator.Play("Blink");
        elapsedTime += Time.unscaledDeltaTime;
        if (elapsedTime > 0.3f && elapsedTime <= 0.5f)
        {
            Time.timeScale = 0.5f;
        }
        else if (elapsedTime > 0.5f)
        {
            Time.timeScale = 1f;
        }
        if (dashDuration < elapsedTime)
        {
            isDashing = false;
            player.ChangeState(new SwordMoveState(player));
            return;
        }

        if (isDashEnd == false)
        {
            Vector3 nextPos = Vector3.MoveTowards(player.transform.position, dashTarget, dashSpeed * Time.deltaTime);
            Vector3 moveDir = nextPos - player.transform.position;
            player.cc.Move(moveDir);
        }

        if (blinkTarget != null)
        {
            Vector3 lookDir = (blinkTarget.position - player.transform.position).normalized;
            lookDir.y = 0f;
            if (lookDir != Vector3.zero)
            {
                player.transform.rotation = Quaternion.LookRotation(lookDir);
            }
        }
    }
    private void DashEventListener(string eventName)
    {
        switch (eventName)
        {
            case "DashEnd" :
                isDashEnd = true;
                break;
            case "BlinkColliderOn" :
                player.sword.GetComponent<Collider>().enabled = true;
                break;
            case "BlinkColliderOff" :
                player.sword.GetComponent<Collider>().enabled = false;
                break;
        }
    }
    public override void Exit()
    {
        player.animStateEventListener.OnOccursAnimStateEvent -= DashEventListener;
        player.sword.GetComponent<Collider>().enabled = false;
        player.isGuardOn = false;
        player.isSkillUse = false;
        isDashing = false;
        isDashing = false;
        blinkTarget  = null;
        Time.timeScale = 1f;
    }
}