using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [Header("State Manager")]
    [SerializeField]
    private StateManager _stateManager;

    [Header("Variables")]
    [SerializeField]
    private AimingInputReference _AimInputRef;

    private Vector2 _moveInput;
    private void Start()
    {
        _AimInputRef.variable.ValueChanged += AimInputRef_ValueChanged;
    }

    public void ProcessAimInput(InputAction.CallbackContext ctx)
    {
        _AimInputRef.variable.value = ctx.ReadValue<Vector2>();
        _AimInputRef.variable.State = _stateManager.AttackState;
    }

    private void AimInputRef_ValueChanged(object sender, AimInputEventArgs e)
    {
        if (_stateManager.AttackState == AttackState.Block) return;
        if (_stateManager.AttackState != AttackState.Idle && _AimInputRef.Value == Vector2.zero) _stateManager.AttackState = AttackState.Idle;
        else if(_stateManager.AttackState != AttackState.Attack && _AimInputRef.Value != Vector2.zero) _stateManager.AttackState = AttackState.Attack;
        _AimInputRef.variable.State = _stateManager.AttackState;
    }

    public void ProccesMoveInput(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();
        //TODO call move funtion trough event?
    }

    public void ProccesSetBlockInput(InputAction.CallbackContext ctx)
    {
        if (ctx.action.WasPressedThisFrame())
        {
            _stateManager.AttackState = AttackState.Block;
        }

        if (ctx.action.WasReleasedThisFrame())
        {
            _stateManager.AttackState = AttackState.Idle;
        }

        _AimInputRef.variable.State = _stateManager.AttackState;
    }
}
