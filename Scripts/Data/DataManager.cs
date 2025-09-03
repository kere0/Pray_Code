using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    public Dictionary<Type, Dictionary<string, object>> dataTables = new();

    // 시작할때 해줌
    public void SetResourceData()
    {
        LoadTextAssetResource();
    }
    private void LoadTextAssetResource()
    {
        // CSV에서 맨처음에 받아오는거
        RegisterData<QuestData>("CSVData/QuestData");
        RegisterData<MemoryRestoreData>("CSVData/MemoryRestoreData");
        RegisterData<PlayerData>("CSVData/PlayerData");
        RegisterData<InventoryData>("CSVData/InventoryData");
    }
    private void RegisterData<T>(string assetName) where T : InterfaceID, new()
    {
        Dictionary<string, object> dict = Managers.CSVLoader.LoadCSV<T>(assetName);
        dataTables[typeof(T)] = dict;
    }
    public T Get<T>(string id) where T : InterfaceID
    {
        if (dataTables.TryGetValue(typeof(T), out Dictionary<string, object> dict))
        {
            if (dict.TryGetValue(id, out object value))
            {
                return (T)value;
            }
        }
        Debug.Log($"ID {id} 없음");
        return default;
    }

    public void DataTableClear()
    {
        dataTables.Clear();
    }
}
