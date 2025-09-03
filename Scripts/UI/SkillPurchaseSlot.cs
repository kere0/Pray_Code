using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkillPurchaseSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image skillIcon;
    private Image ActiveCheckImage;
    public SkillDataSO skillDataSo;
    public TextMeshProUGUI  skillName;
    public TextMeshProUGUI  skillDescription;
    public TextMeshProUGUI  purchaseDescription;
    public bool isPurchasd = false;
    private Color startColor;
    private void Awake()
    {
        ActiveCheckImage = transform.GetChild(1).GetComponent<Image>();
        startColor = purchaseDescription.color;
        skillIcon = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        skillIcon.sprite = skillDataSo.skillIcon;
    }

    private void Start()
    {
        SetPurchase();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        skillName.text = skillDataSo.skillName;
        skillDescription.text = skillDataSo.skillDescription;
        if (isPurchasd == false)
        {
            purchaseDescription.color = startColor;
            purchaseDescription.text = $"가격 : {skillDataSo.price}";
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        skillName.text = "";
        skillDescription.text = "";
        purchaseDescription.text = "";
        StopCoroutine("PurchaseTextCoroutine");

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isPurchasd == false)
        {
            isPurchasd = true; //
            Color c = purchaseDescription.color;
            c = new Color(0.2392f, 1f, 0.7490f, 1);
            purchaseDescription.color = c;
            Inventory.Instance.myGold -= skillDataSo.price;
            
            Color c2 = ActiveCheckImage.color; //
            c2.a = 0f; //
            ActiveCheckImage.color = c2; //
            PlayerSkill.Instance.AddSkill(skillDataSo);
            purchaseDescription.text = "구매하였습니다";
            QuickSlotController.Instance.updateSkillAction?.Invoke();
            StartCoroutine("PurchaseTextCoroutine");
        }
    }

    private void SetPurchase()
    {
        if(SaveSnapshot.Instance.saveData.skillData.Contains(skillDataSo.effectKey))
        {
            isPurchasd = true;
            Color c2 = ActiveCheckImage.color; //
            c2.a = 0f; //
            ActiveCheckImage.color = c2; //
        }
    }
    IEnumerator PurchaseTextCoroutine()
    {
        yield return new WaitForSeconds(1f);
        purchaseDescription.text = "";
    }
}
