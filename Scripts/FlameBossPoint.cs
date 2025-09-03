using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBossPoint : MonoBehaviour, IQuestCheck
{
    public FlameBoss flameBoss;
    public bool isQuestObject = false;
    public bool QuestCheck => isQuestObject;
    private void Update()
    {
        OnQuestCheck();
    }
    public void OnQuestCheck()
    {
        if (flameBoss.TargetID.ToString() == Memory_Mission_Controller.Instance.currentQuestData.TargetID)
        {
            isQuestObject = true;
            Debug.Log("보스포인트~~~~~~~~~~~~~~~");
        }
        else
        {
            isQuestObject = false;
        }
    }
}
