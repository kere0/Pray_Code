using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEvent : MonoBehaviour
{
    public IDamageAble Sender { get; set; }
    public IDamageAble Receiver { get; set; }
    public int Damage;
    public bool isGuardIgnore = false;
    public Vector3 HitPosition;
    public Vector3 HitNormal;
    public Collider colider;
    public Vector3 KnockbackForce; // 어느정도의 힘으로 넉백할지 방향까지 해서 넘겨주자
    public float KnockbackDuration;
    public bool UseEffect;
    public Vector3 EffectPosition;
    public Vector3 EffectRotation;
    public string EffectName;
}
