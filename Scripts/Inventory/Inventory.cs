using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class Inventory : MonoBehaviour, ISaveable
{
    public static Inventory Instance;
    public Dictionary<ItemSO, int> items = new Dictionary<ItemSO, int>();
    public InventorySlot[] slots;
    public int myGold;
    public InventoryData inventoryData;
    private void Start()
    {
        Instance = this;
        MenuController.onBag -= UpdateSlots;
        MenuController.onBag += UpdateSlots;
        RegisterToSaveManager();
        LoadData();
    }
    public void AddItem(ItemSO itemSo, int num = 1)
    {
        if (items.ContainsKey(itemSo))
        {
            items[itemSo] += num;
        }
        else
        {
            items.Add(itemSo, num);
            QuickSlotController.Instance.updateItemAction?.Invoke();
        }
    }
    public void UpdateSlots()
    {
        int itemNum = 0;
        foreach (var item in items)
        {
            if (itemNum < slots.Length)
            {
                slots[itemNum].AddItem(item.Key, item.Value);
                itemNum++;
            }
        }
        // 다 사용한 아이템 클리어
        for (; itemNum < slots.Length; itemNum++)
        {
            slots[itemNum].ClearSlot();
        }
        QuickSlotController.Instance.useItemAction?.Invoke();
    }
    public void SaveData()
    {
        SaveSnapshot.Instance.saveData.money = myGold;
        Dictionary<string, int> tempDict = new Dictionary<string, int>(items.Count);
        foreach (var item in items)
        {
            ItemSO itemSo = item.Key;
            tempDict[itemSo.soName] = item.Value;
        }
        Utils.SerializableDict<string, int> serializableDict = new Utils.SerializableDict<string, int>(tempDict);
        SaveSnapshot.Instance.saveData.itemData = serializableDict;
    }
    public void LoadData()
    {
        switch (GameManager.SelectMode)
        {
            case Define.SelectMode.NewGame:
                inventoryData = Managers.DataManager.Get<InventoryData>("I01");
                myGold = inventoryData.Money;
                break;
            case Define.SelectMode.LoadGame:
                if (File.Exists(Managers.SaveManager.SavePath) == false)
                {
                    Debug.Log($"세이브 파일이 없음: {Managers.SaveManager.SavePath}");
                    inventoryData = Managers.DataManager.Get<InventoryData>("I01");
                    myGold = inventoryData.Money;
                    return;
                }
                Dictionary<string, int> itemdict = SaveSnapshot.Instance.saveData.itemData.ToDictionary();
                if (itemdict.Count != 0)
                {
                    foreach (var item in itemdict)
                    {
                        Debug.Log(item.Key + " 아이템 키~~~~~~");
                        items[Resources.Load<ItemSO>($"ScriptableObjects/Item/{item.Key}")] = item.Value;
                    }
                    QuickSlotController.Instance.updateItemAction?.Invoke();
                }
                myGold = SaveSnapshot.Instance.saveData.money;
                break;
        }
    }
    public void RegisterToSaveManager()
    {
        Managers.SaveManager.Register(this);
    }

    public void Clear()
    {
        items.Clear();
    }
}
