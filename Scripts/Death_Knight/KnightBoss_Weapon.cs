using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBoss_Weapon : MonoBehaviour
{
    private float force = 0.1f;
    public KnightBoss knightBoss;
    private Collider collider;

    private void Awake()
    {
        TryGetComponent(out collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IDamageAble player = CombatSystem.Instance.GetCreatureOrNull(other);
            
            if (player != null)
            {
                collider.enabled = false;
                CombatEvent combatEvent = new CombatEvent();
                combatEvent.Sender = knightBoss;
                combatEvent.Receiver = player;
                // 여기부턴 그때그때 다름 // bossStatus에서 값받아서 하기, 스킬과 공격에 따라서 Damage 다르게
                Vector3 PlayerBackDir = (other.transform.position - transform.position).normalized;
                PlayerBackDir.y = 0f;
                combatEvent.KnockbackForce = PlayerBackDir * force;
                combatEvent.KnockbackDuration = 0.1f; // 0.3f
                combatEvent.Damage = 6;
                if (Player.Instance.isGuardOn == false)
                {
                    combatEvent.UseEffect = true;   
                }
                combatEvent.EffectName = "Blood12";
                combatEvent.EffectPosition = other.gameObject.GetComponent<IDamageAble>().MeshParts
                    .GetClosestMeshSurface(other.ClosestPoint(transform.position));
                combatEvent.HitNormal = other.gameObject.GetComponent<IDamageAble>().MeshParts.dir;
                Debug.Log(other);
                CombatSystem.Instance.AddCombatEvent(combatEvent);
            }
        }
    }
}
