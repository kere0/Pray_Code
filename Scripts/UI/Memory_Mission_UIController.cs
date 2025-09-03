using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(-70)]
public class Memory_Mission_UIController : MonoBehaviour
{
    // MemoryUI
    private GameObject MemoryUI;
    private TextMeshProUGUI dataName_Text;
    private TextMeshProUGUI dataDescription_Text;
    private TextMeshProUGUI memoryRestore_Text;
    // QuestUI
    private TextMeshProUGUI questName_Text;
    private GameObject QuestUI;

    private void Awake()
    {
        // Memory Text;
        MemoryUI = transform.GetChild(0).gameObject;
        dataName_Text = MemoryUI.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        dataDescription_Text = MemoryUI.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        memoryRestore_Text = MemoryUI.transform.GetChild(5).GetComponent<TextMeshProUGUI>();
        // Quest Text
        QuestUI =  transform.GetChild(1).gameObject;
        questName_Text = QuestUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        Memory_Mission_Controller.Instance.NewDataActive -= MemoryUIOn;
        Memory_Mission_Controller.Instance.NewDataActive += MemoryUIOn;
        
        Memory_Mission_Controller.Instance.NewQuestActive -= QuestUIOn;
        Memory_Mission_Controller.Instance.NewQuestActive += QuestUIOn;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Alpha0))
        {
            MemoryUI.SetActive(false);
        }
    }

    void MemoryUIOn()
    {
        MemoryUI.SetActive(true);
        dataName_Text.text = Memory_Mission_Controller.Instance.currentMemoryRestoreData.Title;
        dataDescription_Text.text = Memory_Mission_Controller.Instance.currentMemoryRestoreData.Description;
        memoryRestore_Text.text = $"MEMORY_RESTORE: [ {Memory_Mission_Controller.Instance.currentMemoryRestoreData.Percent} ]";    
    }
    void QuestUIOn()
    {
        QuestUI.SetActive(true);
        if (Memory_Mission_Controller.Instance.currentQuestData.QuestObjective != 0)
        {
            questName_Text.text = $"{Memory_Mission_Controller.Instance.currentQuestData.QuestTitle}  {Memory_Mission_Controller.Instance.currentQuestData.CurrentObjective} / {Memory_Mission_Controller.Instance.currentQuestData.QuestObjective}";
        }
        else
        {
            questName_Text.text = $"{Memory_Mission_Controller.Instance.currentQuestData.QuestTitle}";
        }
    }
}
