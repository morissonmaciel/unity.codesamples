using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[AddComponentMenu("Game Framework/Player/Player Movement Controller")]
public class PlayerMovement : MonoBehaviour
{
    public delegate void PlayerMovementDelegate(Vector2 input, bool isRunning);
    public delegate void PlayerJumpingDelegate(bool jumping);

    public CharacterController Controller;
    public PlayerCamera Camera;
    public float NormalSpeed = 1.75f;
    public float JogSpeed = 2.25f;
    public float RunSpeed = 4.5f;
    public float JumpHeight = 1.5f;
    public float Gravity = 9.8f;

    public event PlayerMovementDelegate OnPlayerMovement;
    public event PlayerJumpingDelegate OnPlayerJumping;

    Vector2 input;
    Vector3 movement;
    bool running;
    bool jumping;
    bool grounded = true;
    float dampRotation;

    void FixedUpdate()
    {
        ComputeRotation();
        ComputeMovement();

        RaycastHit hit;
        var hitted = Physics.Raycast(
            Controller.transform.position, Vector3.down, out hit, grounded ? Controller.stepOffset : JumpHeight * Controller.stepOffset);
        var isGrounded = hitted && hit.distance <= 0.1f;

        grounded = isGrounded;

        if (OnPlayerJumping != null)
            OnPlayerJumping(!(hitted && hit.distance <= 0.35f));
    }

    void ComputeMovement()
    {
        var cameraAngle = 0.0f;

        if (Camera != null)
            cameraAngle = Camera.transform.eulerAngles.y;

        if (Controller.isGrounded)
        {
            movement = new Vector3(input.x, 0, input.y);
            movement *= running ? RunSpeed : input.magnitude > 0.5f ? JogSpeed : NormalSpeed;

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

        if (OnPlayerJumping != null)
            OnPlayerJumping(true);
    }
}
