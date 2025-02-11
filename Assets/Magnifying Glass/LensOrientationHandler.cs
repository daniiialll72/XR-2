using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LensOrientationHandler : MonoBehaviour
{
    public Camera mainCamera;

    void Update()
    {
        this.transform.rotation = mainCamera.transform.rotation;
    }
}
