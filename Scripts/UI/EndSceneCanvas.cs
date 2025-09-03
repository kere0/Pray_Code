using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndSceneCanvas : MonoBehaviour
{
    private Image image;
    public float fadeDuration = 3f;
    public float time;
    private bool OnfadeIn;
    private bool fadeInEnd = false;
    private float sceneChangeTime;
    private void Start()
    {
        image = transform.GetChild(0).GetComponent<Image>();
        OnfadeIn = false;
    }

    private void Update()
    {
        if (OnfadeIn == false)
        {
            time += Time.deltaTime;

            if (time >= 1f)
            {
                StartCoroutine(FadeIn());
                OnfadeIn = true;
            }
        }
        SceneChange();
    }
    private IEnumerator FadeIn()
    {
        float timeElapsed = 0f;
        Color color = image.color; 
        Color targetColor = new Color(color.r, color.g, color.b, 1f); 

        while (timeElapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(color.a, targetColor.a, timeElapsed / fadeDuration);
            image.color = new Color(color.r, color.g, color.b, alpha);

            timeElapsed += Time.deltaTime;
            yield return null; 
        }
        image.color = targetColor;
        fadeInEnd = true;
    }
    void SceneChange()
    {
        if (fadeInEnd == true)
        {
            sceneChangeTime += Time.deltaTime;
            if (sceneChangeTime >= 1f)
            {
                Managers.DataManager.SetResourceData();
                SceneManager.LoadScene("StartScene");
            }
        }
    }
}
