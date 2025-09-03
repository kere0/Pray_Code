using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillState : PlayerBaseState
{
    private Player player;
    private string currentButton;
    private SkillDataSO skillDataSo;
    private bool skillOn = false;
    private GameObject go;
    private float handSkillCount;
    
    public PlayerSkillState(Player player, string button)
    {
        this.player = player;
        currentButton = button;
    }
    public override void Enter()
    {
        player.animStateEventListener.OnOccursAnimStateEvent += SkillEventReceive;
        player.staggerResistance = true;
        SkillCheck();
    }
    void SkillEventReceive(string eventName)
    {
        switch (eventName)
        {
            // PlasmaSurge
            case "PlasmaSurgeStart" :
                go = Managers.PoolManager.ObjPop(skillDataSo.effectKey, player.transform.position);
                go.transform.rotation = player.transform.rotation;
                break;
            case "PlasmaSurgeEnd" :
                StateChangeCheck();
                break;
            // DimensionReaper
            case "DimensionReaperStart" :
                    go = Managers.PoolManager.ObjPop(skillDataSo.effectKey, player.handSkillPoint.position);
                    go.transform.rotation = player.transform.rotation;
                    if (GameManager.Instance.isEndSceneLoad == true)
                    {
                        go.transform.localScale = new Vector3(5, 5, 5);
                    }
                    player.CoroutineController(HandSkillCoroutine());
                break;
            case "DimensionReaperEnd" :
                StateChangeCheck();
                break;
            // VolcanicBurst
            case "VolcanicBurstStart" :
                go = Managers.PoolManager.ObjPop(skillDataSo.effectKey, player.transform.position + player.transform.forward * 2f);
                go.transform.rotation = player.transform.rotation;
                break;
            case "VolcanicBurstEnd":
                StateChangeCheck();
                break;
            // IonStorm
            case "IonStormStart" :
                go = Managers.PoolManager.ObjPop(skillDataSo.effectKey, player.transform.position + player.transform.forward * 5f);
                go.transform.rotation = player.transform.rotation;
                
                break;
            case "IonStormEnd" :
                StateChangeCheck();
                break;
             
        }
    }
    public override void Execute()
    {
    }

    void SkillCheck()
    {
        switch (currentButton)
        {
            //1
            case "R":
                skillDataSo = QuickSlotController.Instance.skillQuickSlots[0].skillDataSo;
                player.animator.Play(skillDataSo.effectKey);
                break;
            //2
            case "T":
                skillDataSo = QuickSlotController.Instance.skillQuickSlots[1].skillDataSo;
                player.animator.Play(skillDataSo.effectKey);

                break;
            //3
            case "F" :
                skillDataSo = QuickSlotController.Instance.skillQuickSlots[2].skillDataSo;
                player.animator.Play(skillDataSo.effectKey);
                break;
            //4
            case "G" :
                skillDataSo = QuickSlotController.Instance.skillQuickSlots[3].skillDataSo;
                player.animator.Play(skillDataSo.effectKey);
                break;
        }
    }
    void StateChangeCheck()
    {
        if (player.isSword == true)
        {
            player.ChangeState(new SwordMoveState(player));
        }
        else if (player.isSword == false)
        {
            player.ChangeState(new MoveState(player));
        }
    }
    public override void Exit()
    {
        player.animStateEventListener.OnOccursAnimStateEvent -= SkillEventReceive;
        player.staggerResistance = false;
    }

    IEnumerator HandSkillCoroutine()
    {
        while (handSkillCount <= 0.01f)
        {
            handSkillCount += Time.deltaTime;
            go.transform.position += player.transform.forward;
            yield return null;
        }
        go.GetComponent<DimensionReaper>().portalOn = true;
    }
    IEnumerator EffectTimeCheckCoroutine()
    {
        while (true)
        {
            if (go == null) yield break;
            int effectEnd = 0;
            ParticleSystem[] patricle = go.GetComponentsInChildren<ParticleSystem>();
            foreach (var p in patricle)
            {
                if (p.IsAlive(true))
                {
                    effectEnd++;
                }
            }
            if (effectEnd == 0) break;
            yield return null;
        }
        skillOn = false;
        Managers.PoolManager.ObjPush(go, skillDataSo.effectKey);
    }
}
