using UnityEngine;

public class CameraControl : MonoBehaviour  
{
    public float rotationSpeed = 3.0f;
    public float zoomSpeed = 10.0f;
    public float minZoom = 10f;
    public float maxZoom = 100f;

    private float currentZoom = 50f;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed;
        float verticalInput = -Input.GetAxis("Mouse Y") * rotationSpeed;

        transform.Rotate(Vector3.up, horizontalInput, Space.World);
        transform.Rotate(Vector3.right, verticalInput, Space.Self);

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        currentZoom -= scrollInput * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        Camera.main.fieldOfView = currentZoom;
    }
}
