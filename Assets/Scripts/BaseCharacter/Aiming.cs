using TMPro;
using UnityEngine;

public enum AimingInputState
{
    Idle, Moving, Hold, Reset
}
public class Aiming : MonoBehaviour
{
    [SerializeField] private AimingInputReference _refAimingInput;
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    private Vector2 _vec2previousDirection = Vector2.zero;
    private AttackState _enmCurrentAttackState = AttackState.Idle;
    private AimingInputState _enmAimingInput = AimingInputState.Idle;
    private const float F_MIN_DIFF_BETWEEN_INPUT = 0.00125f;
    
    void Start()
    {
        _enmAimingInput = AimingInputState.Idle;
        _refAimingInput.variable.ValueChanged += Variable_ValueChanged;
    }

    private void Variable_ValueChanged(object sender, AimInputEventArgs e)
    {
        switch(e.ThisChanged)
        {
            case AimInputEventArgs.WhatChanged.Input:
                OnInputChanged();
                break;
            case AimInputEventArgs.WhatChanged.State:
                OnStateChanged();
                break;
            default:
                break;

        }
    }

    private void OnInputChanged()
    {
        switch (_enmAimingInput)
        {
            case AimingInputState.Idle:
                if (DetectAnalogMovement())
                {
                    _enmAimingInput = AimingInputState.Moving;
                }
                break;

            case AimingInputState.Moving:
                if (!DetectAnalogMovement())
                {
                    _enmAimingInput = 
                        (_refAimingInput.variable.value == Vector2.zero)? AimingInputState.Reset : AimingInputState.Moving;

                }
                break;

            case AimingInputState.Hold:
                break;

            case AimingInputState.Reset:
                break;
            
            default:
                break;
        }
        if (_textMeshPro)
            _textMeshPro.text = $"{_enmAimingInput}";
    }

    private void OnStateChanged()
    {
        if (_enmCurrentAttackState != _refAimingInput.variable.State)
        {
            _enmCurrentAttackState = _refAimingInput.variable.State;
        }


    }


    //--------------------------------------------------------------
    //Helper Functions

    private bool DetectAnalogMovement()
    {
        var diff = _vec2previousDirection - _refAimingInput.variable.value;
        bool value = diff.magnitude > F_MIN_DIFF_BETWEEN_INPUT;
       
        _vec2previousDirection = _refAimingInput.variable.value;
        return value;
    }



}