using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MissionUIController : MonoBehaviour
{
    public static MissionUIController Instance;
    public MissionSlot[] missionSlot;
    public TextMeshProUGUI missionName;
    public TextMeshProUGUI missionDescription;
    public QuestData currentQuest;
    public int QuestNum = 5; 
    public int CurrentQuestIndex = 0;

    void Awake()
    {
        Instance = this;
    }
    public void MissionUpdate()
    {
        CurrentQuestIndex++;
        missionSlot[CurrentQuestIndex-1].questData = Managers.DataManager.Get<QuestData>($"Q{CurrentQuestIndex:D2}");
    }
}
