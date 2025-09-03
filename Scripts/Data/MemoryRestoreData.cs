using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MemoryRestoreData : InterfaceID
{
    public string id;
    public string Percent;
    public string Title;
    public string Description;
    public string QuestNum;
    public string Choice1;
    public string Choice2;
    public bool IsRestore;
    public string ID => id;
}