using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBossDeathState : FlameBossBaseState
{
    private FlameBoss flameBoss;
    public FlameBossDeathState(FlameBoss flameBoss)
    {
        this.flameBoss = flameBoss;
    }
    public override void Enter()
    {
        flameBoss.animator.Play("Death");
        flameBoss.animStateEventListener.OnOccursAnimStateEvent += DeathEventReceive;
        flameBoss.handAttack.enabled = false;
        flameBoss.hammer.enabled = false;
        flameBoss.flameBossBlock.SetActive(false);
        SoundManager.Instance.PlayBgm(Bgm.Chapter1Bgm);
        GameManager.Instance.isBossBattle = false;
    }
    public override void Execute()
    {
    }
    private void DeathEventReceive(string eventName)
    {
        switch (eventName)
        {
            case "OnDeath":
                Destroy(flameBoss.gameObject);
                break;
        }
    }
    public override void Exit()
    {
        flameBoss.animStateEventListener.OnOccursAnimStateEvent -= DeathEventReceive;
    }
}
