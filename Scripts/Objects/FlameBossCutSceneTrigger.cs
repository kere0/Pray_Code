using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlameBossCutSceneTrigger : MonoBehaviour
{
    private bool isPlayFlameBossCutScene = false;
    public GameObject flameBossBlock;
    public GameObject finalBossPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (isPlayFlameBossCutScene == false)
        {
            if (other.tag == "Player")
            {
                isPlayFlameBossCutScene = true;
                flameBossBlock.GetComponent<Collider>().isTrigger = false;
                finalBossPoint.SetActive(false);
                TimelineManager.Instance.PlayFlameBossCutScene();
                GameManager.Instance.isBossBattle = true;
            }
        }
    }
}
