using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public enum TargetID
{
    Player,
    Kryll,
    Mutant,
    Hunter,
    Titan,
    DeathKnight,
    FlameBoss,
    EnergyCore
}
[DefaultExecutionOrder(-80)]
public class Memory_Mission_Controller : MonoBehaviour, ISaveable
{
    public static Memory_Mission_Controller Instance;
    public QuestData currentQuestData;
    public MemoryRestoreData currentMemoryRestoreData;
    public int QuestProgress = 0;
    public Action NewDataActive;
    public Action NewQuestActive;
    public KnightBossCutSceneTrigger knightBossCutSceneTrigger;
    public bool start = false;
    private void Awake()
    {
        Instance = this;
        GameManager.Instance.GameStartAction -= LoadData;
        GameManager.Instance.GameStartAction += LoadData;
    }
    private void Start()
    {
        RegisterToSaveManager();
    }
    public void Notify(TargetID targetID)
    {
        // 퀘스트 진행도 체크
        if (targetID.ToString() == currentQuestData.TargetID)
        {
            currentQuestData.CurrentObjective++;
            Debug.Log(targetID.ToString());
            Debug.Log(currentQuestData.QuestObjective);
            NewQuestActive?.Invoke();
        }
        // 퀘스트 완료시
        if (currentQuestData.QuestObjective == currentQuestData.CurrentObjective )
        {
            if (currentQuestData.QuestObjective != 0)
            {
                // // 중간보스 처치후
                // if (currentQuestData.id == "Q03")
                // {
                //     // SoundManager.Instance.PlayBgm(Bgm.Chapter2Bgm);
                //     knightBossCutSceneTrigger.knightBossBlock.SetActive(false);
                // }
                currentMemoryRestoreData = Managers.DataManager.Get<MemoryRestoreData>(currentQuestData.MemoryRestored);
                currentMemoryRestoreData.IsRestore = true;
                MemoryUIController.Instance.MemoryUpdate();
                NewDataActive.Invoke();
                currentQuestData.IsCompleted = true;
                currentQuestData = Managers.DataManager.Get<QuestData>(currentMemoryRestoreData.QuestNum);
                NewQuestActive.Invoke();
                MissionUIController.Instance.MissionUpdate();
                // 최종보스 처치후
                if (currentQuestData.id == "Q06")
                {
                    // SoundManager.Instance.PlayBgm(Bgm.Chapter1Bgm);
                    GameManager.Instance.EndSceneLoad();
                }
            }
        }
    }
    public void SaveData()
    {
        // 퀘스트 데이터 저장
        Dictionary<string, object> questDataDict = new Dictionary<string, object>();
        Managers.DataManager.dataTables.TryGetValue(typeof(QuestData), out questDataDict);
        Dictionary<string, bool> tempQuestDataDict = new Dictionary<string, bool>();
        foreach (var questData in questDataDict)
        {
            QuestData data = (QuestData)questData.Value;
            tempQuestDataDict[data.id] = data.IsCompleted;
        }
        SaveSnapshot.Instance.saveData.QuestData = new Utils.SerializableDict<string, bool>(tempQuestDataDict);
        // 기억복원 데이터 저장
        Dictionary<string, object> memoryDataDict = new Dictionary<string, object>();
        Managers.DataManager.dataTables.TryGetValue(typeof(MemoryRestoreData), out memoryDataDict);
        Dictionary<string, bool> tempMemoryDataDict = new Dictionary<string, bool>();
        foreach (var memorytData in memoryDataDict)
        {
            MemoryRestoreData data = (MemoryRestoreData)memorytData.Value;
            tempMemoryDataDict[data.id] = data.IsRestore;
        }
        SaveSnapshot.Instance.saveData.MemoryRestoreData = new Utils.SerializableDict<string, bool>(tempMemoryDataDict);
    }
    public void LoadData()
    {
        switch (GameManager.SelectMode)
        {
            case Define.SelectMode.NewGame:
                currentMemoryRestoreData = Managers.DataManager.Get<MemoryRestoreData>("M01");
                currentMemoryRestoreData.IsRestore = true;
                currentQuestData = Managers.DataManager.Get<QuestData>(currentMemoryRestoreData.QuestNum);
                MemoryUIController.Instance.MemoryUpdate();
                MissionUIController.Instance.MissionUpdate();
                NewDataActive.Invoke();
                NewQuestActive.Invoke();
                break;
            case Define.SelectMode.LoadGame:
                if (File.Exists(Managers.SaveManager.SavePath) == false)
                {
                    currentMemoryRestoreData = Managers.DataManager.Get<MemoryRestoreData>("M01");
                    currentMemoryRestoreData.IsRestore = true;
                    currentQuestData = Managers.DataManager.Get<QuestData>(currentMemoryRestoreData.QuestNum);
                    MemoryUIController.Instance.MemoryUpdate();
                    MissionUIController.Instance.MissionUpdate();
                    NewDataActive.Invoke();
                    NewQuestActive.Invoke();

                    Debug.Log($"세이브 파일이 없음: {Managers.SaveManager.SavePath}");
                    return;
                }
                Dictionary<string, bool> tempQuestDataDict = SaveSnapshot.Instance.saveData.QuestData.ToDictionary();
                foreach (var questData in tempQuestDataDict)
                {
                    Managers.DataManager.Get<QuestData>(questData.Key).IsCompleted = questData.Value;
                }
                Dictionary<string, bool> tempMemoryDataDict = SaveSnapshot.Instance.saveData.MemoryRestoreData.ToDictionary();
                foreach (var memorytData in tempMemoryDataDict)
                {
                    Managers.DataManager.Get<MemoryRestoreData>(memorytData.Key).IsRestore = memorytData.Value;
                }
                if (Managers.DataManager.Get<QuestData>("Q05").IsCompleted == true)
                {
                    GameManager.Instance.EndSceneLoad();
                }
                SetLoadData();
                break;
        }
    }

    private void SetLoadData()
    {
        var memoryRestoreData = Managers.DataManager.dataTables[typeof(MemoryRestoreData)];

        foreach (var memorytData in memoryRestoreData)
        {
            if(((MemoryRestoreData)memorytData.Value).IsRestore  == true)
            {
                MemoryUIController.Instance.MemoryUpdate();
                MissionUIController.Instance.MissionUpdate();

                currentMemoryRestoreData = (MemoryRestoreData)memorytData.Value;

                currentQuestData = Managers.DataManager.Get<QuestData>(currentMemoryRestoreData.QuestNum);
                NewQuestActive.Invoke();
            }
        }
    }
    public void RegisterToSaveManager()
    {
        Managers.SaveManager.Register(this);
    }
}
