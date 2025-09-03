using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Weapon : MonoBehaviour
{
    // hand_r 에다가 붙힘
    private float force = 0.2f;
    public ParticleSystem particle;
    public GameObject thunderParticle;

    private void Awake()
    {
        particle = transform.GetChild(3).GetComponent<ParticleSystem>();
        thunderParticle = transform.GetChild(2).gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            IDamageAble monster = CombatSystem.Instance.GetCreatureOrNull(other);    
            if (monster != null)
            {
                CombatEvent combatEvent = new CombatEvent();
                combatEvent.Sender = Player.Instance;
                combatEvent.Receiver = monster;
                // 여기부턴 그때그때 다름
                Vector3 KnockBackDir = (other.transform.position - Player.Instance.transform.position).normalized;
                KnockBackDir.y = 0f;
                combatEvent.KnockbackForce = KnockBackDir * force;
                combatEvent.KnockbackDuration = 0.3f;
                combatEvent.Damage = Player.Instance.playerData.AttackDamage;
                combatEvent.UseEffect = true;
                combatEvent.EffectName = "Blood12";
                combatEvent.EffectPosition = monster.MeshParts.GetClosestMeshSurface(other.ClosestPoint(transform.position));
                combatEvent.HitNormal = monster.MeshParts.dir;
                CombatSystem.Instance.AddCombatEvent(combatEvent);
            }
        }
    }
}
