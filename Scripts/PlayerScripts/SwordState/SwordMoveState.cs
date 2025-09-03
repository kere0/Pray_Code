using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordMoveState : PlayerBaseState
{
    public static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int ON_SWORD = Animator.StringToHash("OnSword");
    private static readonly int ON_HAND = Animator.StringToHash("OnHand");

    private Player player;
    private Sword_Weapon sword;
    public SwordMoveState(Player player)
    {
        this.player = player;
    }
    public override void Enter()
    {
        //player.animator.Play("LocoMotion_Sword");
        player.animator.CrossFade("LocoMotion_Sword", 0.1f);
        //player.animator.CrossFadeInFixedTime("LocoMotion_Sword", 0.08f);
        player.playerState = PlayerState.SwordMoveState;
        player.isSword = true;
        //Debug.Log("검 상태");
        player.animator.SetBool("OnSword", true);
        player.animator.SetBool("OnHand", false);
        sword = player.GetComponentInChildren<Sword_Weapon>(true);
        sword.gameObject.SetActive(true);
        // 검들기
    }
    public override void Execute()      
    {
        if (GameManager.Instance.isInputEnabled == true)
        {
            if (player.stateInfo.IsName("LocoMotion_Sword"))
            {
                if (player.stateInfo.normalizedTime >= 0.1f)
                {
                    HandleAnim();
                    if (!player.stateInfo.IsName("Attack02_01"))
                    {
                        HandleMovement(player);
                    }

                    if (Interaction.Instance.isInteractOn == false && MenuController.Instance.isMenuOn == false)
                    {
                        InputeHandler();
                        Dodge();
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
    

    private void InputeHandler()
    {
        // 기본 상태
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.ChangeState(new MoveState(player));
            sword.gameObject.SetActive(false);
        }
        // 검 공격
        if (Input.GetMouseButtonDown(0))
        {
            player.cc.Move(Vector3.zero);
            player.ChangeState(new SwordAtkState(player));
        }
        if (Input.GetMouseButtonDown(1))
        {
            player.cc.Move(Vector3.zero);
            player.ChangeState(new GuardState(player));
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SkillQuickSlot skillQuickSlot = QuickSlotController.Instance.skillQuickSlots[0];
            if(skillQuickSlot == null) return;
            if (skillQuickSlot.isStartCoroutine == false && skillQuickSlot.skillDataSo != null)
            {
                skillQuickSlot.StartCoolTimeCoroutine(skillQuickSlot.skillDataSo.coolTime, skillQuickSlot.skillDataSo.coolTime);
                player.ChangeState(new PlayerSkillState(player, "R"));
            }
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            SkillQuickSlot skillQuickSlot = QuickSlotController.Instance.skillQuickSlots[1];
            if(skillQuickSlot == null) return;
            if (skillQuickSlot.isStartCoroutine == false && skillQuickSlot.skillDataSo != null)
            {
                skillQuickSlot.StartCoolTimeCoroutine(skillQuickSlot.skillDataSo.coolTime, skillQuickSlot.skillDataSo.coolTime);
                player.ChangeState(new PlayerSkillState(player, "T"));
            }
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            SkillQuickSlot skillQuickSlot = QuickSlotController.Instance.skillQuickSlots[2];
            if(skillQuickSlot == null) return;
            if (skillQuickSlot.isStartCoroutine == false && skillQuickSlot.skillDataSo != null)
            {
                skillQuickSlot.StartCoolTimeCoroutine(skillQuickSlot.skillDataSo.coolTime, skillQuickSlot.skillDataSo.coolTime);
                player.ChangeState(new PlayerSkillState(player, "F"));
            }
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            SkillQuickSlot skillQuickSlot = QuickSlotController.Instance.skillQuickSlots[3];
            if(skillQuickSlot == null) return;
            if (skillQuickSlot.isStartCoroutine == false && skillQuickSlot.skillDataSo != null)
            {
                skillQuickSlot.StartCoolTimeCoroutine(skillQuickSlot.skillDataSo.coolTime, skillQuickSlot.skillDataSo.coolTime);
                player.ChangeState(new PlayerSkillState(player, "G"));
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
    private void Dodge()
    {

        if (player.onDodge == false)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                player.onLeftDodge = true;
                player.onDodge = true;
                player.ChangeState(new PlayerDodgeState(player));
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                player.onRightDodge = true;
                player.onDodge = true;
                player.ChangeState(new PlayerDodgeState(player));
            }
        }
    
    }
    private void HandleAnim()
    {
        player.animator.SetFloat(Speed, currentSpeed, 0.1f, Time.deltaTime);
    }
    public override void Exit()
    {
    }
    
}
