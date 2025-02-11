using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    public Animator handAnimator;

    private bool isVRActive;
    private InputDevice leftHand;
    private InputDevice rightHand;

    void Start()
    {
        isVRActive = XRSettings.enabled;
        leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    void Update()
    {
        UpdateHandAnimation();
    }

    void UpdateHandAnimation()
    {
        float triggerValue = 0f;
        float gripValue = 0f;

        if (isVRActive)
        {
            leftHand.TryGetFeatureValue(CommonUsages.trigger, out float leftTrigger);
            rightHand.TryGetFeatureValue(CommonUsages.trigger, out float rightTrigger);
            triggerValue = Mathf.Max(leftTrigger, rightTrigger);

            leftHand.TryGetFeatureValue(CommonUsages.grip, out float leftGrip);
            rightHand.TryGetFeatureValue(CommonUsages.grip, out float rightGrip);
            gripValue = Mathf.Max(leftGrip, rightGrip);
        }
        else
        {
            triggerValue = Input.GetMouseButton(0) ? 1f : 0f;
            gripValue = Input.GetMouseButton(1) ? 1f : 0f;
        }

        handAnimator.SetFloat("Trigger", triggerValue);
        handAnimator.SetFloat("Grip", gripValue);
    }
}
