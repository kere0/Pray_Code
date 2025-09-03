using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneOpen : MonoBehaviour
{
    public static  EndSceneOpen Instance;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private ParticleSystem ps;
    private Material potalMaterial;
    private Renderer particleRenderer; 
    private BoxCollider boxCollider;
    public GameObject movePoint;
    public float fadeDuration = 1f;
    public bool isEndSceneTrigger = false;
    void Awake()
    {
        Instance = this;
        TryGetComponent(out ps);
        TryGetComponent(out particleRenderer);
        TryGetComponent(out boxCollider);
        potalMaterial = Resources.Load<Material>("PotalMaterial");
    }

    void Update()
    {
        if (GameManager.Instance.isEndSceneLoad == true)
        {
            if (ps.isPlaying && ps.time >= 1)
            {
                ps.Pause();
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                particleRenderer.material = potalMaterial;
                boxCollider.enabled = true;
            }
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("OnParticleCollision@@@@@@@@@@@@@@@@");
            GameManager.Instance.isInputEnabled = false;
            StartCoroutine(PlayerMove());
            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator PlayerMove()
    {
        isEndSceneTrigger = true;
        CameraController.Instance.FreeLookCamera.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 0f;
        CameraController.Instance.FreeLookCamera.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 0f;
        //float duration = 1f;
        float elapsedTime = 0;
        Vector3 startPos = Player.Instance.transform.position;
        Vector3 endPos = movePoint.transform.position;
        while (fadeDuration >= elapsedTime)
        {
            elapsedTime += Time.deltaTime;
            // float t = Mathf.Clamp01(elapsedTime / duration);
            // Player.Instance.transform.position = Vector3.Lerp(startPos, endPos, t);
            Player.Instance.cc.Move((movePoint.transform.position - Player.Instance.transform.position).normalized * PlayerBaseState.walkSpeed * Time.deltaTime);
            Player.Instance.animator.SetFloat(Speed, 1f, PlayerBaseState.walkSpeed, Time.deltaTime);
            yield return null;
        }
    }
    private IEnumerator FadeIn()
    {
        float timeElapsed = 0f;
        Color color = HUDController.Instance.endImage.color; 
        Color targetColor = new Color(color.r, color.g, color.b, 1f); 

        while (timeElapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(color.a, targetColor.a, timeElapsed / fadeDuration);
            HUDController.Instance.endImage.color = new Color(color.r, color.g, color.b, alpha);

            timeElapsed += Time.deltaTime;
            yield return null; 
        }
        HUDController.Instance.endImage.color = targetColor;
        Managers.PoolManager.pools.Clear();
        Managers.DataManager.DataTableClear();
        Inventory.Instance.items.Clear();
        PlayerSkill.Instance.skillData.Clear();
        CombatSystem.Instance.creatureDic.Clear();
        SceneManager.LoadSceneAsync("EndScene");
        SoundManager.Instance.PlayBgm(Bgm.TitleBgm);
    }
}
