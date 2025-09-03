using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using EPOOutline;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    public GameObject FreeLookCamera;
    public GameObject VirtualCamera;
    public Player player;
    private float maxDistance = 100.0f;
    private Transform nearTarget;
    private Transform tempTarget;
    private float dectectTargetDistance;
    private float minDistance;
    public GameObject lookAtTarget;
    public bool isLockOn = false;
    void Awake()
    {
        Instance = this;
        VirtualCamera.SetActive(false);
    }
    private void Update()
    {
        DetectTarget();
        LockOnTarget();
    }
    private void DetectTarget()
    {
        nearTarget = null;
        minDistance = float.MaxValue;

        Collider[] detectTarget = Physics.OverlapSphere(player.transform.position, 10f, LayerMask.GetMask("Monster"));
        
        for (int i = 0; i < detectTarget.Length; i++)
        {
            var col = detectTarget[i];
            if (col == null) continue;

            var damageable = col.GetComponent<IDamageAble>();
            if (damageable == null || damageable.IsDie) continue;

            float dist = Vector3.Distance(col.transform.position, player.transform.position);
            if (dist < minDistance)
            {
                nearTarget = col.transform;
                minDistance = dist;
            }
        }
    }
    public void LockOnTarget()
    {
        if (player.target == null)
        {
            //player.target.GetComponent<Outlinable>().enabled = false;
            isLockOn = false;
            FreeLookCamera.transform.position = VirtualCamera.transform.position;
            FreeLookCamera.transform.rotation = VirtualCamera.transform.rotation;
            FreeLookCamera.SetActive(true);
            VirtualCamera.SetActive(false);
            VirtualCamera.GetComponent<CinemachineVirtualCamera>().LookAt = null;
            player.isTarget = false;
        }
        else
        {
            if (player.target.GetComponent<IDamageAble>().IsDie == true)
            {
                isLockOn = false;
                FreeLookCamera.transform.position = VirtualCamera.transform.position;
                FreeLookCamera.transform.rotation = VirtualCamera.transform.rotation;
                FreeLookCamera.SetActive(true);
                VirtualCamera.SetActive(false);
                VirtualCamera.GetComponent<CinemachineVirtualCamera>().LookAt = null;
                player.isTarget = false;
            }
        }
        if(Input.GetMouseButtonDown(2))
        {
            if(player.target == null)
            {
                if (nearTarget != null)
                {
                    isLockOn = true;
                    VirtualCamera.transform.position = FreeLookCamera.transform.position;
                    VirtualCamera.transform.rotation = FreeLookCamera.transform.rotation;
                    player.target = nearTarget;
                    //player.target = hit.collider.transform;
                    CameraController.Instance.VirtualCamera.SetActive(true);
                    CameraController.Instance.FreeLookCamera.SetActive(false);
                    VirtualCamera.GetComponent<CinemachineVirtualCamera>().LookAt = player.target;
                    player.isTarget = true;
                    player.target.GetComponent<Outlinable>().enabled = true;
                    Debug.Log("락온!!!!!!!!!!!!!!!!!!!!!!!");
                }
            }
            else
            {
                player.target.GetComponent<Outlinable>().enabled = false;
                isLockOn = false;
                FreeLookCamera.transform.position = VirtualCamera.transform.position;
                FreeLookCamera.transform.rotation = VirtualCamera.transform.rotation;
                FreeLookCamera.SetActive(true);
                VirtualCamera.SetActive(false);
                VirtualCamera.GetComponent<CinemachineVirtualCamera>().LookAt = null;
                player.target = null;
                player.isTarget = false;
                Debug.Log("락온취소!!!!!!!!!!!!!!!!!!!!!!");
            }
        }
    }
}
