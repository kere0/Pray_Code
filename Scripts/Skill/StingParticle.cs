using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StingParticle : MonoBehaviour
{
    private GameObject stingParticle;
    private float force = 15f;
    private bool isStartCoroutine = false;
    private float particleEndTime = 3.5f;
    private bool countStart = false;
    private Collider currentCollider;

    private void Awake()
    {
        stingParticle = transform.parent.parent.parent.gameObject;
    }

    private void OnEnable()
    {
        countStart = true;
    }

    private void OnDisable()
    {
        countStart = false;
        particleEndTime = 3.5f;
        currentCollider = null;
    }

    private void Update()
    {
        if (countStart == true)
        {
            particleEndTime -= Time.deltaTime;
        }
        if (particleEndTime <= 0)
        {
            Managers.PoolManager.ObjPush(stingParticle, stingParticle.name);
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Monster") == true)
        {
            Collider collider = other.GetComponent<Collider>();
            if (collider == currentCollider) return;
            if (collider != null)
            {
                IDamageAble monster = CombatSystem.Instance.GetCreatureOrNull(collider);
                if (monster != null)
                {
                    currentCollider = collider;
                    SkinnedMeshRenderer[] renderers = other.GetComponentsInChildren<SkinnedMeshRenderer>();
                    CombatEvent combatEvent = new CombatEvent();
                    combatEvent.Sender = Player.Instance;
                    combatEvent.Receiver = monster;
                    combatEvent.KnockbackDuration = 0f;
                    combatEvent.Damage = 100;
                    combatEvent.UseEffect = true;
                    combatEvent.EffectName = "Blood12";
                    combatEvent.EffectPosition =
                        monster.MeshParts.GetClosestMeshSurface(collider.ClosestPoint(transform.position));
                    combatEvent.HitNormal = monster.MeshParts.dir;
                    PlayerHpBar.Instance.HealHp(3);
                    Player.Instance.HpHealing(3);
                    CombatSystem.Instance.AddCombatEvent(combatEvent);
                }
            }
            
        }
    }
}