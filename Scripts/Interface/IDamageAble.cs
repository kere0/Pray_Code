using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageAble
{
    public GameObject GameObject { get; }
    public MeshParts MeshParts { get; }
    public TargetID TargetID { get; }
    public Collider Collider { get; }
    public GameObject TargetPoint { get; }
    public float Hp { get; }
    public bool IsDie { get; }
    public bool NormalMonster { get; }
    public Define.CreatureType CreatureType { get; }
    public bool ExecutionOn { get; }
    public Dictionary<SkinnedMeshRenderer, Material> OriginMaterial { get; }
    public void TakeDamage(int combatEvent, bool OnDamage, bool isGuardIgnore);
    public void TakeKnockback(Vector3 KnockbackForce, float duration);
}
