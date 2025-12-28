using UnityEngine;

public class GyroCameraTiltScript : MonoBehaviour
{
    [Header("Tilt Settings")]
    public float MaxTiltUpDown = 15f;
    public float MaxTiltSides = 30f;
    public float Xoffset = 55f; //75f if not inverted
    public float TiltDamper = 3f;

    private Quaternion DefaultRotation;
    private Quaternion DefaultGyroRotation;

    private void Start()
    {
        if (PlayerPrefs.GetInt("GyroINT") == 0)
        {
            DefaultGyroRotation = Quaternion.Euler(Xoffset, 0f, 0f);
        }
        else
        {
            //if no gyro
            DefaultRotation = Quaternion.Euler(65f, 0f, 0f);
            transform.localRotation = DefaultRotation;
        }
    }

    void Update()
    {
        if (PlayerPrefs.GetInt("GyroINT") == 0)
        {


            Vector3 GyroInput = Input.acceleration;

            float TiltUpDown = Mathf.Clamp(-GyroInput.y * MaxTiltUpDown, -MaxTiltUpDown, MaxTiltUpDown);
            float TiltToSides = Mathf.Clamp(GyroInput.x * MaxTiltSides, -MaxTiltSides, MaxTiltSides);

            Quaternion QuatUpDown = Quaternion.AngleAxis(TiltUpDown, Vector3.right);
            Quaternion QuatSides = Quaternion.AngleAxis(TiltToSides, Vector3.up);

            Quaternion TargetRotation = DefaultGyroRotation * QuatSides * QuatUpDown;

            transform.localRotation = Quaternion.Slerp(transform.localRotation, TargetRotation, TiltDamper * Time.deltaTime); 
        }
    }
}