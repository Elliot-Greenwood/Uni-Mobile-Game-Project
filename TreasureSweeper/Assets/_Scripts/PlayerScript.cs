
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    InputSub _input;
    CharacterController CharController;

    [SerializeField] Transform CamTargetPoint;

    float PlayerMovementSpeed = 8f;
    public float RotationSpeed = 50f;
    public float Gravity = -9.81f;

    Vector2 PlayerInputValue = Vector2.zero;
    Vector3 PlayerVelocity = Vector3.zero;
    float VerticalVelocity = 0f;


    TileScript TileObject = null;
    //bool PlayerIsOnTile = false;
    private void Awake()
    {
        _input = GetComponent<InputSub>();
        CharController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {

        PlayerInputValue = new Vector2(_input.MoveInput.x, _input.MoveInput.y).normalized;
        PlayerVelocity = CamTargetPoint.forward * PlayerInputValue.y + CamTargetPoint.right * PlayerInputValue.x;

        if (PlayerVelocity.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(PlayerVelocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        }


        if (CharController.isGrounded)
        {
            VerticalVelocity = -1f;
        }
        else
        {
            VerticalVelocity += Gravity * Time.deltaTime;
        }

        PlayerVelocity.y = VerticalVelocity;

        CharController.Move(PlayerVelocity * PlayerMovementSpeed * Time.deltaTime);

        //============================================================

        if (TileObject != null && _input.DigButton)
        {
            TileObject.ActivateTheTile();
        }

        //==========================================================================

        if (_input.FlagButton && TileObject != null && !TileObject.TileIsActivated)
        {
            if (TileObject.TileIsFlagged)
            {
                Debug.Log("RemoveFlag");
                TileObject.RemoveTheFlag();
            }
            else
            {
                Debug.Log("PlacedAFlag");
                TileObject.PlaceTheFlag();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7 && TileObject == null)
        {
            TileObject = other.gameObject.GetComponent<TileScript>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7 && TileObject != null)
        {
            TileObject = null;
        }
    }


}
