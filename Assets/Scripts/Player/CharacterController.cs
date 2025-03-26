using System;
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
    [SerializeField]
    private GameEvent _pickupEvent;

    private Vector2 _moveInput;
    private Coroutine _resetAttackheight;
    private AttackState _storredAttackState = AttackState.Idle;

    private bool _wasSprinting;
    private bool _isHoldingShield;
    private void Start()
    {
        _aimInputRef.variable.ValueChanged += AimInputRef_ValueChanged;
        _aimInputRef.variable.StateManager = _stateManager;
        _moveInputRef.variable.StateManager = _stateManager;
    }

    public void GetKnockBack(Component sender, object obj)
    {
        if (sender.gameObject != gameObject) return;

        _storredAttackState = _stateManager.AttackState;
        _stateManager.AttackState = AttackState.Knock;
        _aimInputRef.variable.State = AttackState.Knock;
    }
    
    public void RecoverKnockBack(Component sender, object obj)
    {
        if (sender.gameObject != gameObject) return;

        _stateManager.AttackState = _storredAttackState;
        _aimInputRef.variable.State = _storredAttackState;
    }


    public void ProcessAimInput(InputAction.CallbackContext ctx)
    {
        _aimInputRef.variable.value = ctx.ReadValue<Vector2>();
    }

    private void AimInputRef_ValueChanged(object sender, AimInputEventArgs e)
    {
        if (_stateManager.AttackState == AttackState.Knock)
        {
            return;
        }


        if (_stateManager.AttackState == AttackState.ShieldDefence ||
            _stateManager.AttackState == AttackState.SwordDefence ||
            _stateManager.AttackState == AttackState.BlockAttack)
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
        if (_stateManager.AttackState == AttackState.Knock)
        {
            if (ctx.action.WasPressedThisFrame())
            {

                _storredAttackState = AttackState.ShieldDefence;
            }

            if (ctx.action.WasReleasedThisFrame())
            {

                _storredAttackState = AttackState.Idle;
            }
            return;
        }


        if (_stateManager.AttackState != AttackState.BlockAttack)
        {
            if (ctx.action.WasPressedThisFrame())
            {
                _stateManager.AttackState = AttackState.ShieldDefence;
            }

            if (ctx.action.WasReleasedThisFrame())
            {
                _stateManager.AttackState = AttackState.Idle;
            }
        }
        else if (ctx.performed)
        {
            _stateManager.AttackState = AttackState.ShieldDefence;
            _isHoldingShield = false;
            _stateManager.IsHoldingShield = _isHoldingShield;
        }

            _aimInputRef.variable.State = _stateManager.AttackState;
    }

    public void ProccesSetParryInput(InputAction.CallbackContext ctx)
    {
        if (_stateManager.AttackState == AttackState.Knock)
        {
            if (ctx.action.WasPressedThisFrame())
            {
               
                _storredAttackState = AttackState.SwordDefence;
            }

            if (ctx.action.WasReleasedThisFrame())
            {
                
                _storredAttackState = AttackState.Idle;
            }
            return;
        }

        if (ctx.action.WasPressedThisFrame())
        {
            if (_stateManager.AttackState == AttackState.BlockAttack) _isHoldingShield = true;

            _stateManager.IsHoldingShield = _isHoldingShield;
            _stateManager.AttackState = AttackState.SwordDefence;
        }

        if (ctx.action.WasReleasedThisFrame())
        {
            if (_isHoldingShield) _stateManager.AttackState = AttackState.BlockAttack;
            else _stateManager.AttackState = AttackState.Idle;
        }


        _aimInputRef.variable.State = _stateManager.AttackState;
    }

    public void ProccesDodgeInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _wasSprinting = true;
            _moveInputRef.variable.SpeedMultiplier = 1.5f;
        }
        if (ctx.canceled && _wasSprinting)
        {
            _wasSprinting = false;
            _moveInputRef.variable.SpeedMultiplier = 1;
            return;
        }

            //TODO add doge event
    }

    public void ProccesInteractInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            _pickupEvent.Raise();
        //TODO add intract event
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
        if (_stateManager.AttackState != AttackState.ShieldDefence) return;
        _isHoldingShield = true;
        _stateManager.IsHoldingShield = _isHoldingShield;
        _stateManager.AttackState = AttackState.BlockAttack;
        _aimInputRef.variable.State = _stateManager.AttackState;
    }

    private IEnumerator ResetAttackHeight()
    {
        yield return new WaitForSeconds(1);
        _stateManager.AttackHeight = AttackHeight.Torso;
    }
}
