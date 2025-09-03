using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Circle : MonoBehaviour
{
    private float durationTime = 0f;
    private void OnEnable()
    {
        durationTime = 0f;
    }
    private void OnDisable()
    {
        durationTime = 0f;
    }
    void Update()
    {
        durationTime += Time.deltaTime;
        if (durationTime > 1)
        {
            Managers.PoolManager.ObjPush(gameObject, gameObject.name);
        }
    }
}
