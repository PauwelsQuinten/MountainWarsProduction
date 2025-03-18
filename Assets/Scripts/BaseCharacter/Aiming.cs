using TMPro;
using System;
using UnityEngine;

public enum AimingInputState
{
    Idle, Moving, Hold, Reset
}

public enum Direction
{
    ToRight, ToLeft, ToCenter
}

public class AimingOutputArgs : EventArgs
{
    public Direction Direction;
    public AimingInputState AimingInputState;
    //public Height AttackHeight;
    public float Speed;
    public float AngleTravelled;
}

public class Aiming : MonoBehaviour
{
    [SerializeField] private AimingInputReference _refAimingInput;
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private TextMeshProUGUI _textMeshPro2;
    private Vector2 _vec2previousDirection = Vector2.zero;
    private AttackState _enmCurrentAttackState = AttackState.Idle;
    private AimingInputState _enmAimingInput = AimingInputState.Idle;
    private const float F_MIN_DIFF_BETWEEN_INPUT = 0.0225f;
    private const float F_MAX_TIME_NOT_MOVING = 0.20f;
    private float _fNotMovingTime = 0f;
    private GameEvent _gameEvent;
    
    void Start()
    {
        _enmAimingInput = AimingInputState.Idle;
        _refAimingInput.variable.ValueChanged += Variable_ValueChanged;
    }

    void Update()
    {
        CheckIfHoldingPosition();

        if (_textMeshPro && _textMeshPro2 && _enmAimingInput != AimingInputState.Idle)
        {
            _textMeshPro2.text = $"{_refAimingInput.variable.value}";
            Debug.Log($"{_enmAimingInput}");
        }
            _textMeshPro.text = $"{_enmAimingInput}";

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
        if (_refAimingInput.variable.value == Vector2.zero)
             _enmAimingInput = AimingInputState.Idle;

        else if (DetectAnalogMovement())
             _enmAimingInput = AimingInputState.Moving;


        _gameEvent.Raise(this, )

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

    private void CheckIfHoldingPosition()
    {
        switch( _enmAimingInput )
        {
            case AimingInputState.Idle:
            case AimingInputState.Hold:
                 _fNotMovingTime = 0f;
            break;

            case AimingInputState.Moving:
                if (!DetectAnalogMovement())
                {
                    _fNotMovingTime += Time.deltaTime;
                }
                else
                    _fNotMovingTime = 0f;
            break;
        }

        if (_fNotMovingTime >= F_MAX_TIME_NOT_MOVING)
        {
            _enmAimingInput = AimingInputState.Hold;
            _fNotMovingTime = 0f;
        }

    }



}