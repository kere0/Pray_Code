using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;

public class FlameBossCutSceneController : MonoBehaviour
{
    private int currentPath = 0;
    public CinemachineVirtualCamera  dollyVirtualCamera;
    private CinemachineTrackedDolly dolly;
    public GameObject OutlineCamera;
    public GameObject bossSpawnDecal;
    public GameObject flameBoss;
    private void Awake()
    {
        dolly = dollyVirtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
    }
    private void Start()
    {
        TimelineManager.Instance.flameBossAction -= StartFlameBossCutScene;
        TimelineManager.Instance.flameBossAction += StartFlameBossCutScene;
        flameBoss.SetActive(false);

    }
    private async Task StartFlameBossCutScene()
    {
        OutlineCamera.SetActive(false);
        GameManager.Instance.isInputEnabled = false;
        CameraController.Instance.FreeLookCamera.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 0f;
        CameraController.Instance.FreeLookCamera.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 0f;

        dollyVirtualCamera.gameObject.SetActive(true);
        bossSpawnDecal.SetActive(true);
        flameBoss.SetActive(true);
        flameBoss.GetComponent<FlameBoss>().animator.Play("In");
        await MovePath(3);
        await MovePath(3);
        await MovePath(8);
        dollyVirtualCamera.gameObject.SetActive(false);
        bossSpawnDecal.SetActive(false);
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
        yield return new WaitForSeconds(1.85f);
        OutlineCamera.SetActive(true);
        GameManager.Instance.isInputEnabled = true;
        CameraController.Instance.FreeLookCamera.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 300f;
        CameraController.Instance.FreeLookCamera.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 2f;
    }
}
