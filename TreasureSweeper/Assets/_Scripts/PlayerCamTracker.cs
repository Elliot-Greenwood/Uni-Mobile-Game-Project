using UnityEngine;

public class PlayerCamTracker : MonoBehaviour
{
    [SerializeField] Transform Player;
    float DampingValue = 2f;

    public float Z;

    Vector3 NewPos = Vector3.zero;
    private void Update()
    {
        
        Z = Player.position.z - 5f;

        if (Z > 10.5f || Z < 9.5f)
        {
            NewPos = new Vector3(transform.position.x, transform.position.y, Z);
            transform.position = Vector3.Lerp(transform.position, NewPos, DampingValue * Time.deltaTime);
        }
    }

        
    



}
