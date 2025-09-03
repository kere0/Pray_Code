using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IonStorm : MonoBehaviour
{
    private GameObject ionStorm;
    private Material stormEffectMaterial;
    private List<Collider> hitColliders = new List<Collider>();
    private float force = 0.2f;
    private bool isStartCoroutine = false;
    private float particleEndTime = 3.5f;
    private bool countStart = false;
    private Dictionary<SkinnedMeshRenderer, Material> backupMaterials = new Dictionary<SkinnedMeshRenderer, Material>();

    private void Awake()
    {
        stormEffectMaterial = Resources.Load<Material>("ThunderEnemySkinnedMeshEffect");
        ionStorm = transform.parent.parent.parent.gameObject;
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
        particleEndTime = 3.5f;
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
                        monster.GameObject.GetComponent<NormalMonsterBase>().SetMaterial(stormEffectMaterial);
                    }
                    Debug.Log(" 넉백~~~~~~~~~~~~~~~~~~");
                    CombatEvent combatEvent = new CombatEvent();
                    combatEvent.Sender = Player.Instance;
                    combatEvent.Receiver = monster;
                    combatEvent.KnockbackForce = GetKnockbackDirection(other.transform.position) * force;
                    combatEvent.KnockbackDuration = 0.2f;
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
    private Vector3 GetKnockbackDirection(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0f;
        return direction;
    }
    
    private void RestoreMaterials()
    {
        foreach (var pair in backupMaterials)
        {
            if (pair.Key != null)
                pair.Key.material = pair.Value;
        }
        Managers.PoolManager.ObjPush(ionStorm, ionStorm.name);
    }
}