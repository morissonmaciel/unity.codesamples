using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPS_Camera : MonoBehaviour
{
    public float RotationSpeed = 1.0f;
    public float MinYAngle = -30f;
    public float MaxYAngle = 60f;
    public Transform Target;
    public Transform Player;

    public TPS_Events EventsHost;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    float mouseX = 0.0f;
    float mouseY = 0.0f;

    float dumpVelocity = 0.0f;

    void LateUpdate()
    {
        mouseX += Input.GetAxis("Mouse X") * RotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * RotationSpeed;
        mouseY = Mathf.Clamp(mouseY, MinYAngle, MaxYAngle);

        if (Target != null)
            Target.rotation = Quaternion.Euler(mouseY, mouseX, 0);

        if (EventsHost != null && Player != null && EventsHost.PlayerMovementMagnitude >= 0.1f)
        {
            var angle = Mathf.SmoothDampAngle(Player.eulerAngles.y, mouseX, ref dumpVelocity, 0.1f);
            Player.rotation = Quaternion.Euler(0, angle, 0);
        }            
    }
}
