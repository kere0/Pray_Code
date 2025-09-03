using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBoosMoveState : BossBaseState
{
    private static readonly int SPEED = Animator.StringToHash("Speed");
    private KnightBoss knightBoss;
    private Vector3 dirMove;
    private float moveSpeed;
    private float walkSpeed =2f;
    private float runSpeed = 6f;
    private float rotateSpeed = 10f;
    public KnightBoosMoveState(KnightBoss knightBoss)
    {
        this.knightBoss = knightBoss;
    }
    public override void Enter()
    {
        moveSpeed = walkSpeed;
        knightBoss.animator.CrossFade("LocoMotion", 0.2f);
    }

    public override void Execute()
    {
        float dist = Vector3.Distance(knightBoss.transform.position, knightBoss.target.position);
        Move();
        if (dist <= 7.0f && dist >=5.0f)
        {
            knightBoss.ChangeState(new KnightBossDashAtkState(knightBoss));
        }
        else if (dist <= 2.0f)
        {
            knightBoss.ChangeState(new KnightBossComboAtkState(knightBoss));
        }

        if (knightBoss.onDamage)
        {
            knightBoss.ChangeState(new KnightBossKnockBackState(knightBoss));
        }
    }

    private void Move()
    {
        // 거리에 따라 속도구현 예정
        if (knightBoss.target != null)
        {
            knightBoss.animator.SetFloat(SPEED, moveSpeed);

            knightBoss.agent.SetDestination(knightBoss.target.position);
            Vector3 direction = (knightBoss.agent.nextPosition - knightBoss.transform.position);
            direction.y = 0;
            
            dirMove = direction.normalized * moveSpeed;
            knightBoss.knightBossController.Move((dirMove + knightBoss.forceReceiver.Movement)  * Time.deltaTime);
            // NaveMesh한테도 nexPosition으로 이동햇다고 알리는 용도
            knightBoss.agent.nextPosition = knightBoss.transform.position;
            if (direction != Vector3.zero)
            {
                knightBoss.transform.rotation = Quaternion.Slerp(knightBoss.transform.rotation,
                    Quaternion.LookRotation(dirMove), rotateSpeed * Time.deltaTime);
            }
        }
    }
    public override void Exit()
    {
        
    }
}
