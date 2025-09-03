using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ItemQuickSlot : MonoBehaviour
{
    public ItemSO itemSo;
    private Image quickSlotimage;
    private TextMeshProUGUI itemNumText;
    private Image coolTimeImage;
    //private float coolTime = 3f;
    private float coolTimeCount;
    public bool isStartCoroutine = false;
    private float saveCoolTimeCount = 0;

    private void Awake()
    {
        transform.GetChild(0).gameObject.TryGetComponent(out quickSlotimage);
        transform.GetChild(1).gameObject.TryGetComponent(out itemNumText);
        transform.GetChild(2).gameObject.TryGetComponent(out coolTimeImage);
    }
    void Start()
    {
        Interaction.Instance.getItemActionOn -= SetSlotInfo;
        Interaction.Instance.getItemActionOn += SetSlotInfo;
        QuickSlotController.Instance.setItemAction -= SetSlotInfo;
        QuickSlotController.Instance.setItemAction += SetSlotInfo;
        SetSlotInfo();
    }
    private void OnEnable()
    {
        if (saveCoolTimeCount != 0)
        {
            StartCoolTimeCoroutine(itemSo.coolTime, saveCoolTimeCount);
        }
        SetSlotInfo();
    }
    public void SetSlotInfo()
    {
        if (itemSo != null)
        {
            if (Inventory.Instance.items[itemSo] <= 0)
            {
                quickSlotimage.sprite = itemSo.icon;
                Color color = quickSlotimage.color;
                color.a = 0.3f;
                quickSlotimage.color = color;
                itemNumText.text = Inventory.Instance.items[itemSo].ToString();
            }
            else
            {
                quickSlotimage.sprite = itemSo.icon;

                Color c = quickSlotimage.color;
                c.a = 1;
                quickSlotimage.color = c;
                itemNumText.text = Inventory.Instance.items[itemSo].ToString();
            }
        }
        else
        {
            quickSlotimage.sprite = null;
            Color c = quickSlotimage.color;
            c.a = 0;
            quickSlotimage.color = c;
            itemNumText.text = "";
        }
    }

    public void StartCoolTimeCoroutine(float coolTime, float coolTimeCount)
    {
        StartCoroutine(UseItemCorutine(coolTime, coolTimeCount));
    }
    IEnumerator UseItemCorutine(float coolTime, float coolTimeCount)
    {
        isStartCoroutine = true;
        SetSlotInfo();
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
