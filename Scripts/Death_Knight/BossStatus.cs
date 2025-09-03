using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStatus : MonoBehaviour
{
    public float MaxHp { get; private set; }
    public float CurrentHp { get; private set; }

    public bool IsDie { get; private set; }

    public BossStatus(int hp)
    {
        MaxHp = hp;
        CurrentHp = MaxHp;
    }

    public void GetDamage(int damage)
    {
        CurrentHp -= damage;
        CurrentHp = Mathf.Clamp(CurrentHp, 0, MaxHp);
        if (CurrentHp <= 0)
        {
            IsDie = true;
        }
    }
}
