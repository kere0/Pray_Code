using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathState : PlayerBaseState
{
    Player player;
    private float jumpTimer = 0.5f;
    private float jumpStay = 0.1f;
    private float durationTime = 3f;
    private float fadeDuration = 1f;
    private bool fadeInStart = false;
    public PlayerDeathState(Player player)
    {
        this.player = player;
    }
    public override void Enter()
    {
        player.animator.Play("Die");
    }

    public override void Execute()
    {
        jumpTimer -= Time.deltaTime;
        durationTime -= Time.deltaTime;
        if(jumpTimer < 0f)
        {   
            jumpStay -= Time.deltaTime;
            if (jumpStay < 0f)
            {
                player.forceReceiver.isGravity = true;
                player.cc.Move(player.forceReceiver.Movement* Time.deltaTime);
            }
        }
        else
        {
            player.forceReceiver.isGravity = false;
            player.cc.Move(Vector3.up * (5f * Time.deltaTime));
        }
        if (fadeInStart == false)
        {
            if (durationTime < 0f)
            {
                player.CoroutineController(FadeIn());
                fadeInStart = true;
            }
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

            timeElapsed += Time.deltaTime*0.5f;
            yield return null; 
        }
        HUDController.Instance.endImage.color = targetColor;
        SceneManager.LoadScene("GameOverScene");
    }

    public override void Exit()
    {
        
    }
}