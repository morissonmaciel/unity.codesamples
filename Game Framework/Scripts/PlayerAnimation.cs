using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[AddComponentMenu("Game Framework/Player/Player Animation Controller")]
public class PlayerAnimation : MonoBehaviour
{
    public PlayerMovement MovementController;
    public string MovementParameter = "Movement";

    Animator animator;
    Vector2 playerInput;
    bool isRunning;
    bool isJumping;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void OnEnable()
    {
        MovementController.OnPlayerMovement += OnMovement;
        MovementController.OnPlayerJumping += OnJumping;
    }

    void OnDisable()
    {
        MovementController.OnPlayerMovement -= OnMovement;
        MovementController.OnPlayerJumping -= OnJumping;
    }

    float dampMagnitude;
    float magnitude;
    float dampJumping;
    float jumping;

    private void FixedUpdate()
    {
        magnitude = Mathf.SmoothDamp(magnitude, playerInput.magnitude * (isRunning ? 2f : 1f), ref dampMagnitude, 0.1f);
        magnitude = Mathf.Clamp(magnitude, 0f, 2f);

        jumping = Mathf.SmoothDamp(jumping, isJumping ? 1f : 0f, ref dampJumping, 0.1f);
        jumping = Mathf.Clamp(jumping, 0f, 1f);

        animator.SetFloat(MovementParameter, magnitude);
        animator.SetFloat("Jumping", jumping);
    }

    public void OnMovement(Vector2 input, bool running)
    {
        playerInput = input;
        isRunning = running;
    }

    public void OnJumping(bool jumping)
    {
        isJumping = jumping;
    }
}
