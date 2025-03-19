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
    private AimingInputReference _aimInputRef;
    [SerializeField]
    private MovingInputReference _moveInputRef;

    private Vector2 _moveInput;
    private Coroutine _resetAttackheight;
    private void Start()
    {
        _aimInputRef.variable.ValueChanged += AimInputRef_ValueChanged;
        _aimInputRef.variable.StateManager = _stateManager;
        _moveInputRef.variable.StateManager = _stateManager;
    }

    public void ProcessAimInput(InputAction.CallbackContext ctx)
    {
        _aimInputRef.variable.value = ctx.ReadValue<Vector2>();
    }

    private void AimInputRef_ValueChanged(object sender, AimInputEventArgs e)
    {
        if (_stateManager.AttackState == AttackState.Block ||
            _stateManager.AttackState == AttackState.Parrry ||
            _stateManager.AttackState == AttackState.blockAttack)
        {
            _aimInputRef.variable.State = _stateManager.AttackState;
            return;
        }

            if (_stateManager.AttackState != AttackState.Idle && _aimInputRef.Value == Vector2.zero) _stateManager.AttackState = AttackState.Idle;
        else if(_stateManager.AttackState != AttackState.Attack && _aimInputRef.Value != Vector2.zero) _stateManager.AttackState = AttackState.Attack;

        _aimInputRef.variable.State = _stateManager.AttackState;
    }

    public void ProccesMoveInput(InputAction.CallbackContext ctx)
    {
        _moveInputRef.variable.value = ctx.ReadValue<Vector2>();
    }

    public void ProccesSetBlockInput(InputAction.CallbackContext ctx)
    {
        if (_stateManager.AttackState != AttackState.blockAttack) 
        {
            if (ctx.action.WasPressedThisFrame())
            {
                _stateManager.AttackState = AttackState.Block;
            }

            if (ctx.action.WasReleasedThisFrame())
            {
                _stateManager.AttackState = AttackState.Idle;
            }
        } else if(ctx.performed) _stateManager.AttackState = AttackState.Idle;

        _aimInputRef.variable.State = _stateManager.AttackState;
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

        _aimInputRef.variable.State = _stateManager.AttackState;
    }

    public void ProccesDodgeInput(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        //TODO add doge event
    }

    public void ProccesInteractInput(InputAction.CallbackContext ctx)
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
        if (_stateManager.AttackState != AttackState.Block) return;
        _stateManager.AttackState = AttackState.blockAttack;
        _aimInputRef.variable.State = _stateManager.AttackState;
    }

    private IEnumerator ResetAttackHeight()
    {
        yield return new WaitForSeconds(1);
        _stateManager.AttackHeight = AttackHeight.Torso;
    }
}
