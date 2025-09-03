    using System;
    using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [FormerlySerializedAs("item")] public ItemSO itemSo;
    //public Image icon;
    public int count;
    private GameObject item_Sprite;
    private GameObject item_Icon;
    public TextMeshProUGUI ItemName_Info;
    public TextMeshProUGUI Item_Info;

    private void Awake()
    {
        //TryGetComponent(out icon);
        item_Sprite = transform.GetChild(0).gameObject;
        item_Icon = transform.GetChild(1).gameObject;
        ItemName_Info = transform.parent.parent.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        Item_Info = transform.parent.parent.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        UpdateSlot();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemSo != null)
        {
            ItemName_Info.text = itemSo.itemName;
            Item_Info.text = itemSo.description;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemSo != null)
        {
            ItemName_Info.text = "";
            Item_Info.text = "";
        }
    }
    public void AddItem(ItemSO newItemSo, int num)
    {
        if (itemSo == null)
        {
            itemSo = newItemSo;
            Image image = item_Sprite.GetComponent<Image>();
            Color color = image.color;
            color.a = 255;
            image.color = color;
            item_Sprite.GetComponent<Image>().sprite = itemSo.icon;
        }
        count = num;
        item_Icon.GetComponent<TextMeshProUGUI>().text = $"{itemSo.itemName} X{num}";
    
    }
    public void ClearSlot()
    {
        if (itemSo != null && item_Icon != null)
        {
            itemSo = null;
            item_Sprite = null;
        }
    }

    void UpdateSlot()
    {
        if (count <= 0)
        {
            if (item_Sprite != null && item_Icon != null) return;
            item_Sprite.SetActive(false);
            item_Icon.SetActive(false);
            // Color color = item_Sprite.GetComponent<Image>().color;
            // color.a = 0;
            ClearSlot();
        }
        else
        {
            item_Sprite.SetActive(true);
            item_Icon.SetActive(true);
        }
    }
}
