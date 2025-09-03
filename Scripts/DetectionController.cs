using System.Collections.Generic;
using EPOOutline;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DetectionController : MonoBehaviour
{
    public float expandSpeed = 1.5f;
    public float detectionDuration = 2f;
    private float currentTime = 0f;
    private bool isDetecting = false;
    public Volume postProcessVolume;
    private ColorAdjustments colorAdjustments;
    private LayerMask postProcessLayer;
    HashSet<GameObject> questObjects = new HashSet<GameObject>();
    private bool detectActive = false;
    void Start()
    {

        postProcessLayer = LayerMask.GetMask("Item", "Box", "Monster", "Shop", "SavePoint");
        if (postProcessVolume != null && postProcessVolume.profile != null)
        {
            postProcessVolume.profile.TryGet(out colorAdjustments);
        }
    }
    void Update()
    {
        if (isDetecting == false)
        {
            if (Input.GetKeyDown(KeyCode.BackQuote)) 
            {
                isDetecting = true;
                currentTime = 0f;
            }
        }
        if (isDetecting)
        {
            currentTime += expandSpeed * Time.deltaTime;
            if (currentTime > detectionDuration)
            {
                isDetecting = false;
            }
            ApplyPostProcessingEffect(currentTime / detectionDuration);
            foreach (Collider col in Physics.OverlapSphere(transform.position, 200f, postProcessLayer))
            {
                if (!questObjects.Contains(col.gameObject))
                {
                    questObjects.Add(col.gameObject);
                    IQuestCheck questCheck = col.gameObject.GetComponent<IQuestCheck>();
                    if (Player.Instance.target != col.gameObject.transform)
                    {
                        Outlinable outlineable = col.gameObject.GetComponent<Outlinable>();
                        if (outlineable != null)
                        {
                            Color color = outlineable.OutlineParameters.Color;
                            color = (questCheck != null && questCheck.QuestCheck) ? new Color(1f,0.5f,0f,0.3921f) : new Color(1f,0f,0f,0.3921f);
                            outlineable.OutlineParameters.Color = color;
                            outlineable.enabled = true;
                        }
                    }
                }
            }
        }
    }
    // 흑백 효과 적용
    void ApplyPostProcessingEffect(float normalizedTime)
    {
        if (detectActive == false)
        {
            if (colorAdjustments != null)
            {
                colorAdjustments.saturation.value = Mathf.Lerp(0, -100, normalizedTime);
            }

            if (normalizedTime >= 1f)
            {
                detectActive = true;
                currentTime = 0f;
                StartCoroutine(DetectEndCoroutine());
            }
        }
    }

    IEnumerator DetectEndCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < detectionDuration)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsed / detectionDuration);
            if (colorAdjustments != null)
            {
                colorAdjustments.saturation.value = Mathf.Lerp(-100f, 0f, normalizedTime);
            }
            yield return null;
        }

        if (colorAdjustments != null)
        {
            colorAdjustments.saturation.value = 0f;
        }
        foreach (GameObject questObject in questObjects)
        {
            if (questObject != null)
            {
                if (questObject.GetComponent<Outlinable>() != null)
                {
                    questObject.GetComponent<Outlinable>().enabled = false;
                }
            }
        }
        questObjects.Clear();
        detectActive = false;
    }
}