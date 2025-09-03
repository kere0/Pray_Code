using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MissionSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public QuestData questData;
    private TextMeshProUGUI  missionName;
    private GameObject line;

    private void Start()
    {
        missionName = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        line = transform.parent.parent.GetChild(3).gameObject;
    }

    private void Update()
    {
        if (questData != null)
        {
            missionName.text = questData.QuestTitle;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (questData.QuestTitle == "") return;
        if (questData != null)
        {
            MissionUIController.Instance.missionName.text = questData.QuestTitle;
            MissionUIController.Instance.missionDescription.text = questData.QuestDescription;
            line.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (questData.QuestTitle == "") return;

        if (questData != null)
        {
            MissionUIController.Instance.missionName.text = "";
            MissionUIController.Instance.missionDescription.text = "";
            line.SetActive(false);
        }
    }
}
