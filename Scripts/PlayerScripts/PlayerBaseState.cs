using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public abstract class PlayerBaseState
{
    public float currentSpeed;
    public const float walkSpeed = 3f;
    private float runSpeed = 6f;
    private float rotateSpeed = 10f;
    private Vector3 moveDirection;
    public Vector2 axisInput;
    public void HandleMovement(Player player)
    {
        if (GameManager.Instance.isInputEnabled == true)
        {
            axisInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            
            currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
            Vector3 camForward = player.camera.transform.forward;
            Vector3 camRight = player.camera.transform.right;
            camForward.y = 0f;
            camRight.y = 0f;

            moveDirection = (camForward * axisInput.y + camRight * axisInput.x).normalized;
            moveDirection.y = 0f;
            player.cc.Move(((moveDirection * currentSpeed)+player.forceReceiver.Movement) * Time.deltaTime);
            if (axisInput == Vector2.zero)
            {
                currentSpeed = 0f;
            }
            else
            {
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(moveDirection), rotateSpeed * Time.deltaTime);
            }
        }
    }
    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();
}
