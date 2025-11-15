using UnityEngine;

public class CameraMouseControl : MonoBehaviour
{
    public float sensitivityX = 150f;
    public float sensitivityY = 150f;
    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivityY;
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
