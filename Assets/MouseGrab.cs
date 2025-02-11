using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MouseGrab : MonoBehaviour
{
    private bool isHolding = false;
    private Transform heldObject;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public float grabDistance = 3.0f;
    public float moveSpeed = 10.0f;

    public XRController leftController;
    public XRController rightController;

    private bool isVRActive = false;

    void Start()
    {
        isVRActive = XRSettings.enabled;
    }

    void Update()
    {
        if (isVRActive)
        {
            HandleVRInteraction();
        }
        else
        {
            HandleKeyboardInteraction();
        }

        if (isHolding && heldObject != null)
        {
            MoveObject();
        }
    }

    void HandleKeyboardInteraction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isHolding)
                TryGrab();
            else
                DropObject();
        }
    }

    void HandleVRInteraction()
    {
        bool leftGrab = leftController.inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool leftGrabPressed) && leftGrabPressed;
        bool rightGrab = rightController.inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool rightGrabPressed) && rightGrabPressed;

        if (!isHolding && (leftGrab || rightGrab))
        {
            TryGrabVR(leftGrab ? leftController.transform : rightController.transform);
        }
        else if (isHolding && !leftGrab && !rightGrab)
        {
            DropObject();
        }
    }

    void TryGrab()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, grabDistance))
        {
            if (hit.collider.CompareTag("Grabbable"))
            {
                heldObject = hit.transform;
                isHolding = true;

                originalPosition = heldObject.position;
                originalRotation = heldObject.rotation;
            }
        }
    }

    void TryGrabVR(Transform hand)
    {
        Collider[] colliders = Physics.OverlapSphere(hand.position, 0.1f);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Grabbable"))
            {
                heldObject = col.transform;
                isHolding = true;

                originalPosition = heldObject.position;
                originalRotation = heldObject.rotation;
                break;
            }
        }
    }

    void DropObject()
    {
        isHolding = false;
        heldObject = null;
    }

    void MoveObject()
    {
        if (heldObject != null)
        {
            Vector3 targetPosition = isVRActive
                ? (leftController.inputDevice.isValid ? leftController.transform.position : rightController.transform.position)
                : Camera.main.transform.position + Camera.main.transform.forward * grabDistance;

            heldObject.position = Vector3.Lerp(heldObject.position, targetPosition, Time.deltaTime * moveSpeed);
        }
    }
}
