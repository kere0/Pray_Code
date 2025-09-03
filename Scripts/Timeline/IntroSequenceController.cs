using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class IntroSequenceController : MonoBehaviour
{
    public CinemachineVirtualCamera  dollyVirtualCamera;
    [SerializeField] private CinemachineSmoothPath path;
    public IntroBlinkController introBlinkController;
    public Player player;
    [SerializeField] private Transform currentSight;
    [SerializeField] private Transform walkSight;
    [SerializeField] private Transform backSight;
    private Vector3 startSight;
    private int totalPoints;
    private CinemachineTrackedDolly dolly;
    private int currentWaypoint = 0;
   public enum CameraState
    {
        Standing,
        Walking,
        Backing,
        FinalMove
    }
    private void Awake()
    {
        dolly = dollyVirtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        startSight = currentSight.position;
        TimelineManager.Instance.introAction -= IntroCameraMove;
        TimelineManager.Instance.introAction += IntroCameraMove;
    }
    public async Task IntroCameraMove()
    {
        GameManager.Instance.isInputEnabled = false;
        dollyVirtualCamera.gameObject.SetActive(true);
        dolly.m_PathPosition = 0;
        dollyVirtualCamera.LookAt = currentSight;
        await introBlinkController.PlayBlink(70, 300f, 3f);
        await introBlinkController.PlayBlink(50, -300f, 3f);
        await introBlinkController.PlayBlink(50, 500f, 3f);
        await introBlinkController.PlayBlink(35, -350f, 3f);
        await introBlinkController.PlayBlink(1, 800f, 7f);
        introBlinkController.RemoveBlink();
        Debug.Log("고개 들기 시작");
        await MovePath(7f, CameraState.Standing);
        await MovePath(5f, CameraState.Walking);
        await MovePath(2f, CameraState.Backing);
        await MovePath(2f, CameraState.FinalMove);
        dollyVirtualCamera.gameObject.SetActive(false);
        CameraController.Instance.FreeLookCamera.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 0f;
        CameraController.Instance.FreeLookCamera.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 0f;
        await Task.Delay(100);
        GameManager.Instance.GameStartAction?.Invoke();

        await Task.Delay(2000);
        GameManager.Instance.isInputEnabled = true;
        CameraController.Instance.FreeLookCamera.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 300f;
        CameraController.Instance.FreeLookCamera.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 2f;
    }
    private async Task MovePath(float duration, CameraState state)
    {
        float elapsedTime = 0f;
        float start = currentWaypoint;
        float end = currentWaypoint + 1;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsedTime / duration));
            dolly.m_PathPosition = Mathf.Lerp(start, end, t);
            switch (state)
            {
                case CameraState.Standing:
                    currentSight.position = Vector3.Lerp(startSight, walkSight.position, t);
                    break;
                case CameraState.Backing:
                    currentSight.position = Vector3.Lerp(walkSight.position, backSight.position, t);
                    break;
                case CameraState.FinalMove:
                    if (t >= 0.25f && player.gameObject.activeSelf == false)
                    {
                        player.gameObject.SetActive(true);
                    }
                    break;
            }
            await Task.Yield();
        }
        currentWaypoint++;
    }
}
