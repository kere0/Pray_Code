using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ItemShopController : MonoBehaviour
{
    public static ItemShopController Instance;
    GameObject itemShop;
    public Action OnItemShop;
    private GameObject itemInfo;
    private GameObject data_Mission_UIController;
    
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemNum;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI itemPrice;
    public TextMeshProUGUI myGold;
    public bool itemShopOn = false;

    private void Awake()
    {
        Instance = this;
        itemShop = transform.GetChild(0).gameObject;
        itemInfo = transform.GetChild(0).transform.GetChild(6).gameObject;
        itemName = itemInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        itemNum = itemInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        itemDescription = itemInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        itemPrice = itemInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        myGold = itemShop.transform.GetChild(7).GetComponent<TextMeshProUGUI>();
        data_Mission_UIController = transform.root.GetChild(2).gameObject;
        
        GameManager.Instance.GameStartAction -= Init;
        GameManager.Instance.GameStartAction += Init;
    }
    void Init()
    {
        Interaction.Instance.ShopInteractActionOn -= ItemShopOn;
        Interaction.Instance.ShopInteractActionOn += ItemShopOn;
    }
    private void Update()
    {
        ItemShopOff();
    }

    void UpdateGold()
    {
        myGold.text = $"$ {Inventory.Instance.myGold.ToString()}";
    }

    void ItemShopOn()
    {
        UpdateGold();
        data_Mission_UIController.SetActive(false);
        itemShop.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        itemShopOn = true;
        Time.timeScale = 0;
    }

    void ItemShopOff()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            data_Mission_UIController.SetActive(true);
            itemShop.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            itemShopOn = false;
            Time.timeScale = 1;
            CanvasController.Instance.Canvas_HUD.SetActive(true);
        }
    }
}
