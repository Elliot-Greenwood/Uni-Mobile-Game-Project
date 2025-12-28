
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    InputSub _input;
    CharacterController CharController;
    AudioSource SFXPlayer;

    [SerializeField] MineFieldManagerScript GameManager;
    [SerializeField] Animator Anim;

    [SerializeField] Transform CamTargetPoint;


    [SerializeField] AudioClip CoinSound;

    float PlayerMovementSpeed = 7f;
    float RotationSpeed = 15f;
    float Gravity = -9.81f;

    Vector2 PlayerInputValue = Vector2.zero;
    Vector3 PlayerVelocity = Vector3.zero;
    float VerticalVelocity = 0f;
    [SerializeField] bool CanMove = true;


    float ActionDelay = 0f;

    TileScript TileObject = null;
    //bool PlayerIsOnTile = false;
    private void Awake()
    {
        _input = GetComponent<InputSub>();
        CharController = GetComponent<CharacterController>();
        SFXPlayer = GetComponent<AudioSource>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (!GameManager.IsGameComplete && CanMove)
        {
            PlayerInputValue = new Vector2(_input.MoveInput.x, _input.MoveInput.y).normalized;
            PlayerVelocity = CamTargetPoint.forward * PlayerInputValue.y + CamTargetPoint.right * PlayerInputValue.x;

            if (PlayerInputValue != Vector2.zero)
            {
                Anim.SetBool("PlayerIsMoving", true);
            }
            else 
            { 
                Anim.SetBool("PlayerIsMoving", false); 
            }



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
        }

        if (!CanMove)
        {
            PlayerInputValue = Vector2.zero;
            CharController.Move(Vector3.zero);
            Anim.SetBool("PlayerIsMoving", false);
        }

        //============================================================

        if (TileObject != null && _input.DigButton && !TileObject.TileIsActivated && ActionDelay <= 0f) //add !TileObject.TileIsActivated if it bothers that dig is performed still with active tile
        {
            ActionDelay = 1.1f;
            CanMove = false;
            Anim.SetTrigger("PlayerDigTile");
            Invoke("InvokeDigAction", 0.5f);
            Invoke("InvokeMovement", 1f);
        }

        //==========================================================================

        if (ActionDelay > 0)
        {
            ActionDelay -= Time.deltaTime;
        }

        if (_input.FlagButton && TileObject != null && !TileObject.TileIsActivated && ActionDelay <= 0f)
        {
            
            if (TileObject.TileIsFlagged)
            {
                CanMove = false;
                ActionDelay = 1.1f;
                Anim.SetTrigger("PlayerUnflagTile");
                Invoke("InvokeFlagRemoval", 0.5f);
                Invoke("InvokeMovement", 1f);
            }
            else if(!TileObject.TileIsFlagged && GameManager.Flags > 0)
            {
                CanMove = false;
                ActionDelay = 1.1f;
                Anim.SetTrigger("PlayerFlagTile");
                Invoke("InvokeFlagPlacement", 0.6f);
                Invoke("InvokeMovement", 1f);
            }
        }
    }

    void InvokeFlagPlacement()
    {
        TileObject.PlaceTheFlag();
    }

    void InvokeFlagRemoval()
    {
        TileObject.RemoveTheFlag();
    }

    void InvokeMovement()
    {
        if (!GameManager.IsGameComplete)
        {
            CanMove = true;
        }
    }
    void InvokeDigAction()
    {
        TileObject.ActivateTheTile();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7 && TileObject == null)
        {
            TileObject = other.gameObject.GetComponent<TileScript>();
        }

        if (other.gameObject.layer == 9)
        {
            //coin collect
            SFXPlayer.PlayOneShot(CoinSound, 0.8f);

            ActionsListener.OnCoinCollected();

            Destroy(other.gameObject);
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
