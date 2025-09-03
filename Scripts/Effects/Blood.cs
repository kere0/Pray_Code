using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Blood : MonoBehaviour
{
    private float timeCount;
    
    private void OnDisable()
    {
        timeCount = 0;
    }

    void Update()
    {
        timeCount += Time.deltaTime;
        if (timeCount >= 7.0f)
        {
            Managers.PoolManager.ObjPush(gameObject, gameObject.name);
        }
    }
}
