using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cinemachine;
using UnityEngine;

public class MouseSensitiveManager : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.GameStartAction -= SetMouseSensitivity;
        GameManager.Instance.GameStartAction += SetMouseSensitivity;
    }
    public void SetHorizontalSensitivity(float sensitivity)
    {
        CameraController.Instance.FreeLookCamera.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 300f + sensitivity * 500f;
    }
    public void SetVerticalSensitivity(float sensitivity)
    {
        CameraController.Instance.FreeLookCamera.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 2f + sensitivity * 3f;
    }
    void SetMouseSensitivity()
    {
        switch (GameManager.SelectMode)
        {
            case Define.SelectMode.NewGame:
                SetHorizontalSensitivity(SettingUIController.defaultValue);
                SetVerticalSensitivity(SettingUIController.defaultValue);
                break;
            case Define.SelectMode.LoadGame:
                if (File.Exists(Managers.SaveManager.SavePath) == false)
                {
                    SetHorizontalSensitivity(SettingUIController.defaultValue);
                    SetVerticalSensitivity(SettingUIController.defaultValue);
                }
                SetHorizontalSensitivity(SaveSnapshot.Instance.saveData.currentMouseHorizontalValue);
                SetVerticalSensitivity(SaveSnapshot.Instance.saveData.currentMouseVerticalValue);
                break;
        }
    }
}
