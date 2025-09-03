using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSnapshot : MonoBehaviour
{
    public static SaveSnapshot Instance;
    
    public SaveData saveData = new SaveData();
    [Serializable]
    public class SaveData
    {
        public string savePointId;  
        public Vector3 savePos;
        public Quaternion saveRot;
        public PlayerData playerData;
        public float currentHp;
        public int money;
        public Utils.SerializableDict<string, int> itemData;
        public List<string> skillData = new List<string>();
        public Utils.SerializableDict<string, bool> QuestData;
        public Utils.SerializableDict<string, bool> MemoryRestoreData;
        public float currentMouseHorizontalValue;
        public float currentMouseVerticalValue;
        public float masterVolume;
        public float bgmVolume;
        public float sfxVolume;
    }
    void Awake()
    {
        Instance = this;
    }
    public void Clear()
    {
        saveData.itemData.Clear();
        saveData.skillData.Clear();
        saveData.QuestData.Clear();
        saveData.MemoryRestoreData.Clear();
    }
}