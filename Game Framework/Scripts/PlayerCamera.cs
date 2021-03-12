using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[AddComponentMenu("Game Framework/Camera/Player Camera Controller")]
public class PlayerCamera : MonoBehaviour
{
    public enum CameraBehaviour
    {
        SimpleFollow = 0,
        ThirdPerson = 1,
    };

    public Transform CameraRig;
    public Transform CameraTarget;
    public CameraBehaviour Behaviour;
    public float CameraSpeed = 0.35f;
    public bool SupressNormalBehaviour = false;
    public float MinUpAngle = -35f;
    public float MaxUpAngle = 60f;

    Vector2 lookAt;
    Vector3 offset;
    float yAngle;
    float xAngle;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yAngle = CameraRig.eulerAngles.y;
        xAngle = CameraRig.eulerAngles.x;

        if (CameraTarget != null)
            offset = CameraTarget.position - CameraRig.position;
    }

    void LateUpdate()
    {
        yAngle += lookAt.x * CameraSpeed;
        xAngle -= lookAt.y * CameraSpeed;

        xAngle = Mathf.Clamp(xAngle, MinUpAngle, MaxUpAngle);

        if (!SupressNormalBehaviour)
        {
            if (Behaviour == CameraBehaviour.ThirdPerson)
                CameraRig.transform.rotation = Quaternion.Euler(xAngle, yAngle, 0);
        }

        CameraRig.position = CameraTarget.position - offset;
    }

    public void OnCameraMovement(InputAction.CallbackContext ctx)
    {
        lookAt = ctx.ReadValue<Vector2>();
    }
}
