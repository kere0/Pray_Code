using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[DefaultExecutionOrder(-90)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isGameStart = false;
    public float time;
    public bool isEndSceneLoad = false;
    public Action GameStartAction;
    public static Define.SelectMode SelectMode;
    public IntroSequenceController introSequenceController;
    public bool isInputEnabled = true;
    public bool isBossBattle = false;
    void Awake()
    {
        Instance = this;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;  // 마우스 커서 숨기기
        GameStartAction -= GameStart;
        GameStartAction += GameStart;
    }

    private void Start()
    {
        Managers.DataManager.SetResourceData();
        Managers.SaveManager.Init();
        Managers.SaveManager.LoadAll(SelectMode);
        if (SaveSnapshot.Instance.saveData.savePointId == "S02")
        {
            SoundManager.Instance.PlayBgm(Bgm.Chapter2Bgm);
        }
        else
        {
            SoundManager.Instance.PlayBgm(Bgm.Chapter1Bgm);
        }
        if (SaveSnapshot.Instance.saveData.savePointId == "")
        {
            TimelineManager.Instance.PlayIntro();
        }
        else
        {
            Debug.Log("그냥시작");
            introSequenceController.dollyVirtualCamera.Priority = 0;
            introSequenceController.player.gameObject.SetActive(true);
            introSequenceController.introBlinkController.gameObject.SetActive(false);
            GameStartAction?.Invoke();
        }
    }
    void GameStart()
    {
        isGameStart = true;
        CanvasController.Instance.Canvas_HUD.SetActive(true);
        //SaveManager.SetLoadData();
        Debug.Log("데이터 불러오기 gamestart에서");
    }
    public void EndSceneLoad()
    {
        if (isEndSceneLoad == false)
        {
            isEndSceneLoad = true;
            SceneManager.LoadSceneAsync("EndScene_Texture", LoadSceneMode.Additive);
        }
    }
    IEnumerator FadeInWhileLoading()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("EndScene_Texture", LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false; 

        float duration = 5f;
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
    }
}
