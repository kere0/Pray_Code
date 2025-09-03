using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkillQuickSlot : MonoBehaviour
{
    public SkillDataSO skillDataSo;
    //private Image quickSlotimage;

    public Image skillImage;
    public Image skiilMaskImage;
    
    private Image coolTimeImage;
    //private float coolTime = 0f;
    private float saveCoolTimeCount = 0;
    public bool isStartCoroutine = false;
    void Awake()
    {
        transform.GetChild(1).TryGetComponent(out coolTimeImage);
    }
    private void OnEnable()
    {
        if (saveCoolTimeCount != 0)
        {
            StartCoolTimeCoroutine(skillDataSo.coolTime, saveCoolTimeCount);
        }
    }
    void Update()
    {
        if (skillDataSo != null)
        {
            skillImage.sprite = skillDataSo.skillIcon;
            Color c = skiilMaskImage.color;
            c.a = 1;
            skiilMaskImage.color = c;
        }
    }
    public void StartCoolTimeCoroutine(float coolTime, float coolTimeCount)
    {
        StartCoroutine(UseSkillCorutine(coolTime, coolTimeCount));
    }
    IEnumerator UseSkillCorutine(float coolTime, float coolTimeCount)
    {
        isStartCoroutine = true;
        //coolTimeCount = coolTime;
        coolTimeImage.fillAmount = 1f;
        coolTimeImage.gameObject.SetActive(true);

        while (coolTimeCount > 0)
        {
            coolTimeCount -= Time.deltaTime;
            saveCoolTimeCount = coolTimeCount;
            coolTimeImage.fillAmount = coolTimeCount / coolTime;
            yield return null;
        }
        coolTimeImage.gameObject.SetActive(false);
        coolTimeImage.fillAmount = 0f;
        saveCoolTimeCount = 0f;
        isStartCoroutine = false;
    }
}
