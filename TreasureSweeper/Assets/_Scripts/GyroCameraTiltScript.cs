using UnityEngine;

public class GyroCameraTiltScript : MonoBehaviour
{
    [Header("Tilt Settings")]
    public float MaxTiltX = 10f;
    public float MaxTiltZ = 10f;
    public float Smooth = 3f;

    void Update()
    {
        Vector3 gyro = Input.acceleration; // device tilt
        float tiltX = Mathf.Clamp(gyro.x * MaxTiltX, -MaxTiltX, MaxTiltX);
        float tiltZ = Mathf.Clamp(gyro.y * MaxTiltZ, -MaxTiltZ, MaxTiltZ);

        Quaternion targetRot = Quaternion.Euler(tiltZ, 0f, -tiltX);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, Smooth * Time.deltaTime);
    }
}
