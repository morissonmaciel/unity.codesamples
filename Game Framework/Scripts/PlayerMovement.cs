using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[AddComponentMenu("Game Framework/Player/Player Movement Controller")]
public class PlayerMovement : MonoBehaviour
{
    public delegate void PlayerMovementDelegate(Vector2 input, bool isRunning);

    public CharacterController Controller;
    public PlayerCamera Camera;
    public float NormalSpeed = 1.75f;
    public float RunSpeed = 3.75f;
    public float JumpHeight = 1.5f;
    public float Gravity = 9.8f;

    public event PlayerMovementDelegate OnPlayerMovement;

    Vector2 input;
    Vector3 movement;
    bool running;
    bool jumping;
    float dampRotation;

    void FixedUpdate()
    {
        ComputeRotation();
        ComputeMovement();
    }

    void ComputeMovement()
    {
        var cameraAngle = 0.0f;

        if (Camera != null)
            cameraAngle = Camera.transform.eulerAngles.y;

        if (Controller.isGrounded)
        {
            movement = new Vector3(input.x, 0, input.y);
            movement *= running ? RunSpeed : NormalSpeed;

            if (jumping)
                movement.y += Mathf.Sqrt(2.0f * JumpHeight * Gravity);
        }

        movement.y -= Gravity * Time.deltaTime;

        Controller.Move((Quaternion.Euler(0, cameraAngle, 0) * movement) * Time.deltaTime);
    }

    void ComputeRotation()
    {
        var cameraAngle = 0.0f;

        if (Camera != null)
            cameraAngle = Camera.transform.eulerAngles.y;

        if (input.magnitude > 0)
        {
            var targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + cameraAngle;
            var angle = Mathf.SmoothDampAngle(Controller.transform.eulerAngles.y, targetAngle, ref dampRotation, 0.1f);

            Controller.transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        input = ctx.ReadValue<Vector2>();

        if (OnPlayerMovement != null)
            OnPlayerMovement(input, running);
    }

    public void OnRun(InputAction.CallbackContext ctx)
    {
        running = ctx.ReadValueAsButton();

        if (OnPlayerMovement != null)
            OnPlayerMovement(input, running);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        jumping = ctx.ReadValueAsButton();
    }
}
