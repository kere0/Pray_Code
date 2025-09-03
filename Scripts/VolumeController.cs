using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour, ISaveable
{
    public AudioMixer AudioMixer;
    public MouseSensitiveManager mouseSensitiveManager;
    public Slider masterVolumeSlider;
    public Slider bgmVolumeSlider;
    public Slider sfxVolumeSlider;
    public TextMeshProUGUI masterVolumeText;
    public TextMeshProUGUI bgmVolumeText;
    public TextMeshProUGUI sfxVolumeText;
    public float currentMasterVolumeValue;
    public float currentBgmVolumeValue;
    public float currentSfxVolumeValue;
    private const float defaultValue = 0.7f;
    private void Awake()
    {
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmVolumeSlider.onValueChanged.AddListener(SetBgmVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSfxVolume);
        
    }
    private void Start()
    {
        GameManager.Instance.GameStartAction -= SetVolume;
        GameManager.Instance.GameStartAction += SetVolume;
        GameManager.Instance.GameStartAction -= InitSetData;
        GameManager.Instance.GameStartAction += InitSetData;
        SaveData();
    }
    public void SetMasterVolume(float volume)
    {
        AudioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        masterVolumeText.text = (volume * 100).ToString("F0");
        currentMasterVolumeValue = masterVolumeSlider.value;
        SaveSnapshot.Instance.saveData.masterVolume = currentMasterVolumeValue;
    }
    public void SetBgmVolume(float volume)
    {
        AudioMixer.SetFloat("BGMVolume", Mathf.Log10(volume) * 20);
        bgmVolumeText.text = (volume * 100).ToString("F0");
        currentBgmVolumeValue = bgmVolumeSlider.value;
        SaveSnapshot.Instance.saveData.bgmVolume = currentBgmVolumeValue;
    }
    public void SetSfxVolume(float volume)
    {
        AudioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        sfxVolumeText.text = (volume * 100).ToString("F0");
        currentSfxVolumeValue = sfxVolumeSlider.value;
        SaveSnapshot.Instance.saveData.sfxVolume = currentSfxVolumeValue;
    }
    void SetVolume()
    {
        AudioMixer.SetFloat("MasterVolume", Mathf.Log10(SaveSnapshot.Instance.saveData.masterVolume) * 20);
        AudioMixer.SetFloat("BGMVolume", Mathf.Log10(SaveSnapshot.Instance.saveData.bgmVolume) * 20);
        AudioMixer.SetFloat("SFXVolume", Mathf.Log10(SaveSnapshot.Instance.saveData.sfxVolume) * 20);
    }
    public void SaveData()
    {
        SaveSnapshot.Instance.saveData.masterVolume = currentMasterVolumeValue;
        SaveSnapshot.Instance.saveData.bgmVolume = currentBgmVolumeValue;
        SaveSnapshot.Instance.saveData.sfxVolume = currentSfxVolumeValue;
    }

    public void InitSetData()
    {
        switch (GameManager.SelectMode)
        {
            case Define.SelectMode.NewGame :
                SaveSnapshot.Instance.saveData.currentMouseVerticalValue = SettingUIController.defaultValue;
                SaveSnapshot.Instance.saveData.currentMouseHorizontalValue = SettingUIController.defaultValue;
                SaveSnapshot.Instance.saveData.masterVolume = defaultValue;
                SaveSnapshot.Instance.saveData.bgmVolume = defaultValue;
                SaveSnapshot.Instance.saveData.sfxVolume = defaultValue;
                break;
            case Define.SelectMode.LoadGame :
                AudioMixer.SetFloat("MasterVolume", Mathf.Log10(SaveSnapshot.Instance.saveData.masterVolume) * 20); 
                AudioMixer.SetFloat("BGMVolume", Mathf.Log10(SaveSnapshot.Instance.saveData.bgmVolume) * 20);
                AudioMixer.SetFloat("SFXVolume", Mathf.Log10(SaveSnapshot.Instance.saveData.sfxVolume) * 20);
                mouseSensitiveManager.SetHorizontalSensitivity(SaveSnapshot.Instance.saveData.currentMouseHorizontalValue);
                mouseSensitiveManager.SetVerticalSensitivity(SaveSnapshot.Instance.saveData.currentMouseVerticalValue);
                break;
        }
    }
    public void LoadData()
    {
        switch (GameManager.SelectMode)
        {
            case Define.SelectMode.NewGame:
                currentMasterVolumeValue = defaultValue;
                currentBgmVolumeValue = defaultValue;
                currentSfxVolumeValue = defaultValue;
                masterVolumeSlider.value = currentMasterVolumeValue;
                bgmVolumeSlider.value = currentBgmVolumeValue;
                sfxVolumeSlider.value = currentSfxVolumeValue;
                SetMasterVolume(currentMasterVolumeValue);
                SetBgmVolume(currentBgmVolumeValue);
                SetSfxVolume(currentSfxVolumeValue);
                SaveData();
                break;
            case Define.SelectMode.LoadGame:
                if (File.Exists(Managers.SaveManager.SavePath) == false)
                {
                    currentMasterVolumeValue = defaultValue;
                    currentBgmVolumeValue = defaultValue;
                    currentSfxVolumeValue = defaultValue;
                    masterVolumeSlider.value = currentMasterVolumeValue;
                    bgmVolumeSlider.value = currentBgmVolumeValue;
                    sfxVolumeSlider.value = currentSfxVolumeValue;
                    SetMasterVolume(currentMasterVolumeValue);
                    SetBgmVolume(currentBgmVolumeValue);
                    SetSfxVolume(currentSfxVolumeValue);
                    SaveData();
                }
                currentMasterVolumeValue = SaveSnapshot.Instance.saveData.masterVolume;
                currentBgmVolumeValue = SaveSnapshot.Instance.saveData.bgmVolume;
                currentSfxVolumeValue = SaveSnapshot.Instance.saveData.sfxVolume;
                masterVolumeSlider.value = currentMasterVolumeValue;
                bgmVolumeSlider.value = currentBgmVolumeValue;
                sfxVolumeSlider.value = currentSfxVolumeValue;
                SetMasterVolume(SaveSnapshot.Instance.saveData.masterVolume);
                SetBgmVolume(SaveSnapshot.Instance.saveData.bgmVolume);
                SetSfxVolume(SaveSnapshot.Instance.saveData.sfxVolume);
                break;
        }
    }
    public void RegisterToSaveManager()
    {
        Managers.SaveManager.Register(this);
    }
}
