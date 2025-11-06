using UnityEngine;

public class PlayerCamTracker : MonoBehaviour
{
    [SerializeField] Transform Player;

    [Header("Damping Settings")]
    float DampingX = 7f;
    float DampingZ = 3f;

    Vector3 targetPos;

    private void Update()
    {
        
        targetPos = new Vector3(Player.position.x, transform.position.y, Player.position.z - 2f);

        
        float newX = Mathf.Lerp(transform.position.x, targetPos.x, DampingX * Time.deltaTime);
        float newZ = Mathf.Lerp(transform.position.z, targetPos.z, DampingZ * Time.deltaTime);

        transform.position = new Vector3(newX, transform.position.y, newZ);
    }
}