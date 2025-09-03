using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBossCutSceneTrigger : MonoBehaviour
{
    private bool isPlayKnightBossCutScene = false;
    public GameObject knightBossBlock;
    private void OnTriggerEnter(Collider other)
    {
        if (isPlayKnightBossCutScene == false)
        {
            if (other.tag == "Player")
            {
                isPlayKnightBossCutScene = true;
                knightBossBlock.GetComponent<Collider>().isTrigger = false;
                TimelineManager.Instance.KnightBossCutScene();
                GameManager.Instance.isBossBattle = true;
            }
        } 
    }
}
