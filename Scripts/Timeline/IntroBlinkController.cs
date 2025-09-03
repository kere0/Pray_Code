using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class IntroBlinkController : MonoBehaviour
{
    [SerializeField] private Volume introPostProcessVolume;
    [SerializeField] private Image introImageTop; 
    [SerializeField] private Image introImageBottom;
    private DepthOfField depthOfField;
    //private float currentBlurAmount;
    void Awake()
    {
        introPostProcessVolume.profile.TryGet(out depthOfField);
    }
    public void RemoveBlink()
    {
        introImageTop.enabled = false;
        introImageBottom.enabled = false;
    }
    public async Task PlayBlink(int blurAmount, float blinkAmount, float duration, Action<bool> onComplete = null)
    {
        var tcs = new TaskCompletionSource<bool>();
        StartCoroutine(PlayBlinkCoroutine(blurAmount, blinkAmount, duration, (end) =>
        {
            tcs.SetResult(true);
        }));
        await tcs.Task;
    }
    IEnumerator PlayBlinkCoroutine(int blurAmount, float blinkAmount, float duration, Action<bool> onComplete = null)
    { 
        float elapsedTime = 0f;
        Vector3 topStart = introImageTop.transform.localPosition;
        Vector3 topEnd = topStart + new Vector3(0, blinkAmount, 0);

        Vector3 bottomStart = introImageBottom.transform.localPosition;
        Vector3 bottomEnd = bottomStart - new Vector3(0, blinkAmount, 0);
        
        float startBlur = depthOfField.focalLength.value;
        float endBlur = blurAmount;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsedTime / duration)); // 오차 없애기위해서
            introImageTop.transform.localPosition = Vector3.Lerp(topStart, topEnd, t);
            introImageBottom.transform.localPosition = Vector3.Lerp(bottomStart, bottomEnd, t);
            depthOfField.focalLength.value = Mathf.Lerp(startBlur, endBlur, elapsedTime / duration);
            yield return null;
        }
        onComplete?.Invoke(true);
    }
}
