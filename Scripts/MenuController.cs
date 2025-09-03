using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public static  MenuController Instance;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject missionPanel;
    [SerializeField] private GameObject memoryPanel;
    [SerializeField] private GameObject bagPanel;
    [SerializeField] private GameObject skillPanel;
    [SerializeField] private GameObject settingPanel;

    public bool isMenuOn = false;
    public bool isShopOn = false;
    [SerializeField] private TextMeshProUGUI myGold;
    public static int menuNum;
    public static Action onBag;
    [SerializeField] private GameObject data_Mission_UIController;
    private void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        MenuOn();
        if (isMenuOn == false) return;
        myGold.text = $"$ {Inventory.Instance.myGold.ToString()}";
        // 데이터
        memoryPanel.SetActive(menuNum == 0);
        // 미션
        missionPanel.SetActive(menuNum == 1);
    
        // 가방
        bagPanel.SetActive(menuNum == 2);
        if (menuNum == 2) onBag?.Invoke();

        // 스킬
        skillPanel.SetActive(menuNum == 3);

        // 세팅
        settingPanel.SetActive(menuNum == 4);
    }
    void MenuOn()
    {
        if (isMenuOn == false)
        {
            if (ItemShopController.Instance.itemShopOn == false)
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    mainMenu.SetActive(true);
                    isMenuOn = true;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    menuNum = 0;
                    CanvasController.Instance.Canvas_HUD.SetActive(false);
                    data_Mission_UIController.SetActive(false);
                    Time.timeScale = 0;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                menuNum = 0;
                mainMenu.SetActive(false);
                memoryPanel.SetActive(false);
                missionPanel.SetActive(false);
                bagPanel.SetActive(false);
                skillPanel.SetActive(false);
                settingPanel.SetActive(false);
                isMenuOn = false;
                CanvasController.Instance.Canvas_HUD.SetActive(true);
                data_Mission_UIController.SetActive(true);
                Time.timeScale = 1f;
            }
        }
    }
}
