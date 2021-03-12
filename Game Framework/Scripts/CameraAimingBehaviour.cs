using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[AddComponentMenu("Game Framework/Camera/Aiming Behaviour")]
public class CameraAimingBehaviour : MonoBehaviour
{
    public PlayerMovement MovementController;
    public PlayerCamera CameraController;
    public Camera playerCamera;
    public Transform NormalHint;
    public Transform AimingHint;
    public float BodyRotationSpeed = 0.75f;
    public float AimingAngleModifier = 3.5f;

    bool IsAiming;

    void LateUpdate()
    {
        MoveCameraToDesiredPosition();
        RotateBodyAndCameraToAimingTarget();
    }

    void MoveCameraToDesiredPosition()
    {
        var targetPosition = IsAiming ? AimingHint.position : NormalHint.position;
        var isCameraPositioned = (playerCamera.transform.position - targetPosition).magnitude == 0;

        if (!isCameraPositioned)
        {
            var position = Vector3.MoveTowards(playerCamera.transform.position, targetPosition, 0.01f);
            playerCamera.transform.position = position;
        }
    }

    private void RotateBodyAndCameraToAimingTarget()
    {
        if (IsAiming)
        {
            var angle = CameraController.CameraRig.eulerAngles.y;
            MovementController.Controller.transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
        }
    }

    public void OnAiming(InputAction.CallbackContext ctx)
    {
        IsAiming = ctx.ReadValueAsButton();
    }
}
