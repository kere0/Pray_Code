using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float blinkSpeed = 1f;
    public Image fadeImage; 

    private bool OnFadein = false;
    private float lerpTime = 0f;
    private bool isClicked = false;
    private void Update()
    {
        if (text != null)
        {
            Color color = text.color;
            color.a = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
            text.color = color;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isClicked == false)
            {
                isClicked = true;
                OnFadein = true;
                Managers.Instance.Clear();
            }
        }
        FadeIn();
    }
    void FadeIn()
    {
        if (OnFadein)
        {
            lerpTime += Time.deltaTime;

            fadeImage.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), lerpTime);

            if (fadeImage.color.a >= 0.98f)
            {
                Managers.DataManager.SetResourceData();
                SceneManager.LoadSceneAsync("StartScene");
                SoundManager.Instance.PlayBgm(Bgm.TitleBgm);
            }
        }     
    }
}