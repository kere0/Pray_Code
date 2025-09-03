using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public static HUDController Instance;
    public TextMeshProUGUI text;
    public Image endImage;
    Vector3 TextPos = new Vector3(290f, -107.8f, 0f);

    private void Awake()
    {
        Instance = this;
        GameManager.Instance.GameStartAction -= Init;
        GameManager.Instance.GameStartAction += Init;
    }
    public void Init()
    {
        Interaction.Instance.itemActionOn -= ItemInteractOn;
        Interaction.Instance.itemActionOn += ItemInteractOn;
        
        Interaction.Instance.shopActionOn -= ShopInteractOn;
        Interaction.Instance.shopActionOn += ShopInteractOn;
        
        Interaction.Instance.boxActionOn -= BoxInteractOn;
        Interaction.Instance.boxActionOn += BoxInteractOn;

        Interaction.Instance.interactActionOff -= InteractOff;
        Interaction.Instance.interactActionOff += InteractOff;

        Interaction.Instance.saveActionOn -= SaveInteractOn;
        Interaction.Instance.saveActionOn += SaveInteractOn;
        
        text.enabled = false;
    }
    void ItemInteractOn(ItemSO itemSo)
    {
        text.enabled = true;
        text.gameObject.GetComponent<RectTransform>().anchoredPosition  = TextPos;
        text.text = $"{itemSo.itemName} 획득하기(E)";
    }
    void ShopInteractOn()
    {
        text.enabled = true;
        text.gameObject.GetComponent<RectTransform>().anchoredPosition  = Vector2.zero;
        text.text = $"상점이용(E)";
    }
    void BoxInteractOn()
    {
        text.enabled = true;
        text.gameObject.GetComponent<RectTransform>().anchoredPosition  = Vector2.zero;
        text.text = $"열기(E)";
    }
    void SaveInteractOn()
    {
        text.enabled = true;
        text.gameObject.GetComponent<RectTransform>().anchoredPosition  = Vector2.zero;
        text.text = $"저장하기(E)";
    }
    void InteractOff()
    {
        text.text = "";
        text.enabled = false;
    }
    
}
