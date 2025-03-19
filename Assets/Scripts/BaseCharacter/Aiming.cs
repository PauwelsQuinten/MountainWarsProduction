using TMPro;
using System;
using UnityEngine;
using System.Collections;

public enum AimingInputState
{
    Idle, Moving, Hold, Reset
}

public enum Direction
{
    ToRight, ToLeft, ToCenter
}

public enum AttackSignal
{
    Idle, Stab, Feint, Swing
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
    private AttackSignal _enmAttackSignal = AttackSignal.Swing;
    private const float F_MIN_DIFF_BETWEEN_INPUT = 0.0225f;
    private const float F_MAX_TIME_NOT_MOVING = 0.20f;
    private const float F_MIN_ACCEPTED_VALUE = 0.40f;
    private const float F_TIME_BETWEEN_SWING = 0.40f;
    private const float F_TIME_BETWEEN_STAB = 0.20f;
    private const float F_MAX_ALLOWED_ANGLE_ON_ORIENTATION = 17f;

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
        if (_enmAimingInput == AimingInputState.Reset)
            return;

        float inputLength = _refAimingInput.variable.value.magnitude;

        if (_refAimingInput.variable.value == Vector2.zero || inputLength < F_MIN_ACCEPTED_VALUE)
        {
            _enmAimingInput = AimingInputState.Idle;
            _enmAttackSignal = AttackSignal.Idle;
        }


        else if (inputLength >= 0.9f 
            && _refAimingInput.variable.StateManager.Orientation == CalculateOrientationOfInput(_refAimingInput.variable.value)
            && _refAimingInput.variable.State == AttackState.Attack
            && _enmAttackSignal == AttackSignal.Idle
            && CalculateAngleLengthDegree() < F_MAX_ALLOWED_ANGLE_ON_ORIENTATION)
        {
            _enmAttackSignal = AttackSignal.Stab;
            StartCoroutine(ResetAttack(F_TIME_BETWEEN_STAB));

            Debug.Log("Stab");
            //SendPackage(_refAimingInput.variable.State);
        }


        else if (DetectAnalogMovement() && _enmAttackSignal == AttackSignal.Idle)
        {
            switch (_enmAimingInput)
            {
                case AimingInputState.Idle:
                case AimingInputState.Hold:
                    _vec2Start = _refAimingInput.variable.value;
                    _enmAimingInput = AimingInputState.Moving;
                    _fMovingTime = 0f;
                    _vec2previousDirection = Vector2.zero;
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
            case AimingInputState.Reset:
                return;
            case AimingInputState.Idle:
            case AimingInputState.Hold:
                 _fNotMovingTime = 0f;
            break;

            case AimingInputState.Moving:
                if (!DetectAnalogMovement())
                {
                    _fNotMovingTime += Time.deltaTime;
                    //Debug.Log($"no movement detected : {_fNotMovingTime}");
                }
                else
                    _fNotMovingTime = 0f;
            break;
        }

        if (_fNotMovingTime >= F_MAX_TIME_NOT_MOVING)
        {
            _fNotMovingTime = 0f;

            switch(_refAimingInput.variable.State)
            {
                case AttackState.Attack:
                    _enmAimingInput = AimingInputState.Reset;
                    StartCoroutine(ResetAttack(F_TIME_BETWEEN_SWING));
                    break;

                case AttackState.Block:
                    _enmAimingInput = AimingInputState.Hold;

                    break;

                default:
                    break;
            }
            
            var dir = CalculateSwingDirection();
            Debug.Log($"{dir}");
            var angl = CalculateAngleLengthDegree();
            Debug.Log($"distance : {angl}");
            var speed = CalculateSwingSpeed(angl);
            Debug.Log($"speed : {speed}");
            var feint = IsFeintMovement();
            Debug.Log($"signal : {feint}");
             
             //SendPackage(_refAimingInput.variable.State);
            _vec2Start = _refAimingInput.variable.value;
            _fMovingTime = 0f;

           
        }

    }

    private void SendPackage(AttackState attackState)
    {
        Direction direction = CalculateSwingDirection();
        float distance = CalculateAngleLengthDegree();
        var package = new AimingOutputArgs
        {
            AimingInputState = _enmAimingInput
                ,
            AngleTravelled = CalculateAngleLengthDegree()
                ,
            AttackHeight = _refAimingInput.variable.StateManager.AttackHeight
                ,
            Direction = CalculateSwingDirection()
                ,
            Speed = CalculateSwingSpeed(distance)
                ,
            AttackSignal = _enmAttackSignal
        };
        _gameEvent.Raise(this, package);
    }

    private Direction CalculateSwingDirection()
    {
        float cross = _vec2Start.x * _refAimingInput.variable.value.y - _vec2Start.y * _refAimingInput.variable.value.x;
        if (cross == 0f)
            return Direction.ToCenter;
        return cross > 0 ? Direction.ToLeft : Direction.ToRight;
    }

    private float CalculateAngleLengthDegree()
    {
        //Debug.Log($"Distance calculation. start = {_vec2Start}, end = {_refAimingInput.variable.value}");
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
    
     private float CalculateAngleOfInput(Vector2 direction)
    {
        return Mathf.Atan2(direction.y, direction.x);
    }
     private Orientation CalculateOrientationOfInput(Vector2 direction)
    {
        //Debug.Log($"{Mathf.Atan2(_refAimingInput.variable.value.y, _refAimingInput.variable.value.x)}");
        float angle = CalculateAngleOfInput(direction) * Mathf.Rad2Deg;
        if (angle == 0)
            return Orientation.East;

        int newAngle = (int) angle / 45;

        return (Orientation)(newAngle * 45) ;
    }

    private AttackSignal IsFeintMovement()
    {
        var startAngleRad = CalculateAngleOfInput(_vec2Start);
        var endAngleRad = CalculateAngleOfInput(_refAimingInput.variable.value);
        var orientAngleRad = (int)_refAimingInput.variable.StateManager.Orientation * Mathf.Deg2Rad;

        if (orientAngleRad > startAngleRad && orientAngleRad < endAngleRad)
            return AttackSignal.Feint;
        return AttackSignal.Swing;
    }

    private IEnumerator ResetAttack(float time)
    {
        yield return new WaitForSeconds(time);
        _enmAimingInput = AimingInputState.Idle;
    }

}