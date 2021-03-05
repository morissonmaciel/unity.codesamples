using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Third Person Game/Player/Movement", 100)]
public class TPS_Movement : MonoBehaviour
{
    [Header("Physics")]
    public Transform GroundChecker;
    public bool IsGrounded = true;
    public bool CanMove = true;
    public bool CanJump = true;
    public bool JustLanded = false;

    [Header("Traversal")]
    public float NormalSpeed = 2.5f;
    public float SprintModifier = 1.5f;
    public float BackwardsSpeed = 1.5f;
    public float JumpPower = 1.75f;
    [Range(0.5f, 5f)] public float TimeBeforeMoveAgain = 0.5f;
    [Range(0.5f, 5f)] public float TimeBetweenJumps = 0.65f;

    public string JumpInput = "Jump";
    public string SprintInput = "Fire3";

    [Header("Animator Interaction")]
    public Animator ModelAnimator;
    public string MovementParameter = "Movement";
    public string ForwardMoveParameter = "MoveForward";
    public string SideMoveParameter = "MoveSides";
    public string SprintParameter = "Sprinting";
    public string JumpingParameter = "Jumping";

    [Header("Framework Interaction")]
    public TPS_Camera Camera;
    public TPS_Events EventsHost;

    CharacterController controller;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    Vector3 moveDirection;

    float movement;
    float movementDampVelocity;
    float sprint;
    float sprintDampVelocity;

    void FixedUpdate()
    {
        var input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        var isSprinting = Input.GetButton(SprintInput);

        IsGrounded = CheckIsGroundedPhysicaly(out JustLanded);

        if (JustLanded)
        {
            CanMove = false;

            StartCoroutine(WaitForNextJump());
            StartCoroutine(WaitForNextMovement());
        }

        if (IsGrounded && CanMove)
        {
            moveDirection = controller.transform.TransformDirection(input);
            moveDirection *= (input.z > 0 ? NormalSpeed : BackwardsSpeed);
            moveDirection *= (isSprinting && input.x == 0 ? SprintModifier : 1.0f);

            if (Input.GetButton(JumpInput) && CanJump)
            {
                CanJump = false;
                moveDirection.y += Mathf.Sqrt(JumpPower * -1.5f * Physics.gravity.y);
            }

            if (ModelAnimator != null)
            {
                var mag = Mathf.SmoothDamp(movement, Mathf.Abs(input.x) + Mathf.Abs(input.z), ref movementDampVelocity, 0.01f);
                movement = mag;

                var spr = Mathf.SmoothDamp(sprint, isSprinting ? 1.0f : 0.0f, ref sprintDampVelocity, 0.1f);
                sprint = spr;

                ModelAnimator.SetFloat(MovementParameter, movement);
                ModelAnimator.SetFloat(ForwardMoveParameter, input.z);
                ModelAnimator.SetFloat(SideMoveParameter, input.x);
                ModelAnimator.SetFloat(SprintParameter, sprint);
            }
        }

        if (!CanMove)
        {
            input = Vector3.zero;
            moveDirection = Vector3.zero;

            ModelAnimator.SetFloat(MovementParameter, 0.0f);
            ModelAnimator.SetFloat(ForwardMoveParameter, 0.0f);
            ModelAnimator.SetFloat(SideMoveParameter, 0.0f);
            ModelAnimator.SetFloat(SprintParameter, 0.0f);
        }

        if (ModelAnimator != null)
        {
            ModelAnimator.SetBool("IsGrounded", IsGrounded);
            ModelAnimator.SetBool("CanMove", CanMove);
        }
            

        if (EventsHost)
            EventsHost.PlayerMovementMagnitude = input.magnitude;

        moveDirection.y -= -1.0f * Physics.gravity.y * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    bool CheckIsGroundedPhysicaly(out bool justLanded)
    {
        justLanded = false;
        if (GroundChecker == null) return false;

        var origin = GroundChecker.position;
        var direction = -1.0f * GroundChecker.up;
        var distance = 0.1f;

        var grounded = Physics.Raycast(origin, direction, distance);

        justLanded = IsGrounded == false && grounded == true;
        return grounded;
    }

    IEnumerator WaitForNextJump()
    {
        yield return new WaitForSeconds(TimeBetweenJumps);
        CanJump = true;
    }

    IEnumerator WaitForNextMovement()
    {
        yield return new WaitForSeconds(TimeBeforeMoveAgain);
        CanMove = true;
    }
}
