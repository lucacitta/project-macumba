using UnityEngine;
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {

    public float sensitivityX = 6f;
    public float sensitivityY = 6f;

    public float minimumX = -360f;
    public float maximumX = 360f;

    public float minimumY = -60f;
    public float maximumY = 60f;

    float rotationX = 0f;

    Quaternion originalRotation;

    void Update ()
    {
        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        rotationX = ClampAngle (rotationX, minimumX, maximumX);

        Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
        transform.localRotation = originalRotation * xQuaternion;
    }

    void Start ()
    {
        originalRotation = transform.localRotation;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static float ClampAngle (float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp (angle, min, max);
    }
}