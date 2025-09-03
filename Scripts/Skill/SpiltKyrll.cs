using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiltKyrll : MonoBehaviour
{
    private GameObject headPrefab;
    private GameObject bodyPrefab;
    private bool deathAnimEnd;
    private GameObject go1;
    private GameObject go2;
    private bool end;
    void Awake()
    {
        headPrefab = transform.GetChild(0).gameObject;
        bodyPrefab = transform.GetChild(1).gameObject;
    }
    private void Update()
    {
        if (end == false)
        {
            if (deathAnimEnd == false)
            {
                Execution();
            }
            else
            {
                float t = 0;
                while (t < 2f)
                {
                    t += Time.deltaTime;
                    headPrefab.GetComponent<Animator>().enabled = false;
                    bodyPrefab.GetComponent<Animator>().enabled = false;
                    headPrefab.transform.GetChild(1).GetChild(1).localScale = new Vector3(0f, 1f, 0f);
                    headPrefab.transform.GetChild(1).GetChild(2).localScale = new Vector3(0f, 1f, 0f);
                    headPrefab.transform.GetChild(1).GetChild(3).localScale = new Vector3(0f, 1f, 0f);

                    bodyPrefab.transform.GetChild(1).GetChild(0).localScale = new Vector3(0f, 1f, 0f);
                    headPrefab.transform.localPosition = Vector3.Lerp(headPrefab.transform.localPosition, headPrefab.transform.localPosition - headPrefab.transform.forward * 0.1f, Time.deltaTime*2f);
                    
                    bodyPrefab.transform.localPosition= Vector3.Lerp(bodyPrefab.transform.localPosition, bodyPrefab.transform.localPosition + bodyPrefab.transform.forward * 0.1f, Time.deltaTime*2f);
                }
                t = 0f;
                end = true;
            }
        }
    }

    void Execution()
    {
        if (headPrefab.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            if (headPrefab.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                deathAnimEnd = true;
            }
        }
    }
    
}
