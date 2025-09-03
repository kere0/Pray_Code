using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanicBurst : MonoBehaviour
{
    private GameObject volcanicBurst;
    private Material volcanicBurstEffectMaterial;
    private List<Collider> hitColliders = new List<Collider>();
    private float force = 5f;
    private bool isStartCoroutine = false;
    private float particleEndTime = 8f;
    private bool countStart = false;
    private Dictionary<SkinnedMeshRenderer, Material> backupMaterials = new Dictionary<SkinnedMeshRenderer, Material>();

    private void Awake()
    {
        volcanicBurstEffectMaterial = Resources.Load<Material>("FireEffectMaterial");
        volcanicBurst = transform.parent.parent.parent.gameObject;
    }
    private void OnEnable()
    {
        countStart = true;
    }
    private void OnDisable()
    {
        backupMaterials.Clear();
        hitColliders.Clear();
        countStart = false;
        particleEndTime = 8f;
    }
    private void Update()
    {
        if (countStart == true)
        {
            particleEndTime -= Time.deltaTime;
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Monster") == true)
        {
            Collider collider = other.GetComponent<Collider>();
            if (collider != null && hitColliders.Contains(collider) == false)
            {
                hitColliders.Add(collider);
                IDamageAble monster = CombatSystem.Instance.GetCreatureOrNull(collider);
                if (monster != null)
                {
                    if (monster.CreatureType == Define.CreatureType.Normal)
                    {
                        monster.GameObject.GetComponent<NormalMonsterBase>().SetMaterial(volcanicBurstEffectMaterial);
                    }
                    Debug.Log(" 넉백~~~~~~~~~~~~~~~~~~");
                    CombatEvent combatEvent = new CombatEvent();
                    combatEvent.Sender = Player.Instance;
                    combatEvent.Receiver = monster;
                    combatEvent.KnockbackForce = Vector3.up * force;
                    combatEvent.KnockbackDuration = 0.3f;
                    combatEvent.Damage = 15;
                    combatEvent.UseEffect = true;
                    combatEvent.EffectName = "Blood12";
                    combatEvent.EffectPosition =
                        monster.MeshParts.GetClosestMeshSurface(collider.ClosestPoint(transform.position));
                    combatEvent.HitNormal = monster.MeshParts.dir;
                    CombatSystem.Instance.AddCombatEvent(combatEvent);
                }
            }
        }
    }
    private void RestoreMaterials()
    {
        foreach (var pair in backupMaterials)
        {
            if (pair.Key != null)
                pair.Key.material = pair.Value;
        }
        Managers.PoolManager.ObjPush(volcanicBurst, volcanicBurst.name);
    }
}