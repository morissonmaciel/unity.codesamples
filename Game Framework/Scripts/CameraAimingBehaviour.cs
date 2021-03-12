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

    Vector2 lookAt;
    bool IsAiming;

    float yAngle;
    float xAngle;

    void Start()
    {
        if (CameraController.CameraRig != null)
            yAngle = CameraController.CameraRig.eulerAngles.y;
            xAngle = CameraController.CameraRig.eulerAngles.x;
    }


    void LateUpdate()
    {
        MoveCameraToDesiredPosition();
        //RotateBodyAndCameraToAimingTarget();
    }

    void MoveCameraToDesiredPosition()
    {
        var targetPosition = IsAiming ? AimingHint.position : NormalHint.position;
        var isCameraPositioned = (playerCamera.transform.position - targetPosition).magnitude == 0;

        if (!isCameraPositioned)
        {
            var position = Vector3.MoveTowards(playerCamera.transform.position, targetPosition, 0.05f);
            playerCamera.transform.position = position;
        }
    }

    private void RotateBodyAndCameraToAimingTarget()
    {
        var targetPosition = IsAiming ? AimingHint.position : NormalHint.position;
        var isCameraRepositioning = (playerCamera.transform.position - targetPosition).magnitude == 0;

        var minAngle = AimingAngleModifier * CameraController.MinUpAngle;
        var maxAngle = AimingAngleModifier * CameraController.MaxUpAngle;

        yAngle += lookAt.x * CameraController.CameraSpeed;
        xAngle -= lookAt.y * CameraController.CameraSpeed;
        xAngle = Mathf.Clamp(xAngle, minAngle, maxAngle);

        CameraController.SupressNormalBehaviour = IsAiming;

        if (IsAiming && !isCameraRepositioning)
        {
            MovementController.Controller.transform.rotation = Quaternion.Euler(0f, yAngle * BodyRotationSpeed, 0f);

            if (xAngle > minAngle && xAngle < maxAngle)
            {
                CameraController.CameraRig.transform.RotateAround(
                    CameraController.CameraTarget.position,
                    CameraController.CameraRig.right,
                    (xAngle * 2.0f * CameraController.CameraSpeed * Mathf.Rad2Deg) * Time.deltaTime);

                xAngle = 0.0f;
            }
        }
    }

    public void OnAiming(InputAction.CallbackContext ctx)
    {
        IsAiming = ctx.ReadValueAsButton();
    }

    public void OnCameraMovement(InputAction.CallbackContext ctx)
    {
        lookAt = ctx.ReadValue<Vector2>();
    }
}
