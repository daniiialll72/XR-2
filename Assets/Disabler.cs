using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Disabler : MonoBehaviour
{
    public Camera vrCamera;  // Assign the Main Camera
    private Quaternion lockedRotation;
    private Vector3 lockedPosition;
    private bool isPositionLocked = false;
    private bool isRotationLocked = false;

    public float moveSpeed = 5.0f;  // Speed for movement
    public float rotationSpeed = 3.0f;  // Speed for rotation
    private bool isVRActive = false;  // Detect if VR is enabled

    void Start()
    {
        if (vrCamera == null)
        {
            vrCamera = Camera.main;  
        }

        isVRActive = XRSettings.enabled;  // Detect if VR is active
    }

    private void Update()
    {
        CheckKeyboardInput();

        if (isVRActive)
        {
            HandleVRMovement();
        }
        else
        {
            HandleKeyboardMouseMovement();
        }
    }

    void CheckKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Toggle position lock
        {
            if (!isPositionLocked) lockedPosition = vrCamera.transform.position;
            isPositionLocked = !isPositionLocked;
        }

        if (Input.GetKeyDown(KeyCode.R)) // Toggle rotation lock
        {
            if (!isRotationLocked) lockedRotation = vrCamera.transform.rotation;
            isRotationLocked = !isRotationLocked;
        }
    }

    void HandleVRMovement()
    {
        if (isPositionLocked)
        {
            vrCamera.transform.position = lockedPosition;
        }

        if (isRotationLocked)
        {
            vrCamera.transform.rotation = lockedRotation;
        }
    }

    void HandleKeyboardMouseMovement()
    {
        if (!isPositionLocked)
        {
            float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
            vrCamera.transform.Translate(moveX, 0, moveZ);
        }

        if (!isRotationLocked)
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed;
            vrCamera.transform.Rotate(Vector3.up, mouseX, Space.World);
            vrCamera.transform.Rotate(Vector3.right, mouseY, Space.Self);
        }
    }
}
