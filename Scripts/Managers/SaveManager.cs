using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager
{   
    public List<ISaveable> saveables = new List<ISaveable>();
    public string SavePath;

    public void Init()
    {
        SavePath = Application.persistentDataPath + "/jsonSaveData.json";
    }

    public void Register(ISaveable saveable)
    {
        if (saveables.Contains(saveable) != true)
        {
            saveables.Add(saveable);
        }
    }
    public void Unregister(ISaveable saveable)
    {
        if (saveables.Contains(saveable) == true)
        {
            saveables.Remove(saveable);
        }
    }
    public void SaveAll()
    {
        SaveSnapshot.Instance.Clear();
        foreach (ISaveable saveable in saveables)
        {
            saveable.SaveData();
        }
        string allJson = JsonUtility.ToJson(SaveSnapshot.Instance.saveData, true);
        File.WriteAllText(SavePath, allJson);
    }
    public void LoadAll(Define.SelectMode selectMode)
    {
        if (selectMode == Define.SelectMode.LoadGame)
        {
            if (File.Exists(SavePath) == false) return;
            string jsonData = File.ReadAllText(SavePath);
            SaveSnapshot.Instance.saveData = JsonUtility.FromJson<SaveSnapshot.SaveData>(jsonData);
        }
    }
    // public void SetLoadData()
    // {
    //     switch (GameManager.SelectMode)
    //     {
    //         case Define.SelectMode.NewGame:
    //             foreach (ISaveable saveable in saveables)
    //             {
    //                 saveable.LoadData(null);
    //             }
    //             break;
    //         case Define.SelectMode.LoadGame:
    //             if (File.Exists(SavePath) == false)
    //             {
    //                 Debug.Log($"세이브 파일이 없음: {SavePath}");
    //                 foreach (ISaveable saveData in saveables)
    //                 {
    //                     saveData.LoadData(null);
    //                 }
    //                 return;
    //             }
    //             Debug.Log("LoadData 뿌려줌");
    //             foreach (ISaveable saveable in saveables)
    //             {
    //                 saveable.LoadData(SaveSnapshot.Instance.saveData);
    //             }
    //             break;
    //     }
    // }
    // public void LoadAll(Define.SelectMode selectMode)
    // {
    //     switch (selectMode)
    //     {
    //         case Define.SelectMode.NewGame : 
    //             Debug.Log($"새 게임: {SavePath}");
    //             foreach (ISaveable saveData in saveables)
    //             {
    //                 saveData.LoadData(null);
    //             }
    //             break;
    //         case Define.SelectMode.LoadGame :
    //             if (File.Exists(SavePath) == false)
    //             {
    //                 Debug.Log($"세이브 파일이 없음: {SavePath}");
    //                 foreach (ISaveable saveData in saveables)
    //                 {
    //                     saveData.LoadData(null);
    //                 }
    //                 return;
    //             }
    //             string jsonData = File.ReadAllText(SavePath);
    //             SaveFileData saveFileData = JsonUtility.FromJson<SaveFileData>(jsonData);
    //             foreach (SaveFileData.SaveEntry saveData in  saveFileData.entries)
    //             {
    //                 Type classType = Type.GetType(saveData.className); // 문자열을 진짜 C#Type 객체로 변환해주는 메서드 // 타입정보
    //                 Type type = Type.GetType(saveData.typeName); // 문자열을 진짜 C#Type 객체로 변환해주는 메서드 // 타입정보
    //
    //                 object obj = JsonUtility.FromJson(saveData.jsonData, type);
    //                 ISaveable target = saveables.Find(saveAble => saveAble.GetType().Name == classType.Name);
    //                 Debug.Log(classType.Name + " 클래스 이름 ");
    //
    //                 if (target != null)
    //                 {
    //                     Debug.Log("값 들어감");
    //
    //                     target.LoadData(obj);
    //                 }
    //             }
    //             break;
    //     }
    // }
    // 모든 데이터를 세이브해줌
    // public void SaveAll()
    // {
    //     SaveFileData SaveFileData = new SaveFileData();
    //     //SaveData 
    //     foreach (ISaveable saveable in saveables)
    //     {
    //         var data = saveable.SaveData();
    //         string className = saveable.GetType().AssemblyQualifiedName; // 클래스 타입정보 저장
    //         string typeName = data.GetType().AssemblyQualifiedName; // 기존 타입정보 저장
    //         string jsonData = JsonUtility.ToJson(data, true);
    //         SaveFileData.entries.Add(new SaveFileData.SaveEntry
    //         {
    //             id = data.GetType().Name,
    //             jsonData = jsonData,
    //             className = className,
    //             typeName = typeName
    //         });
    //     }
    //     string allJosn = JsonUtility.ToJson(SaveFileData, true);
    //     File.WriteAllText(SavePath, allJosn);
    // }
    // // 모든 데이터를 로드해줌
    // public void LoadAll(Define.SelectMode selectMode)
    // {
    //     switch (selectMode)
    //     {
    //         case Define.SelectMode.NewGame : 
    //             Debug.Log($"새 게임: {SavePath}");
    //             foreach (ISaveable saveData in saveables)
    //             {
    //                 saveData.LoadData(null);
    //             }
    //             break;
    //         case Define.SelectMode.LoadGame :
    //             if (File.Exists(SavePath) == false)
    //             {
    //                 Debug.Log($"세이브 파일이 없음: {SavePath}");
    //                 foreach (ISaveable saveData in saveables)
    //                 {
    //                     saveData.LoadData(null);
    //                 }
    //                 return;
    //             }
    //             string jsonData = File.ReadAllText(SavePath);
    //             SaveFileData saveFileData = JsonUtility.FromJson<SaveFileData>(jsonData);
    //             foreach (SaveFileData.SaveEntry saveData in  saveFileData.entries)
    //             {
    //                 Type classType = Type.GetType(saveData.className); // 문자열을 진짜 C#Type 객체로 변환해주는 메서드 // 타입정보
    //                 Type type = Type.GetType(saveData.typeName); // 문자열을 진짜 C#Type 객체로 변환해주는 메서드 // 타입정보
    //
    //                 object obj = JsonUtility.FromJson(saveData.jsonData, type);
    //                 ISaveable target = saveables.Find(saveAble => saveAble.GetType().Name == classType.Name);
    //                 Debug.Log(classType.Name + " 클래스 이름 ");
    //
    //                 if (target != null)
    //                 {
    //                     Debug.Log("값 들어감");
    //
    //                     target.LoadData(obj);
    //                 }
    //             }
    //             break;
    //     }
    // }
    public void Clear()
    {
        saveables.Clear();
        SavePath = null;
    }
}
