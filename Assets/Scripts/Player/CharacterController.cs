using System.Collections;
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
    private Coroutine _resetAttackheight;
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
        if (_stateManager.AttackState == AttackState.Block || _stateManager.AttackState == AttackState.Parrry) return;
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

    public void ProccesSetParryInput(InputAction.CallbackContext ctx)
    {
        if (ctx.action.WasPressedThisFrame())
        {
            _stateManager.AttackState = AttackState.Parrry;
        }

        if (ctx.action.WasReleasedThisFrame())
        {
            _stateManager.AttackState = AttackState.Idle;
        }

        _AimInputRef.variable.State = _stateManager.AttackState;
    }

    public void ProccesDodgeInput(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        //TODO add doge event
    }

    public void ProccesPickUpInput(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        //TODO add dodge event
    }

    public void ProccesAtackHeightInput(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        _stateManager.AttackHeight = AttackHeight.Head;

        if (_resetAttackheight != null) StopCoroutine(_resetAttackheight);
        _resetAttackheight = StartCoroutine(ResetAttackHeight());
    }

    public void ProssesLockShieldInput(InputAction.CallbackContext ctx)
    {
        if(!ctx.performed) return;
        _stateManager.AttackState = AttackState.blockAttack;
    }

    private IEnumerator ResetAttackHeight()
    {
        yield return new WaitForSeconds(1);
        _stateManager.AttackHeight = AttackHeight.Torso;
    }
}
