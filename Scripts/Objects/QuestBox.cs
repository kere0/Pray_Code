using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class QuestBox : BaseBox, IQuestCheck
{
    public bool QuestCheck => isQuestObject;

    public Transform coreItem;
    public bool isQuestObject = false;

    void Awake()
    {
        GameManager.Instance.GameStartAction -= Init;
        GameManager.Instance.GameStartAction += Init;
    }
    void Init()
    {
        boxCap = transform.GetChild(1);
        Interaction.Instance.boxOpenAction -= BoxOpen;
        Interaction.Instance.boxOpenAction += BoxOpen;
    }

    private void Update()
    {
        BoxOpen();
        if (Memory_Mission_Controller.Instance.currentQuestData.TargetID == TargetID.EnergyCore.ToString())
        {
            isQuestObject = true;
        }
        else
        {
            isQuestObject = false;
        }
    }

    void BoxOpen()
    {
        if (isBoxOpen == false)
        {
            if (isOpen == true)
            {
                StartCoroutine(BoxOpenCoroutine());
                isBoxOpen = true;
            }
        }
    }

    void ItemActive()
    {
        coreItem.DOJump(coreItem.transform.position + transform.TransformDirection(new Vector3(1.5f , 0f, 0f)), 1.5f, 1, 1f);
    }

    IEnumerator BoxOpenCoroutine()
    {
        float duration = 0.5f; // 열리는 시간
        float elapsed = 0f;

        Quaternion startRot = boxCap.transform.rotation;
        Quaternion endRot = Quaternion.Euler(boxCap.transform.eulerAngles + new Vector3(0f, 0f, 51f)); // 예: 위로 90도 열기

        while (elapsed < duration)
        {
            boxCap.transform.rotation = Quaternion.Lerp(startRot, endRot, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        boxCap.transform.rotation = endRot; // 마지막 보정
        ItemActive();
    }

}
