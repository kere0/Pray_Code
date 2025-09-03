using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotController : MonoBehaviour
{
    public static QuickSlotController Instance;
    ItemQuickSlot[] itemQuickSlots;
    public SkillQuickSlot[] skillQuickSlots;

    public Action updateItemAction;
    public Action updateSkillAction;
    public Action useItemAction;
    public Action setItemAction;
    void Awake()
    {
        Instance = this;
        itemQuickSlots = new ItemQuickSlot[3];
        itemQuickSlots[0] = transform.GetChild(0).GetComponent<ItemQuickSlot>();
        itemQuickSlots[1] = transform.GetChild(1).GetComponent<ItemQuickSlot>();
        itemQuickSlots[2] = transform.GetChild(2).GetComponent<ItemQuickSlot>();
        skillQuickSlots = new SkillQuickSlot[4];
        skillQuickSlots[0] = transform.GetChild(3).GetComponent<SkillQuickSlot>();
        skillQuickSlots[1] = transform.GetChild(4).GetComponent<SkillQuickSlot>();
        skillQuickSlots[2] = transform.GetChild(5).GetComponent<SkillQuickSlot>();
        skillQuickSlots[3] = transform.GetChild(6).GetComponent<SkillQuickSlot>();
        updateItemAction -= UpdateItemQuickSlot;
        updateItemAction += UpdateItemQuickSlot;
        useItemAction -= UseItemQuickSlot;
        useItemAction += UseItemQuickSlot;
        updateSkillAction -= UpdateSkillQuickSlot;
        updateSkillAction += UpdateSkillQuickSlot;
    }
    void Update()
    {
        UseItemQuickSlot();
    }
    void UpdateItemQuickSlot() // 아이템 획득할때
    {
        int itemNum = 0;
        foreach (var item in Inventory.Instance.items)
        {
            if (item.Key.itemType != Define.ItemType.EnergyCore)
            {
                if (itemNum < itemQuickSlots.Length)
                {
                    itemQuickSlots[itemNum].itemSo = item.Key;
                    itemNum++;
                }
            }
        }
    }
    void UseItemQuickSlot() // 아이템 사용할때
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ItemSO itemSo = itemQuickSlots[0].itemSo;
            if(itemSo == null) return;
            if (Inventory.Instance.items[itemSo] > 0 && itemQuickSlots[0].isStartCoroutine == false)
            {
                ItemType(itemSo);
                Debug.Log(itemSo.name);
                itemQuickSlots[0].StartCoolTimeCoroutine(itemSo.coolTime, itemSo.coolTime);
                Inventory.Instance.items[itemSo] -= 1;
                setItemAction?.Invoke();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ItemSO itemSo = itemQuickSlots[1].itemSo;
            if(itemSo == null) return;
            if (Inventory.Instance.items[itemSo] > 0 && itemQuickSlots[1].isStartCoroutine == false)
            {
                ItemType(itemSo);
                Debug.Log(itemSo.name);
                itemQuickSlots[1].StartCoolTimeCoroutine(itemSo.coolTime, itemSo.coolTime);
                Inventory.Instance.items[itemSo] -= 1;
                setItemAction?.Invoke();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ItemSO itemSo = itemQuickSlots[2].itemSo;
            if(itemSo == null) return;
            if (Inventory.Instance.items[itemSo] > 0 && itemQuickSlots[2].isStartCoroutine == false)
            {
                ItemType(itemSo);
                Debug.Log(itemSo.name);
                itemQuickSlots[2].StartCoolTimeCoroutine(itemSo.coolTime, itemSo.coolTime);
                Inventory.Instance.items[itemSo] -= 1;
                setItemAction?.Invoke();
            }
        }
    }
    void UpdateSkillQuickSlot() // 스킬 구매할때
    {
        int skillNum = 0;
        foreach (var skill in PlayerSkill.Instance.skillData)
        {
            if (skillNum < skillQuickSlots.Length)
            {
                skillQuickSlots[skillNum].skillDataSo = skill.Value;
                skillNum++;
            }
        }
    }
    void ItemType(ItemSO itemSo)
    {
        if (itemSo.name == "BlueItem")
        {
            PlayerHpBar.Instance.HealHp(9);
            Player.Instance.HpHealing(9);
        }
        else if (itemSo.name == "OrangeItem")
        {
            StartCoroutine(DurationHeal());
        }
    }
    IEnumerator DurationHeal()
    {
        for (int i = 0; i < 5; i++)
        {
            PlayerHpBar.Instance.HealHp(3);
            Player.Instance.HpHealing(3);
            yield return new WaitForSeconds(1f);
        }
    }
}
