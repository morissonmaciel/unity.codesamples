using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPS_Movement : MonoBehaviour
{
    public float NormalSpeed = 2.5f;
    public float BackwardsSpeed = 1.5f;
    public float JumpPower = 2.5f;

    public Animator ModelAnimator;
    public TPS_Events EventsHost;

    CharacterController controller;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    Vector3 movement;

    void Update()
    {
        var input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (controller.isGrounded)
        {
            movement = controller.transform.TransformDirection(input);
            movement *= (input.z > 0 ? NormalSpeed : BackwardsSpeed);

            if (Input.GetButton("Jump"))
            {
                movement.y += Mathf.Sqrt(JumpPower * -2.0f * Physics.gravity.y);
            }

            if (ModelAnimator != null)
            {
                ModelAnimator.SetFloat("Velocity Z", input.z);
                ModelAnimator.SetFloat("Velocity X", input.x);
                ModelAnimator.SetBool("Is Jumping", Input.GetButton("Jump"));
            }
        }

        if (EventsHost)
            EventsHost.PlayerMovementMagnitude = input.magnitude;

        movement.y -= -1.0f * Physics.gravity.y * Time.deltaTime;
        controller.Move(movement * Time.deltaTime);
    }
}
