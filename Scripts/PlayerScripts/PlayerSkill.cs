using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkill : MonoBehaviour , ISaveable
{
    public static PlayerSkill Instance;
    public Dictionary<string, SkillDataSO> skillData = new Dictionary<string, SkillDataSO>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RegisterToSaveManager();
        LoadData();
    }

    public void AddSkill(SkillDataSO skill)
    {
        Debug.Log(skill.name);
        skillData.Add(skill.effectKey, skill);
    }
    public void SaveData()
    {
        foreach (var skill in skillData)
        {
            SaveSnapshot.Instance.saveData.skillData.Add(skill.Key);
        }
    }
    public void LoadData()
    {
        switch (GameManager.SelectMode)
        {
            case Define.SelectMode.NewGame:
                break;
            case Define.SelectMode.LoadGame:
                if (File.Exists(Managers.SaveManager.SavePath) == false)
                {
                    Debug.Log($"세이브 파일이 없음: {Managers.SaveManager.SavePath}");
                    return;
                }
                if (SaveSnapshot.Instance.saveData.skillData.Count != 0)
                {
                    foreach (var skill in SaveSnapshot.Instance.saveData.skillData)
                    {
                        skillData.Add(skill, Resources.Load<SkillDataSO>($"ScriptableObjects/Skill/{skill}"));
                    }
                }
                QuickSlotController.Instance.updateSkillAction?.Invoke();
                break;
        }
    }
    public void RegisterToSaveManager()
    {
        Managers.SaveManager.Register(this);
    }

    public void Clear()
    {
        skillData.Clear();
    }
}
