using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerScript : MonoBehaviour
{
    InputSub _input;
    CharacterController CharController;

    public float PlayerMovementSpeed = 5f;
    public float RotationSpeed = 50f;
    public float Gravity = -9.81f;

    Vector2 PlayerInputValue = Vector2.zero;
    Vector3 PlayerVelocity = Vector3.zero;
    Vector3 MoveDirection = Vector3.zero;
    float VerticalVelocity = 0f;

    private void Awake()
    {
        _input = GetComponent<InputSub>();
        CharController = GetComponent<CharacterController>();
    }


    private void Update()
    {
        PlayerInputValue = new Vector2(_input.MoveInput.x, _input.MoveInput.y);
        MoveDirection = new Vector3(PlayerInputValue.x, 0f, PlayerInputValue.y).normalized;

        if (MoveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(MoveDirection);
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


        PlayerVelocity = MoveDirection * PlayerMovementSpeed;
        PlayerVelocity.y = VerticalVelocity;


        CharController.Move(PlayerVelocity * Time.deltaTime);

    }


}
