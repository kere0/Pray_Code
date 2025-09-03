using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    public Vector3 Movement => (Vector3.up * verticalVelocity)+impact; // 중력가속도 + 추가적으로 가해지는 힘

    public float ImpactDragTime = 0.3f;
    public CharacterController Controller;
 
    float verticalVelocity; // 수직으로 적용하는 음수의 값
    public Vector3 impact;         // 캐릭터에 적용 중인 추가적인 물리적인 힘 값
    Vector3 dampingVelocity;
    public bool isGravity = true;
    private void Awake()
    {
        TryGetComponent(out Controller);
    }
    public void AddForce(Vector3 force, float dragTime) // 물리적인 힘을 가하는 함수
    {
        ImpactDragTime = dragTime;
        impact = force; // y값은 0으로 해서 넘겨줘야함
    }   
    
    public void RemoveForce()
    {
        impact = Vector3.zero;
    }

    void Update()
    {
        if (isGravity == true)
        {
            if (verticalVelocity < 0.0f && Controller.isGrounded)
            {
                verticalVelocity = -5f;
            }
            else
            {
                verticalVelocity += Physics.gravity.y * Time.deltaTime;
            }
        }
        else
        {
            verticalVelocity = 0.0f;
        }
        // 추가적인 물리적인 힘 적용
        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, ImpactDragTime); // 현재 impact가 Vector3.zero 가 될때까지 몇초에 걸쳐서 물리적힘을 줄임
    }
}