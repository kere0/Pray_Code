using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SettingUIController : MonoBehaviour, ISaveable
{
    public MouseSensitiveManager mouseSensitiveManager;
    public Slider mouseHorizontalSlider;
    public TextMeshProUGUI horizontalText;
    public Slider mouseVerticalSlider;
    public TextMeshProUGUI verticalText;
    public Button gameEndButton;
    private float fadeDuration = 1f;
    public Image endImage;
    public VolumeController volumeController;
    private bool isClicked = false;

    // MouseSensitive
    private float currentMouseHorizontalValue;
    private float currentMouseVerticalValue;
    public const float defaultValue = 0.5f;
    private void Awake()
    {
        gameEndButton.onClick.AddListener(GameEndClick);
        mouseVerticalSlider.onValueChanged.AddListener(SetMouseVerticalSensitivity);
        mouseHorizontalSlider.onValueChanged.AddListener(SetMouseHorizontalSensitivity);
    }

    private void Start()  
    {
        LoadData();
        volumeController.LoadData();
    }
    void SetMouseVerticalSensitivity(float value)
    {
        float sensitivity = value - 0.5f;
        verticalText.text = (sensitivity * 100f).ToString("F0");
        mouseSensitiveManager.SetVerticalSensitivity(sensitivity);
        currentMouseVerticalValue = mouseVerticalSlider.value;
        SaveSnapshot.Instance.saveData.currentMouseVerticalValue = currentMouseVerticalValue;
    }
    void SetMouseHorizontalSensitivity(float value)
    {
        float sensitivity = value - 0.5f;
        horizontalText.text = (sensitivity * 100f).ToString("F0");
        mouseSensitiveManager.SetHorizontalSensitivity(sensitivity);
        currentMouseHorizontalValue = mouseHorizontalSlider.value;
        SaveSnapshot.Instance.saveData.currentMouseHorizontalValue = currentMouseHorizontalValue;
    }
    void GameEndClick()
    {
        if (isClicked == false)
        {
            isClicked = true;
            Managers.Instance.Clear();
            StartCoroutine(FadeIn());
        }
    }
    private IEnumerator FadeIn()
    {
        float timeElapsed = 0f;
        Color color = endImage.color; 
        Color targetColor = new Color(color.r, color.g, color.b, 1f); 
        
        while (timeElapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(color.a, targetColor.a, timeElapsed / fadeDuration);
            endImage.color = new Color(color.r, color.g, color.b, alpha);

            timeElapsed += Time.unscaledDeltaTime * 0.5f;
            Debug.Log(endImage.color.a);
            yield return null; 
        }
        endImage.color = targetColor;
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync("StartScene");
        SoundManager.Instance.PlayBgm(Bgm.TitleBgm);
    }

    public void SaveData()
    {
        //SaveSnapshot.Instance.saveData.currentVolumeValue = currentMasterVolumeValue;
        SaveSnapshot.Instance.saveData.currentMouseHorizontalValue = currentMouseHorizontalValue;
        SaveSnapshot.Instance.saveData.currentMouseVerticalValue = currentMouseVerticalValue;
    }

    public void LoadData()
    {
        switch (GameManager.SelectMode)
        {
            case Define.SelectMode.NewGame:
                // 마우스 민감도
                currentMouseVerticalValue = defaultValue;
                currentMouseHorizontalValue = defaultValue;
                mouseVerticalSlider.value = currentMouseVerticalValue;
                mouseHorizontalSlider.value = currentMouseHorizontalValue;
                SetMouseVerticalSensitivity(currentMouseVerticalValue);
                SetMouseHorizontalSensitivity(currentMouseHorizontalValue);
                SaveData();
                break;
            case Define.SelectMode.LoadGame:
                if (File.Exists(Managers.SaveManager.SavePath) == false)
                {
                    // 마우스 민감도
                    currentMouseVerticalValue = defaultValue;
                    currentMouseHorizontalValue = defaultValue;
                    mouseVerticalSlider.value = currentMouseVerticalValue;
                    mouseHorizontalSlider.value = currentMouseHorizontalValue;
                    SetMouseVerticalSensitivity(currentMouseVerticalValue);
                    SetMouseHorizontalSensitivity(currentMouseHorizontalValue);
                    SaveData();
                }
                // 마우스 민감도
                currentMouseVerticalValue = SaveSnapshot.Instance.saveData.currentMouseVerticalValue;
                currentMouseHorizontalValue = SaveSnapshot.Instance.saveData.currentMouseHorizontalValue;
                mouseVerticalSlider.value = currentMouseVerticalValue;
                mouseHorizontalSlider.value = currentMouseHorizontalValue;
                SetMouseVerticalSensitivity(currentMouseVerticalValue);
                SetMouseHorizontalSensitivity(currentMouseHorizontalValue);
                break;
        }
    }
    public void RegisterToSaveManager()
    {
        Managers.SaveManager.Register(this);
    }
}
