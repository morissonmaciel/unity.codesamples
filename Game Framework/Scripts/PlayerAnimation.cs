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

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void OnEnable()
    {
        MovementController.OnPlayerMovement += OnMovement;    
    }

    void OnDisable()
    {
        MovementController.OnPlayerMovement -= OnMovement;
    }

    float dampMagnitude;
    float magnitude;

    private void FixedUpdate()
    {
        magnitude = Mathf.SmoothDamp(magnitude, playerInput.magnitude * (isRunning ? 2.0f : 1.0f), ref dampMagnitude, 0.1f);
        magnitude = Mathf.Clamp(magnitude, 0f, 2f);

        animator.SetFloat(MovementParameter, magnitude);
    }

    public void OnMovement(Vector2 input, bool running)
    {
        playerInput = input;
        isRunning = running;
    }
}
