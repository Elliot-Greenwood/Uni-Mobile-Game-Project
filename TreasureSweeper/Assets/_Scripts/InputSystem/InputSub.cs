using UnityEngine;
using UnityEngine.InputSystem;

public class InputSub : MonoBehaviour
{

    public Vector2 MoveInput { get; private set; } = Vector2.zero;
    public bool DigButton { get; private set; } = false;
    public bool FlagButton { get; private set; } = false;

    PlayerInputMap _input = null;


    private void OnEnable()
    {
        _input = new PlayerInputMap();
        _input.PLInputs.Enable();

        _input.PLInputs.Movement.performed += SetMove;
        _input.PLInputs.Movement.canceled += SetMove;
    }

    private void OnDisable()
    {
        _input.PLInputs.Movement.performed -= SetMove;
        _input.PLInputs.Movement.canceled -= SetMove;

        _input.PLInputs.Disable();
    }

    private void Update()
    {
        DigButton = _input.PLInputs.Dig.WasPressedThisFrame();
        FlagButton = _input.PLInputs.PlantAFlag.WasPressedThisFrame();
    }

    private void SetMove(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }
   



}
