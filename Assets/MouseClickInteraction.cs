using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MouseClickInteraction : MonoBehaviour
{
    public Camera playerCamera;
    private bool isHeld = false;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isDoubleRotationEnabled = false;

    public XRController leftController;
    public XRController rightController;
    private Transform leftHand;
    private Transform rightHand;

    private bool isVRActive = false;

    private void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        isVRActive = XRSettings.enabled;
    }

    private void Update()
    {
        if (isVRActive)
        {
            HandleVRInteraction();
        }
        else
        {
            HandleKeyboardInteraction();
        }
    }

    void HandleKeyboardInteraction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isHeld)
            {
                PickUpMagnifier();
            }
            else
            {
                DropMagnifier();
            }
        }

        if (isHeld)
        {
            FollowCamera();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            isDoubleRotationEnabled = !isDoubleRotationEnabled;
        }
    }

    void HandleVRInteraction()
    {
        bool leftGrab = leftController.inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool leftGrabPressed) && leftGrabPressed;
        bool rightGrab = rightController.inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool rightGrabPressed) && rightGrabPressed;

        if (leftGrab && rightGrab)
        {
            ApplyTwoHandedGrab(leftController.transform, rightController.transform);
        }
        else if (leftGrab)
        {
            PickUpMagnifier(leftController.transform);
        }
        else if (rightGrab)
        {
            PickUpMagnifier(rightController.transform);
        }
        else
        {
            DropMagnifier();
        }
    }

    void PickUpMagnifier(Transform handTransform = null)
    {
        isHeld = true;
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        if (handTransform != null)
        {
            transform.SetParent(handTransform);
        }
        else
        {
            transform.SetParent(playerCamera.transform);
        }
    }

    void DropMagnifier()
    {
        isHeld = false;
        transform.SetParent(null);
    }

    void FollowCamera()
    {
        transform.position = playerCamera.transform.position + playerCamera.transform.forward * 0.5f;

        if (isDoubleRotationEnabled)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, playerCamera.transform.rotation * Quaternion.Euler(0, 180, 0), 0.1f);
        }
        else
        {
            transform.rotation = playerCamera.transform.rotation;
        }
    }

    void ApplyTwoHandedGrab(Transform leftHand, Transform rightHand)
    {
        Vector3 combinedPosition = (leftHand.position + rightHand.position) / 2;
        Quaternion combinedRotation = Quaternion.Slerp(leftHand.rotation, rightHand.rotation, 0.5f);
        transform.position = combinedPosition;
        transform.rotation = combinedRotation;
    }
}
