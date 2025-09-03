using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHpBar : MonoBehaviour
{
    public static PlayerHpBar Instance;
    private GameObject hpImage;
    List<GameObject> hpImageList = new List<GameObject>();
    public Action SetHPAction;
    void Awake()
    {
        Instance = this;
        hpImage = Resources.Load<GameObject>("HpImage");
        SetHPAction -= SetHP;
        SetHPAction += SetHP;
    }
    public void Init()
    {
        for (int i = 0; i < Player.Instance.currentHp; i++)
        {
            hpImageList.Add(Instantiate(hpImage, transform));
        }
    }
    public void HealHp(int amount)
    {
        float overHealAmount = 0;
        if (Player.Instance.currentHp + amount > Player.Instance.playerData.Hp)
        {
            overHealAmount = (Player.Instance.currentHp + amount) - Player.Instance.playerData.Hp;
        }
        int healAmount = Convert.ToInt32(amount - overHealAmount);
        for (int i = 0; i < healAmount; i++)
        {
            hpImageList.Add(Instantiate(hpImage, transform));
        }
    }
    private void SetHP()
    {
        while (hpImageList.Count > Player.Instance.currentHp)
        {
            int lastIndex = hpImageList.Count - 1;
            Destroy(hpImageList[lastIndex]);
            hpImageList.RemoveAt(lastIndex);
        }
    }
}
