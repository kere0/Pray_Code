using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextColorTransition : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Define.SelectMode selectMode;
    public GameModeManager gameModeManager;
    private TextMeshProUGUI text;
    private Color originColor = new Color(0.7735f, 0.7735f, 0.7735f, 1f);
    private Color highlightedColor = new Color(0.7264151f, 0.6108491f, 0, 1f);
    public Image fadeImage;
    private bool isClicked = false;
    void Awake()
    {
        TryGetComponent(out text);
    }

    void Start()    
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = highlightedColor;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameModeManager.modeSelected == false)
        {
            if (isClicked == false)
            {
                gameModeManager.modeSelected = true;
                isClicked = true;
                switch (selectMode)
                {
                    case Define.SelectMode.NewGame :
                        GameManager.SelectMode = selectMode;
                        break;
                    case Define.SelectMode.LoadGame :
                        GameManager.SelectMode = selectMode;
                        break;
                }
                StartCoroutine(FadeInWhileLoading("MainScene"));
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = originColor;
    }
    IEnumerator FadeInWhileLoading(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; 

        float duration = 5f;
        float t = 0f;
        Color c = fadeImage.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, t / duration);
            fadeImage.color = c;
            yield return null;
        }
        c.a = 1;
        fadeImage.color = c;
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
    }
}
