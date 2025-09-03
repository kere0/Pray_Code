using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType
{
    SurviveTime,
    ArriveDestination,
    AcquireItem,
    KillEnemy,
    TalkToNPC
}
[System.Serializable]
public class QuestData : InterfaceID
{
    public string id;
    public string QuestTitle;
    public string QuestDescription;
    public int QuestObjective;
    public int CurrentObjective;
    public string TargetID;
    public string NextQuestID;
    public string MemoryRestored;
    public bool IsCompleted;
    public string ID => id;

}