using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Game/Player/Player Movement")]
public class PlayerMovement : MonoBehaviour
{
    public float NormalSpeed = 1.75f;
    public float RunSpeed = 3.25f;
    public float Gravity = 9.8f;

    CharacterController controller;
    Vector2 input;
    bool running;
    bool jumping;

    float rotationDamp;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    Vector3 movement;
    void LateUpdate()
    {
        ComputePlayerRotation();
        ComputeMovement();

        Debug.Log(movement);
    }

    void ComputeMovement()
    {
        if (input.magnitude > 0f)
        {
            movement = new Vector3(input.x, 0.0f, input.y);
            movement *= running ? RunSpeed : NormalSpeed;
            movement.y -= Gravity * Time.deltaTime;

            controller.Move(movement * Time.deltaTime);
        }
    }

    void ComputePlayerRotation()
    {
        if (input.magnitude > 0)
        {
            var targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
            var angle = Mathf.SmoothDampAngle(controller.transform.eulerAngles.y, targetAngle, ref rotationDamp, 0.1f);

            controller.transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
        }
    }

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        input = ctx.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext ctx)
    {
        running = ctx.ReadValueAsButton();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        jumping = ctx.ReadValueAsButton();
    }
}
