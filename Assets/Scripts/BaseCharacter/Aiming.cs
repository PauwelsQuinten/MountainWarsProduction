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

public class Aiming : MonoBehaviour
{
    [SerializeField] private AimingInputReference _refAimingInput;
    [SerializeField] private GameEvent _gameEvent;
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private TextMeshProUGUI _textMeshPro2;
    private Vector2 _vec2previousDirection = Vector2.zero;
    private Vector2 _vec2Start = Vector2.zero;
    private AttackState _enmCurrentAttackState = AttackState.Idle;
    private AimingInputState _enmAimingInput = AimingInputState.Idle;
    private const float F_MIN_DIFF_BETWEEN_INPUT = 0.0225f;
    private const float F_MAX_TIME_NOT_MOVING = 0.20f;

    private float _fNotMovingTime = 0f;
    private float _fMovingTime = 0f;
    
    void Start()
    {
        _enmAimingInput = AimingInputState.Idle;
        _refAimingInput.variable.ValueChanged += Variable_ValueChanged;
    }

    void Update()
    {
        CheckIfHoldingPosition();
        UpdateMovingTime();

        if (_textMeshPro && _textMeshPro2 && _enmAimingInput != AimingInputState.Idle)
        {
            _textMeshPro2.text = $"{_refAimingInput.variable.value}";
            //Debug.Log($"{_enmAimingInput}");
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
        {

            switch(_enmAimingInput)
            {
                case AimingInputState.Idle:
                case AimingInputState.Hold:
                    _vec2Start = _refAimingInput.variable.value;
                    _enmAimingInput = AimingInputState.Moving;
                    _fMovingTime = 0f;

                    break;
            }
        }
      
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
            var dir = CalculateStartDirection();
            Debug.Log($"{dir}");
            var angl = CalculateAngleLength();
            Debug.Log($"distance : {angl}");
            var speed = CalculateSwingSpeed(angl);
            Debug.Log($"speed : {speed}");
            //if (_refAimingInput.variable.State == AttackState.Attack)
            //    SendPackage();
            _vec2Start = _refAimingInput.variable.value;
            _fMovingTime = 0f;
        }

    }

    private void SendPackage()
    {
        Direction direction = CalculateStartDirection();
        var package = new AimingOutputArgs
        {
            AimingInputState =
                _enmAimingInput
                ,
            AngleTravelled = CalculateAngleLength()
                ,
            AttackHeight = _refAimingInput.variable.StateManager.AttackHeight
                ,
            Direction = CalculateStartDirection()
                ,
            Speed = 0f
        };
        _gameEvent.Raise(this, package);
    }

    private Direction CalculateStartDirection()
    {
        float cross = _vec2Start.x * _refAimingInput.variable.value.y - _vec2Start.y * _refAimingInput.variable.value.x;
        if (cross == 0f)
            return Direction.ToCenter;
        return cross > 0 ? Direction.ToLeft : Direction.ToRight;
    }

    private float CalculateAngleLength()
    {
        return Vector2.Angle(_vec2Start, _refAimingInput.variable.value);
    }

    private void UpdateMovingTime()
    {
        _fMovingTime += Time.deltaTime;
    }
     private float CalculateSwingSpeed(float length)
    {
        return (length *1/ _fMovingTime) * 0.01f;
    }
    
}