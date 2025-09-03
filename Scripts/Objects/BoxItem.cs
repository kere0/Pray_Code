using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BoxItem : BaseBox
{
    public Transform blueItem;
    public Transform orangeItem;
    public Transform yellowItem;
    void Awake()
    {
        GameManager.Instance.GameStartAction -= Init;
        GameManager.Instance.GameStartAction += Init;
    }
    void Init()
    {
        // TODO 게임스타트시
        boxCap = transform.GetChild(1);
        Interaction.Instance.boxOpenAction -= BoxOpen;
        Interaction.Instance.boxOpenAction += BoxOpen;
    }
    private void Update()
    {
        BoxOpen();
    }

    void BoxOpen()
    {
        if (isBoxOpen == false)
        {
            if (isOpen == true)
            {
                StartCoroutine(BoxOpenCoroutine());
                isBoxOpen = true;
            }
        }
    }
    void ItemActive()
    {
        blueItem.DOJump(blueItem.transform.position + transform.TransformDirection(new Vector3(1.5f , 0f, -0.5f)), 1.5f, 1, 1f);
        orangeItem.DOJump(orangeItem.transform.position + transform.TransformDirection(new Vector3(1.5f , 0f, 0f)), 1.5f, 1, 1.25f);
        yellowItem.DOJump(yellowItem.transform.position + transform.TransformDirection(new Vector3(1.5f , 0f, 0.5f)), 1.5f, 1, 1.5f);
    }

    IEnumerator BoxOpenCoroutine()
    {
        float duration = 0.5f; // 열리는 시간
        float elapsed = 0f;

        Quaternion startRot = boxCap.transform.rotation;
        Quaternion endRot = Quaternion.Euler(boxCap.transform.eulerAngles + new Vector3(0f, 0f, 51f)); // 예: 위로 90도 열기

        while (elapsed < duration)
        {
            boxCap.transform.rotation = Quaternion.Lerp(startRot, endRot, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        boxCap.transform.rotation = endRot; // 마지막 보정
        ItemActive();
    }
}
