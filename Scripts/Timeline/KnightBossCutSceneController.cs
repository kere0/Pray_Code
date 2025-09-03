using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;

public class KnightBossCutSceneController : MonoBehaviour
{
    private int currentPath = 0;
    public CinemachineVirtualCamera  dollyVirtualCamera;
    private CinemachineTrackedDolly dolly;
    public GameObject OutlineCamera;
    public bool knightBossActive;
    private void Awake()
    {
        dolly = dollyVirtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
    }
    private void Start()
    {
        TimelineManager.Instance.deathKnightAction -= StartKnightBossCutScene;
        TimelineManager.Instance.deathKnightAction += StartKnightBossCutScene;
    }
    private async Task StartKnightBossCutScene()
    {
        //OutlineCamera.SetActive(false);
        GameManager.Instance.isInputEnabled = false;
        CameraController.Instance.FreeLookCamera.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 0f;
        CameraController.Instance.FreeLookCamera.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 0f;

        dollyVirtualCamera.gameObject.SetActive(true);
        //flameBoss.SetActive(true);
        await MovePath(3);
        await MovePath(3);
        knightBossActive = true;
        await MovePath(3);

        dollyVirtualCamera.gameObject.SetActive(false);
        StartCoroutine(InputEnableCoroutine());
    }
    private async Task MovePath(float duration)
    {
        float elapsedTime = 0f;
        float start = currentPath;
        float end = currentPath + 1;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            dolly.m_PathPosition = Mathf.Lerp(start, end, elapsedTime / duration);
            await Task.Yield();
        }
        currentPath++;
    }
    IEnumerator InputEnableCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        OutlineCamera.SetActive(true);
        GameManager.Instance.isInputEnabled = true;
        CameraController.Instance.FreeLookCamera.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 300f;
        CameraController.Instance.FreeLookCamera.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 2f;
    }
}
