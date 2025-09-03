using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData : InterfaceID
{
    public string id;
    public float Hp;
    public int AttackDamage;
    public float AttackSpeed;
    public string ID => id;
}
