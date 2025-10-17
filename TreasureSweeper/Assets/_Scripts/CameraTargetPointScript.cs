using UnityEngine;

public class CameraTargetPointScript : MonoBehaviour
{
    [SerializeField] Transform Camera;

    private void Update()
    {
        transform.eulerAngles = new Vector3(0, Camera.transform.eulerAngles.y, 0);
    }
}
