using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public ItemSO itemSo;
    private Image image;

    public GameObject purchaseText;

    private void Start()
    {
        TryGetComponent(out image);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UpdateInfo();
        Color color = Color.white;
        color.r = 0.2f;
        color.g = 0.2f;
        color.b = 0.2f;
        image.color = color;
        image.color = new Color(0.2392f, 1f, 0.7490f, 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemShopController.Instance.itemName.text = "";
        ItemShopController.Instance.itemNum.text = "";
        ItemShopController.Instance.itemDescription.text = "";
        ItemShopController.Instance.itemPrice.text = "";
        image.color = new Color(0.6078f, 0.7803f, 0.7490f, 1f);
        purchaseText.SetActive(false);
        StopCoroutine("PurchaseTextCoroutine");
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Inventory.Instance.myGold >= itemSo.price)
        {
            Inventory.Instance.myGold -= itemSo.price;
            purchaseText.SetActive(true);
            purchaseText.GetComponent<TextMeshProUGUI>().text = $"{itemSo.itemName}  X 1 구매";
            Inventory.Instance.AddItem(itemSo);
            UpdateInfo();
            StopCoroutine("PurchaseTextCoroutine");
            StartCoroutine("PurchaseTextCoroutine");
        }
    }

    private void UpdateInfo()
    {
        ItemShopController.Instance.itemName.text = itemSo.itemName;
        if (Inventory.Instance.items.ContainsKey(itemSo) == false)
        {
            ItemShopController.Instance.itemNum.text = $"소지 X 0";
        }
        else
        {
            ItemShopController.Instance.itemNum.text = $"소지 X {Inventory.Instance.items[itemSo].ToString()}";
        }
        ItemShopController.Instance.itemDescription.text = itemSo.description;
        ItemShopController.Instance.itemPrice.text = $"골드 {itemSo.price.ToString()}";
        ItemShopController.Instance.myGold.text = $"$ {Inventory.Instance.myGold.ToString()}";

    }
    IEnumerator PurchaseTextCoroutine()
    {
        yield return new WaitForSeconds(1f);
        purchaseText.SetActive(false);
    }
}
